using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua.Gds;
using OPCUaClient.Exceptions;
using OPCUaClient.Objects;
using System.Security.Cryptography.X509Certificates;

namespace OPCUaClient
{
    /// <summary>
    /// Client for OPCUA Server
    /// </summary>
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

        #region Private methods

        private void Reconnect(object sender, EventArgs e)
        {
            if (!ReferenceEquals(sender, this._reconnectHandler))
            {
                return;
            }

            lock (this._lock)
            {
                if (this._reconnectHandler.Session != null)
                {
                    this._session = (Session)_reconnectHandler.Session;
                }

                this._reconnectHandler.Dispose();
                this._reconnectHandler = null;
            }
        }

        private Subscription Subscription(int miliseconds)
        {
            var subscription = new Subscription()
            {
                PublishingEnabled = true,
                PublishingInterval = miliseconds,
                Priority = 1,
                KeepAliveCount = 10,
                LifetimeCount = 20,
                MaxNotificationsPerPublish = 1000
            };

            return subscription;
        }

        #endregion

        #region Public fields

        /// <summary>
        /// Indicates if the instance is connected to the server.
        /// </summary>
        public bool IsConnected => this._session is { Connected: true };

        #endregion

        #region Public methods

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="appName">
        /// Name of the application
        /// </param>
        /// <param name="serverUrl">
        /// Url of server
        /// </param>
        /// <param name="security">
        /// Enable or disable the security
        /// </param>
        /// <param name="untrusted">
        /// Accept untrusted certificates
        /// </param>
        /// <param name="user">
        /// User of the OPC UA Server
        /// </param>
        /// <param name="password">
        /// Password of the user
        /// </param>
        public UaClient(String appName, String serverUrl, bool security, bool untrusted, String user = "",
            String password = "")
        {
            String path = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(Path.Combine(path, "Application"));
            Directory.CreateDirectory(Path.Combine(path, "Trusted"));
            Directory.CreateDirectory(Path.Combine(path, "TrustedPeer"));
            Directory.CreateDirectory(Path.Combine(path, "Rejected"));
            String hostName = System.Net.Dns.GetHostName();

            var userCert = new X509Certificate2(
    @"C:\Users\serhat.ozdemir\Desktop\certfcates\opcua-user-cert.pfx",
    "1234", // <-- password you entered during export
    X509KeyStorageFlags.Exportable |
    X509KeyStorageFlags.PersistKeySet |
    X509KeyStorageFlags.MachineKeySet
);

            _userIdentity = new UserIdentity(userCert);
            _appConfig = new ApplicationConfiguration
            {
                ApplicationName = appName,
                ApplicationUri = Utils.Format(@"urn:{0}" + appName, hostName),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StorePath = Path.Combine(path, "Application"),
                        SubjectName = $"CN={appName}, DC={hostName}"
                    },
                    TrustedIssuerCertificates = new CertificateTrustList
                    {
                        StoreType = @"Directory",
                        StorePath = Path.Combine(path, "Trusted")
                    },
                    TrustedPeerCertificates = new CertificateTrustList
                    {
                        StoreType = @"Directory",
                        StorePath = Path.Combine(path, "TrustedPeer")
                    },
                    RejectedCertificateStore = new CertificateTrustList
                    {
                        StoreType = @"Directory",
                        StorePath = Path.Combine(path, "Rejected")
                    },
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true,
                    RejectSHA1SignedCertificates = false
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 20000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 5000 },
                TraceConfiguration = new TraceConfiguration
                {
                    DeleteOnLoad = true,
                },
                DisableHiResClock = false
            };
            _appConfig.Validate(ApplicationType.Client).GetAwaiter().GetResult();

            if (_appConfig.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                _appConfig.CertificateValidator.CertificateValidation += (s, ee) => { ee.Accept = (ee.Error.StatusCode == StatusCodes.BadCertificateUntrusted && untrusted); };
            }

            var application = new ApplicationInstance
            {
                ApplicationName = appName,
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = _appConfig
            };
            Utils.SetTraceMask(0);
            application.CheckApplicationInstanceCertificate(true, 2048).GetAwaiter().GetResult();

            var endpointDescription = CoreClientUtils.SelectEndpoint(serverUrl, true);
            var endpointConfig = EndpointConfiguration.Create(_appConfig);
            _endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfig);
        }

        /// <summary>
        /// Open the connection with the OPC UA Server
        /// </summary>
        /// <param name="timeOut">
        /// Timeout to try to connect with the server in seconds.
        /// </param>
        /// <param name="keepAlive">
        /// Sets whether to try to connect to the server in case the connection is lost.
        /// </param>
        /// <exception cref="ServerException"></exception>
        public void Connect(uint timeOut = 5, bool keepAlive = false)
        {
            this.Disconnect();

            this._session =
                Task.Run(
                    async () => await Session.Create(_appConfig, _endpoint, false, false, _appConfig.ApplicationName,
                        timeOut * 1000, _userIdentity, null)).GetAwaiter().GetResult();

            if (keepAlive)
            {
                this._session.KeepAlive += this.KeepAlive;
            }

            if (this._session == null || !this._session.Connected)
            {
                throw new ServerException("Error creating a session on the server");
            }
        }

        /// <summary>
        /// Open the connection with the OPC UA Server
        /// </summary>
        /// <param name="timeOut">
        /// Timeout to try to connect with the server in seconds.
        /// </param>
        /// <param name="keepAlive">
        /// Sets whether to try to connect to the server in case the connection is lost.
        /// </param>
        /// <exception cref="ServerException"></exception>
        public async Task ConnectAsync(uint timeOut = 5, bool keepAlive = false, CancellationToken ct = default)
        {
            await this.DisconnectAsync(ct);

            this._session = await Session.Create(_appConfig, _endpoint, false, false, _appConfig.ApplicationName,
                timeOut * 1000, _userIdentity, null, ct);

            if (keepAlive)
            {
                this._session.KeepAlive += this.KeepAlive;
            }

            if (this._session == null || !this._session.Connected)
            {
                throw new ServerException("Error creating a session on the server");
            }
        }

        private void KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            try
            {
                if (!ServiceResult.IsBad(e.Status)) return;
                lock (this._lock)
                {
                    if (this._reconnectHandler != null) return;
                    this._reconnectHandler = new SessionReconnectHandler(true);
                    this._reconnectHandler.BeginReconnect(this._session, ReconnectPeriod, this.Reconnect);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        /// <summary>
        /// Close the connection with the OPC UA Server
        /// </summary>
        public void Disconnect()
        {
            if (this._session is { Connected: true })
            {
                if (this._session.Subscriptions != null && this._session.Subscriptions.Any())
                {
                    foreach (var subscription in this._session.Subscriptions)
                    {
                        subscription.Delete(true);
                    }
                }

                this._session.Close();
                this._session.Dispose();
                this._session = null;
            }
        }

        /// <summary>
        /// Close the connection with the OPC UA Server
        /// </summary>
        public async Task DisconnectAsync(CancellationToken ct = default)
        {
            if (this._session is { Connected: true })
            {
                if (this._session.Subscriptions != null && this._session.Subscriptions.Any())
                {
                    foreach (var subscription in this._session.Subscriptions)
                    {
                        await subscription.DeleteAsync(true, ct);
                    }
                }

                await this._session.CloseAsync(ct);
                this._session.Dispose();
                this._session = null;
            }
        }


        /// <summary>
        /// Write a value on a tag
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <param name="value">
        /// Value to write
        /// </param>
        /// <exception cref="WriteException"></exception>
        public void Write(Group grp, Object value)
        {
            WriteValueCollection writeValues = new WriteValueCollection();
            var writeValue = new WriteValue
            {
                NodeId = new NodeId((uint)grp.Identifier, (ushort)grp.NameSpaceIndex),
                AttributeId = 7,
                Value = new DataValue
                {
                    Value = value
                }
            };
            writeValues.Add(writeValue);
            this._session.Write(null, writeValues, out StatusCodeCollection statusCodes,
                out DiagnosticInfoCollection diagnosticInfo);
            if (!StatusCode.IsGood(statusCodes[0]))
            {
                throw new WriteException("Error writing value. Code: " + statusCodes[0].Code.ToString());
            }
        }


        /// <summary>
        /// Write a value on a tag
        /// </summary>
        /// <param name="tag"> <see cref="Tag"/></param>
        /// <exception cref="WriteException"></exception>
        public void Write(Tag tag)
        {
            //this.Write(tag.Address, tag.Value);
        }


        /// <summary>
        /// Read a tag of the sepecific address
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <returns>
        /// <see cref="Tag"/>
        /// </returns>
        public DataValue Read(Group grp)
        {

            ReadValueIdCollection readValues = new ReadValueIdCollection()
            {
                new ReadValueId
                {
                    NodeId = new NodeId((uint)grp.Identifier, (ushort)grp.NameSpaceIndex),
                    AttributeId = Attributes.Value
                }
            };
            this._session.Read(null, 0, TimestampsToReturn.Both, readValues, out DataValueCollection dataValues,
                out DiagnosticInfoCollection diagnosticInfo);
            return dataValues[0];
        }


        /// <summary>
        /// Read an address
        /// </summary>
        /// <param name="address">
        /// Address to read.
        /// </param>
        /// <typeparam name="TValue">
        /// Type of value to read.
        /// </typeparam>
        /// <returns></returns>
        /// <exception cref="ReadException">
        /// If the status of read action is not good <see cref="StatusCodes"/>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// If the type is not supported.
        /// </exception>
        public TValue Read<TValue>(String address)
        {
            ReadValueIdCollection readValues = new ReadValueIdCollection()
            {
                new ReadValueId
                {
                    NodeId = new NodeId(address, 2),
                    AttributeId = Attributes.Value
                }
            };


            this._session.Read(null, 0, TimestampsToReturn.Both, readValues, out DataValueCollection dataValues,
                out DiagnosticInfoCollection diagnosticInfo);


            if (dataValues[0].StatusCode != StatusCodes.Good)
            {
                throw new ReadException(dataValues[0].StatusCode.Code.ToString());
            }

            if (typeof(TValue) == typeof(Boolean))
            {
                return (TValue)(object)Convert.ToBoolean(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(byte))
            {
                return (TValue)(object)Convert.ToByte(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(UInt16))
            {
                return (TValue)(object)Convert.ToUInt16(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(UInt32))
            {
                return (TValue)(object)Convert.ToUInt32(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(UInt64))
            {
                return (TValue)(object)Convert.ToUInt64(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(Int16))
            {
                return (TValue)(object)Convert.ToInt16(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(Int32))
            {
                return (TValue)(object)Convert.ToInt32(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(Int64))
            {
                return (TValue)(object)Convert.ToInt64(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(Single))
            {
                return (TValue)(object)Convert.ToSingle(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(Double))
            {
                return (TValue)(object)Convert.ToDouble(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(Decimal))
            {
                return (TValue)(object)Convert.ToDecimal(dataValues[0].Value);
            }
            else if (typeof(TValue) == typeof(String))
            {
                return (TValue)(object)Convert.ToString(dataValues[0].Value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        /// <summary>
        /// Write a lis of values
        /// </summary>
        /// <param name="tags"> <see cref="Tag"/></param>
        /// <exception cref="WriteException"></exception>
        public void Write(List<Tag> tags)
        {
            WriteValueCollection writeValues = new WriteValueCollection();


            writeValues.AddRange(tags.Select(tag => new WriteValue
            {
                NodeId = new NodeId(tag.Address, 2),
                AttributeId = Attributes.Value,
                Value = new DataValue()
                {
                    Value = tag.Value
                }
            }));
            this._session.Write(null, writeValues, out StatusCodeCollection statusCodes,
                out DiagnosticInfoCollection diagnosticInfo);

            if (statusCodes.All(StatusCode.IsGood)) return;
            {
                var status = statusCodes.First(sc => !StatusCode.IsGood(sc));
                throw new WriteException("Error writing value. Code: " + status.Code.ToString());
            }
        }


        /// <summary>
        /// Read a list of tags on the OPCUA Server
        /// </summary>
        /// <param name="address">
        /// List of address to read.
        /// </param>
        /// <returns>
        /// A list of tags <see cref="Tag"/>
        /// </returns>
        public List<Tag> Read(List<String> address)
        {
            var tags = new List<Tag>();

            ReadValueIdCollection readValues = new ReadValueIdCollection();
            readValues.AddRange(address.Select(a => new ReadValueId
            {
                NodeId = new NodeId(a, 2),
                AttributeId = Attributes.Value
            }));

            this._session.Read(null, 0, TimestampsToReturn.Both, readValues, out DataValueCollection dataValues,
                out DiagnosticInfoCollection diagnosticInfo);

            for (int i = 0; i < address.Count; i++)
            {
                tags.Add(new Tag
                {
                    Address = address[i],
                    Value = dataValues[i].Value,
                    Code = dataValues[i].StatusCode
                });
            }

            return tags;
        }

        public IList<object> CallMethod(NodeId objectId, NodeId methodId, params object[] inputArguments)
        {
            if(this._session == null || !this._session.Connected)
                throw new InvalidOperationException("Not connected to the server.");

            var inputArgs = new VariantCollection(
                inputArguments.Select(arg => new Variant(arg))
            );

            _session.Call(
                null,
                new CallMethodRequestCollection
                {
            new CallMethodRequest
            {
                ObjectId = objectId,
                MethodId = methodId,
                InputArguments = inputArgs
            }
                },
                out var results,
                out var diagnosticInfos
            );

            if (results == null || results.Count == 0)
                throw new Exception("No result returned from method call.");

            if (StatusCode.IsBad(results[0].StatusCode))
                throw new Exception($"Method call failed: {results[0].StatusCode}");

            return results[0].OutputArguments
                .Select(v => v.Value)
                .ToList();
        }

        /// <summary>
        /// Monitoring a tag and execute a function when the value change
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <param name="miliseconds">
        /// Sets the time to check changes in the tag
        /// </param>
        /// <param name="monitor">
        /// Function to execute when the value changes.
        /// </param>
        public void Monitoring(String address, int miliseconds, MonitoredItemNotificationEventHandler monitor)
        {
            var subscription = this.Subscription(miliseconds);
            MonitoredItem monitored = new MonitoredItem();
            monitored.StartNodeId = new NodeId(address, 2);
            monitored.AttributeId = Attributes.Value;
            monitored.Notification += monitor;
            subscription.AddItem(monitored);
            this._session.AddSubscription(subscription);
            subscription.Create();
            subscription.ApplyChanges();
        }


        /// <summary>
        /// Scan root folder of OPC UA server and get all devices
        /// </summary>
        /// <param name="recursive">
        /// Indicates whether to search within device groups
        /// </param>
        /// <returns>
        /// List of <see cref="Device"/>
        /// </returns>
        public List<Device> Devices(bool recursive = false)
        {
            Browser browser = new Browser(this._session)
            {
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable,
                ReferenceTypeId = ReferenceTypeIds.Organizes
            };

            ReferenceDescriptionCollection browseResults = browser.Browse(Opc.Ua.ObjectIds.ObjectsFolder);

            var devices = browseResults.Where(d => d.ToString() != "Server").Select(b => new Device
            {
                Address = b.ToString(),
                Identifier = b.NodeId.Identifier is uint id ? (int)id : 0,
                NameSpaceIndex = b.NodeId.NamespaceIndex
            }).ToList();

            devices.ForEach(d =>
            {
                d.Groups = this.Groups(d.Address, d.Identifier, d.NameSpaceIndex, recursive);
                //d.Tags = this.Tags(d.Address);
            });

            return devices;
        }


        /// <summary>
        /// Scan an address and retrieve the tags and groups
        /// </summary>
        /// <param name="address">
        /// Address to search
        /// </param>
        /// <param name="recursive">
        /// Indicates whether to search within group groups
        /// </param>
        /// <returns>
        /// List of <see cref="Group"/>
        /// </returns>
        //public List<Group> Groups(String address, bool recursive = false)
        //{
        //    var groups = new List<Group>();
        //    Browser browser = new Browser(this._session)
        //    {
        //        BrowseDirection = BrowseDirection.Forward,
        //        NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable,
        //        ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences
        //    };

        //    ReferenceDescriptionCollection browseResults = browser.Browse(new NodeId(50000, 1));

        //    foreach (var result in browseResults)
        //    {
        //        if (result.NodeClass != NodeClass.Object) continue;
        //        var group = new Group
        //        {
        //            Address = address + "." + result.ToString()
        //        };
        //        //group.Groups = this.Groups(group.Address, recursive);
        //        //group.Tags = this.Tags(group.Address);
        //        groups.Add(group);
        //    }

        //    return groups;
        //}
        public List<Group> Groups(string address, int identifier, int nameSpaceIndex, bool recursive = false, HashSet<string>? visited = null)
        {
            visited ??= new HashSet<string>();

            var groups = new List<Group>();

            Browser browser = new Browser(this._session)
            {
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable | (int) NodeClass.Method,
                ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                IncludeSubtypes = true
            };

            NodeId parentNodeId = new NodeId((uint)identifier, (ushort)nameSpaceIndex);

            ReferenceDescriptionCollection browseResults;

            try
            {
                browseResults = browser.Browse(parentNodeId);
            }
            catch (ServiceResultException ex) when (ex.StatusCode == StatusCodes.BadUserAccessDenied)
            {
                return groups;
            }

            foreach (var result in browseResults)
            {

                var childNodeId = result.NodeId;

                string uniqueKey = $"{childNodeId.NamespaceIndex}:{childNodeId.Identifier}";

                if (visited.Contains(uniqueKey))
                    continue;

                visited.Add(uniqueKey);

                int childIdentifier = childNodeId.Identifier is uint id ? (int)id : 0;

                var group = new Group
                {
                    Address = address + "." + result.BrowseName.Name,
                    Identifier = childIdentifier,
                    NameSpaceIndex = childNodeId.NamespaceIndex,
                    NodeClass = result.NodeClass
                };
                if (group.Address == "HEIDENHAIN NC.Machine.FileSystem.TNC" || group.Address == "HEIDENHAIN NC.Machine.ToolDataManagement")
                {
                    groups.Add(group);
                    continue;
                }
                if (recursive)
                {
                    group.Groups = Groups(
                        group.Address,
                        group.Identifier,
                        group.NameSpaceIndex,
                        true,
                        visited);
                }

                groups.Add(group);
            }

            return groups;
        }





        ///// <summary>
        ///// Scan an address and retrieve the tags.
        ///// </summary>
        ///// <param name="address">
        ///// Address to search
        ///// </param>
        ///// <returns>
        ///// List of <see cref="Tag"/>
        ///// </returns>
        //public List<Tag> Tags(String address)
        //{
        //    var tags = new List<Tag>();
        //    Browser browser = new Browser(this._session)
        //    {
        //        BrowseDirection = BrowseDirection.Forward,
        //        NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable,
        //        ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences
        //    };

        //    ReferenceDescriptionCollection browseResults = browser.Browse(new NodeId(address, 2));
        //    foreach (var result in browseResults)
        //    {
        //        if (result.NodeClass == NodeClass.Variable)
        //        {
        //            tags.Add(new Tag
        //            {
        //                Address = address + "." + result.ToString()
        //            });
        //        }
        //    }

        //    return tags;
        //}


        #region Async methods

        /// <summary>
        /// Scan root folder of OPC UA server and get all devices
        /// </summary>
        /// <param name="recursive">
        /// Indicates whether to search within device groups
        /// </param>
        /// <param name="ct">
        /// Cancellation token
        /// </param>
        /// <returns>
        /// List of <see cref="Device"/>
        /// </returns>
        public Task<List<Device>> DevicesAsync(bool recursive = false, CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                Browser browser = new Browser(this._session)
                {
                    BrowseDirection = BrowseDirection.Forward,
                    NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable,
                    ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences
                };

                ReferenceDescriptionCollection browseResults = browser.Browse(Opc.Ua.ObjectIds.ObjectsFolder);

                var devices = browseResults.Where(d => d.ToString() != "Server").Select(b => new Device
                {
                    Address = b.ToString()
                }).ToList();

                devices.ForEach(d =>
                {
                    //d.Groups = this.Groups(d.Address, recursive);
                    //d.Tags = this.Tags(d.Address);
                });
                return devices;
            }, ct);
        }


        /// <summary>
        /// Scan an address and retrieve the tags and groups
        /// </summary>
        /// <param name="address">
        /// Address to search
        /// </param>
        /// <param name="recursive">
        /// Indicates whether to search within group groups
        /// </param>
        /// <param name="ct">
        /// Cancellation token
        /// </param>
        /// <returns>
        /// List of <see cref="Group"/>
        /// </returns>
        //public Task<List<Group>> GroupsAsync(String address, bool recursive = false, CancellationToken ct = default)
        //{
        //    return Task.Run(() => Groups(address, recursive), ct);
        //}


        /// <summary>
        /// Scan an address and retrieve the tags.
        /// </summary>
        /// <param name="address">
        /// Address to search
        /// </param>
        /// <param name="ct">
        ///  Cancellation token
        /// </param>
        /// <returns>
        /// List of <see cref="Tag"/>
        /// </returns>
        //public Task<List<Tag>> TagsAsync(String address, CancellationToken ct = default)
        //{
        //    return Task.Run(() => Tags(address), ct);
        //}


        /// <summary>
        /// Write a value on a tag
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <param name="value">
        /// Value to write
        /// </param>
        /// <param name="ct">
        /// Cancellation token
        /// </param>
        public async Task<Tag> WriteAsync(String address, Object value, CancellationToken ct = default)
        {
            WriteValueCollection writeValues = new WriteValueCollection();
            var writeValue = new WriteValue
            {
                NodeId = new NodeId(address, 2),
                AttributeId = Attributes.Value,
                Value = new DataValue
                {
                    Value = value
                }
            };
            writeValues.Add(writeValue);
            WriteResponse response = await this._session.WriteAsync(null, writeValues, ct);

            var tag = new Tag()
            {
                Address = address,
                Value = value,
                Code = response.Results[0].Code
            };

            return tag;
        }


        /// <summary>
        /// Write a value on a tag
        /// </summary>
        /// <param name="tag"> <see cref="Tag"/></param>
        /// <param name="ct"> Cancellation token</param>
        public Task<Tag> WriteAsync(Tag tag, CancellationToken ct = default)
        {
            var task = this.WriteAsync(tag.Address, tag.Value, ct);

            return task;
        }

        /// <summary>
        /// Write a lis of values
        /// </summary>
        /// <param name="tags"><see cref="Tag"/></param>
        /// <param name="ct">
        /// Cancellation token
        /// </param>
        public async Task<IEnumerable<Tag>> WriteAsync(List<Tag> tags, CancellationToken ct = default)
        {
            WriteValueCollection writeValues = new WriteValueCollection();


            writeValues.AddRange(tags.Select(tag => new WriteValue
            {
                NodeId = new NodeId(tag.Address, 2),
                AttributeId = Attributes.Value,
                Value = new DataValue()
                {
                    Value = tag.Value
                }
            }));

            WriteResponse response = await this._session.WriteAsync(null, writeValues, ct);

            for (int i = 0; i < response.Results.Count; i++)
            {
                tags[i].Code = response.Results[i].Code;
            }

            return tags;
        }


        /// <summary>
        /// Read a tag of the specific address
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <param name="ct">
        /// Cancellation token
        /// </param>
        /// <returns>
        /// <see cref="Tag"/>
        /// </returns>
        public async Task<Tag> ReadAsync(String address, CancellationToken ct = default)
        {
            var tag = new Tag
            {
                Address = address,
                Value = null,
            };
            ReadValueIdCollection readValues = new ReadValueIdCollection()
            {
                new ReadValueId
                {
                    NodeId = new NodeId(address, 2),
                    AttributeId = Attributes.Value
                }
            };

            var dataValues = await this._session.ReadAsync(null, 0, TimestampsToReturn.Both, readValues, ct);

            tag.Value = dataValues.Results[0].Value;
            tag.Code = dataValues.Results[0].StatusCode;

            return tag;
        }

        /// <summary>
        /// Read an address
        /// </summary>
        /// <param name="address">
        /// Address to read.
        /// </param>
        /// <param name="ct">
        ///  Cancellation token
        /// </param>
        /// <typeparam name="TValue">
        /// Type of value to read.
        /// </typeparam>
        /// <returns></returns>
        /// <exception cref="ReadException">
        /// If the status of read action is not good <see cref="StatusCodes"/>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// If the type is not supported.
        /// </exception>
        public Task<TValue> ReadAsync<TValue>(String address, CancellationToken ct = default)
        {
            return Task.Run(() => Read<TValue>(address), ct);
        }


        /// <summary>
        /// Read a list of tags on the OPCUA Server
        /// </summary>
        /// <param name="address">
        /// List of address to read.
        /// </param>
        /// <param name="ct">
        ///  Cancellation token
        /// </param>
        /// <returns>
        /// A list of tags <see cref="Tag"/>
        /// </returns>
        public async Task<IEnumerable<Tag>> ReadAsync(IEnumerable<String> address, CancellationToken ct = default)
        {
            var tags = new List<Tag>();

            ReadValueIdCollection readValues = new ReadValueIdCollection();
            readValues.AddRange(address.Select(a => new ReadValueId
            {
                NodeId = new NodeId(a, 2),
                AttributeId = Attributes.Value
            }));

            var dataValues =
                await this._session.ReadAsync(null, 0, TimestampsToReturn.Both, readValues, ct);

            for (int i = 0; i < dataValues.Results.Count; i++)
            {
                tags.Add(new Tag
                {
                    Address = address.ToArray()[i],
                    Value = dataValues.Results[i].Value,
                    Code = dataValues.Results[i].StatusCode
                });
            }

            return tags;
        }

        #endregion

        #endregion
    }
}