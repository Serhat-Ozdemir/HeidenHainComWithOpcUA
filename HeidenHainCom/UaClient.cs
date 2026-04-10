using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using OPCUaClient.Exceptions;
using OPCUaClient.Objects;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

namespace OPCUaClient
{
    public class UaClient
    {
        #region Private Fields

        private readonly ConfiguredEndpoint _endpoint;
        public Session? _session = null;
        private readonly UserIdentity _userIdentity;
        private readonly ApplicationConfiguration _appConfig;
        private const int ReconnectPeriod = 10000;
        private readonly object _lock = new object();
        private SessionReconnectHandler? _reconnectHandler;

        #endregion

        #region Private helpers

        private void Reconnect(object sender, EventArgs e)
        {
            if (!ReferenceEquals(sender, _reconnectHandler)) return;
            lock (_lock)
            {
                if (_reconnectHandler?.Session != null)
                    _session = (Session)_reconnectHandler.Session;
                _reconnectHandler?.Dispose();
                _reconnectHandler = null;
            }
        }

        private Subscription Subscription(int milliseconds) => new Subscription
        {
            PublishingEnabled = true,
            PublishingInterval = milliseconds,
            Priority = 1,
            KeepAliveCount = 10,
            LifetimeCount = 20,
            MaxNotificationsPerPublish = 1000
        };

        private void WaitForReconnect()
        {
            const int interval = 500;
            const int timeout = 30000;
            int waited = 0;
            while (waited < timeout)
            {
                lock (_lock) { if (_reconnectHandler == null) return; }
                Thread.Sleep(interval);
                waited += interval;
            }
            throw new InvalidOperationException("Reconnect timed out after 30 seconds.");
        }

        #endregion

        public bool IsConnected => _session is { Connected: true };

        #region Constructor — DER cert + PEM key (Heidenhain)

        public UaClient(string appName, string serverUrl, bool security, bool untrusted,
                        string certPath, string keyPath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            string hostName = System.Net.Dns.GetHostName();
            CreateCertDirs(path);

            var userCertWithoutKey = new X509Certificate2(certPath);
            using var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(keyPath));
            _userIdentity = new UserIdentity(userCertWithoutKey.CopyWithPrivateKey(rsa));

            _appConfig = BuildAppConfig(appName, hostName, path, untrusted);
            InitApp(appName, _appConfig);

            var endpointDesc = CoreClientUtils.SelectEndpoint(serverUrl, true);
            var endpointConfig = EndpointConfiguration.Create(_appConfig);
            _endpoint = new ConfiguredEndpoint(null, endpointDesc, endpointConfig);
        }

        private static void CreateCertDirs(string basePath)
        {
            foreach (var sub in new[] { "", "Application", "Trusted", "TrustedPeer", "Rejected" })
                Directory.CreateDirectory(Path.Combine(basePath, sub));
        }

        private static void InitApp(string appName, ApplicationConfiguration cfg)
        {
            Utils.SetTraceMask(0);
            var app = new ApplicationInstance
            {
                ApplicationName = appName,
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = cfg
            };
            app.CheckApplicationInstanceCertificate(true, 2048).GetAwaiter().GetResult();
        }

        private static ApplicationConfiguration BuildAppConfig(string appName, string hostName,
                                                               string certBasePath, bool untrusted)
        {
            var cfg = new ApplicationConfiguration
            {
                ApplicationName = appName,
                ApplicationUri = $"urn:{hostName}/{appName}",
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StorePath = Path.Combine(certBasePath, "Application"),
                        SubjectName = $"CN={appName}, DC={hostName}"
                    },
                    TrustedIssuerCertificates = new CertificateTrustList
                    { StoreType = "Directory", StorePath = Path.Combine(certBasePath, "Trusted") },
                    TrustedPeerCertificates = new CertificateTrustList
                    { StoreType = "Directory", StorePath = Path.Combine(certBasePath, "TrustedPeer") },
                    RejectedCertificateStore = new CertificateTrustList
                    { StoreType = "Directory", StorePath = Path.Combine(certBasePath, "Rejected") },
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true,
                    RejectSHA1SignedCertificates = false
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 20000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                TraceConfiguration = new TraceConfiguration { DeleteOnLoad = true },
                DisableHiResClock = false
            };

            cfg.Validate(ApplicationType.Client).GetAwaiter().GetResult();

            cfg.CertificateValidator.CertificateValidation += (s, e) =>
            {
                if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted ||
                    e.Error.StatusCode == StatusCodes.BadCertificateChainIncomplete)
                    e.Accept = untrusted;
            };

            return cfg;
        }

        #endregion

        #region Connect / Disconnect

        public void Connect(uint timeOut = 5, bool keepAlive = false)
        {
            Disconnect();
            _session = Task.Run(async () =>
                await Session.Create(_appConfig, _endpoint, false, false,
                    _appConfig.ApplicationName, timeOut * 1000, _userIdentity, null))
                .GetAwaiter().GetResult();

            _session.KeepAliveInterval = 5000;
            if (keepAlive) _session.KeepAlive += KeepAlive;
            if (_session == null || !_session.Connected)
                throw new ServerException("Session was not connected after creation.");
        }

        private void KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (!ServiceResult.IsBad(e.Status)) return;
            lock (_lock)
            {
                if (_reconnectHandler != null) return;
                _reconnectHandler = new SessionReconnectHandler(true);
                _reconnectHandler.BeginReconnect(_session, ReconnectPeriod, Reconnect);
            }
        }

        public void Disconnect()
        {
            if (_session is { Connected: true })
            {
                foreach (var sub in _session.Subscriptions ?? Enumerable.Empty<Subscription>())
                    sub.Delete(true);
                _session.Close();
                _session.Dispose();
                _session = null;
            }
        }

        #endregion

        #region Browse — synchronous

        /// <summary>
        /// Returns top-level devices. Each device gets one level of children (non-recursive).
        /// Children that are folder-type Objects get a placeholder so the tree can lazy-load them.
        /// </summary>
        public List<Device> Devices(bool recursive = false)
        {
            var browser = new Browser(_session)
            {
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable,
                ReferenceTypeId = ReferenceTypeIds.Organizes
            };

            var browseResults = browser.Browse(Opc.Ua.ObjectIds.ObjectsFolder);

            var devices = browseResults
                .Where(d => d.BrowseName.Name != "Server")
                .Select(b => new Device
                {
                    // ── FIX: store the raw BrowseName so Name never gets truncated ──
                    Name = b.BrowseName.Name,
                    Address = b.BrowseName.Name,          // use Name as address root
                    Identifier = b.NodeId.Identifier is uint id ? (int)id : 0,
                    NameSpaceIndex = b.NodeId.NamespaceIndex
                }).ToList();

            devices.ForEach(d =>
                d.Groups = Groups(d.Address, d.Identifier, d.NameSpaceIndex, recursive));

            return devices;
        }

        /// <summary>
        /// Browse one level (or recursively) of children under a node.
        /// Groups are returned with <see cref="Group.Name"/> set to the raw BrowseName,
        /// NOT parsed from the address — this fixes the ".h → h" truncation bug.
        /// </summary>
        public List<Group> Groups(string address, int identifier, int nameSpaceIndex,
                                  bool recursive = false, HashSet<string>? visited = null)
        {
            visited ??= new HashSet<string>();
            var groups = new List<Group>();

            var browser = new Browser(_session)
            {
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable | (int)NodeClass.Method,
                ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                IncludeSubtypes = true
            };

            ReferenceDescriptionCollection browseResults;
            try
            {
                browseResults = browser.Browse(new NodeId((uint)identifier, (ushort)nameSpaceIndex));
            }
            catch (ServiceResultException ex) when (ex.StatusCode == StatusCodes.BadUserAccessDenied)
            {
                return groups;
            }

            foreach (var result in browseResults.OrderBy(r => r.BrowseName.Name))
            {
                var childNodeId = result.NodeId;
                string uniqueKey = $"{childNodeId.NamespaceIndex}:{childNodeId.Identifier}";
                if (!visited.Add(uniqueKey)) continue;

                var group = new Group
                {
                    // ── FIX: Name stored directly from BrowseName, not parsed from address ──
                    Name = result.BrowseName.Name,
                    Address = address + "." + result.BrowseName.Name,
                    Identifier = childNodeId.Identifier is uint id ? (int)id : 0,
                    NameSpaceIndex = childNodeId.NamespaceIndex,
                    NodeClass = result.NodeClass
                };

                if (recursive)
                    group.Groups = Groups(group.Address, group.Identifier,
                                          group.NameSpaceIndex, true, visited);

                groups.Add(group);
            }

            return groups;
        }

        #endregion

        #region Read / Write

        public DataValue Read(Group grp)
        {
            WaitForReconnect();
            var currentSession = _session;
            if (currentSession == null || !currentSession.Connected)
                throw new ServerException("Not connected to server.");
            var readValues = new ReadValueIdCollection
            {
                new ReadValueId
                {
                    NodeId      = new NodeId((uint)grp.Identifier, (ushort)grp.NameSpaceIndex),
                    AttributeId = Attributes.Value
                }
            };
            currentSession!.Read(null, 0, TimestampsToReturn.Both, readValues,
                out DataValueCollection dataValues, out _);
            return dataValues[0];
        }

        public void Write(Group grp, object value)
        {
            WaitForReconnect();
            var currentSession = _session;
            if (currentSession == null || !currentSession.Connected)
                throw new ServerException("Not connected to server.");
            var writeValues = new WriteValueCollection
            {
                new WriteValue
                {
                    NodeId      = new NodeId((uint)grp.Identifier, (ushort)grp.NameSpaceIndex),
                    AttributeId = Attributes.Value,
                    Value       = new DataValue { Value = value }
                }
            };
            currentSession!.Write(null, writeValues, out StatusCodeCollection statusCodes, out _);
            if (!StatusCode.IsGood(statusCodes[0]))
                throw new WriteException("Write failed. Code: " + statusCodes[0].Code);
        }

        #endregion

        #region CallMethod

        public IList<object> CallMethod(NodeId objectId, NodeId methodId, params object[] inputArguments)
        {
            WaitForReconnect();
            if (_session == null || !_session.Connected)
                throw new InvalidOperationException("Not connected to the server.");

            var inputArgs = new VariantCollection(inputArguments.Select(a => new Variant(a)));

            _session.Call(null,
                new CallMethodRequestCollection
                {
                    new CallMethodRequest
                    {
                        ObjectId       = objectId,
                        MethodId       = methodId,
                        InputArguments = inputArgs
                    }
                },
                out var results, out _);

            if (results == null || results.Count == 0)
                throw new Exception("No result returned from method call.");
            if (StatusCode.IsBad(results[0].StatusCode))
                throw new Exception($"Method call failed: {results[0].StatusCode} \n {objectId}, {methodId}");

            return results[0].OutputArguments.Select(v => v.Value).ToList();
        }

        #endregion

        #region File operations (OPC UA FileType — standard node IDs)

        /// <summary>
        /// Opens a file. Mode: "1"=Read, "2"=Write/append, "6"=Write/erase existing.
        /// Returns file handle (>0) on success, throws on failure.
        /// </summary>
        public uint OpenFile(NodeId objectId, string mode)
        {
            if (!byte.TryParse(mode, out byte modeByte))
                throw new ArgumentException($"Invalid file open mode: '{mode}'");

            WaitForReconnect();

            var inputArgs = new VariantCollection
            {
                new Variant(modeByte, new TypeInfo(BuiltInType.Byte, ValueRanks.Scalar))
            };

            _session!.Call(null,
                new CallMethodRequestCollection
                {
                    new CallMethodRequest
                    {
                        ObjectId       = objectId,
                        MethodId       = new NodeId(11580, 0), // FileType_Open
                        InputArguments = inputArgs
                    }
                },
                out var results, out _);

            if (results == null || results.Count == 0 || StatusCode.IsBad(results[0].StatusCode))
                throw new Exception($"OpenFile failed: {results?[0].StatusCode}");

            return Convert.ToUInt32(results[0].OutputArguments[0].Value);
        }

        /// <summary>
        /// Reads <paramref name="length"/> bytes from an open file handle.
        /// Pass -1 to read the entire file (sends Int32.MaxValue as length).
        /// </summary>
        public string ReadFile(NodeId objectId, uint fileHandle, int length)
        {
            WaitForReconnect();
            // OPC UA Read method expects Int32 length; -1 means "read all"
            int readLength = length <= 0 ? int.MaxValue : length;
            var result = CallMethod(objectId, new NodeId(11585, 0), fileHandle, readLength);

            if (result == null || result.Count == 0 || result[0] is not byte[] data || data.Length == 0)
                return string.Empty;

            return System.Text.Encoding.UTF8.GetString(data);
        }

        /// <summary>Writes UTF-8 encoded content to an open file handle.</summary>
        public void WriteFile(NodeId objectId, uint fileHandle, string content)
        {
            WaitForReconnect();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(content);

            var inputArgs = new VariantCollection
            {
                new Variant(fileHandle, new TypeInfo(BuiltInType.UInt32,     ValueRanks.Scalar)),
                new Variant(data,       new TypeInfo(BuiltInType.ByteString, ValueRanks.Scalar))
            };

            _session!.Call(null,
                new CallMethodRequestCollection
                {
                    new CallMethodRequest
                    {
                        ObjectId       = objectId,
                        MethodId       = new NodeId(11588, 0), // FileType_Write
                        InputArguments = inputArgs
                    }
                },
                out var results, out _);

            if (results == null || results.Count == 0 || StatusCode.IsBad(results[0].StatusCode))
                throw new Exception($"WriteFile failed: {results?[0].StatusCode}");
        }

        /// <summary>Closes an open file handle. Silently swallows errors (session may be closing).</summary>
        public void CloseFile(NodeId objectId, uint fileHandle)
        {
            try { CallMethod(objectId, new NodeId(11583, 0), fileHandle); }
            catch { /* ignore — file will be auto-closed when session ends */ }
        }

        /// <summary>
        /// Browses the children of a file node to find the Size variable,
        /// reads it and returns the file size in bytes. Returns -1 if not found.
        /// </summary>
        public int GetFileSize(Group fileNode)
        {
            try
            {
                // First check if we already have Size in the cached groups
                var sizeVar = fileNode.Groups.FirstOrDefault(g => g.Name == "Size");

                if (sizeVar == null)
                {
                    // Browse children to find it
                    var children = Groups(fileNode.Address, fileNode.Identifier,
                                          fileNode.NameSpaceIndex, recursive: false);
                    sizeVar = children.FirstOrDefault(g => g.Name == "Size");
                }

                if (sizeVar == null) return -1;

                var result = Read(sizeVar);
                if (result?.Value == null) return -1;
                return Convert.ToInt32(result.Value);
            }
            catch { return -1; }
        }

        #endregion

        #region Heidenhain-specific helpers

        /// <summary>
        /// Returns all active error groups from the Heidenhain OPC UA server.
        /// Each returned Group represents one error, its children are the error fields.
        /// </summary>
        public async Task<List<Group>> GetErrorsAsync(CancellationToken ct = default)
        {
            // AllActiveErrors node: ns=1; i=53001
            var allErrors = await Task.Run(() =>
                Groups("AllActiveErrors", 53001, 1, recursive: true), ct);

            // Remove container nodes — keep only leaf error-entry groups
            allErrors.RemoveAll(g => g.Name == "ChannelActiveErrors" || g.Groups.Count == 0);
            return allErrors;
        }

        /// <summary>
        /// Returns the machine status subtree rooted at ns=1;i=100000.
        /// Used to build the status node map for telemetry polling.
        /// </summary>
        public async Task<List<Group>> GetStatusAsync(CancellationToken ct = default)
        {
            return await Task.Run(() =>
                Groups("Status", 100000, 1, recursive: true), ct);
        }

        #endregion
    }
}