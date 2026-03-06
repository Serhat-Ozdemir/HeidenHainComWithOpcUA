using Opc.Ua;
using OPCUaClient;
using OPCUaClient.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeidenHainCom
{
    public partial class Form1 : Form
    {
        // ─────────────────────────────────────────────────────────────────────
        // Inner types
        // ─────────────────────────────────────────────────────────────────────

        public enum OperatingMode
        {
            Manual = 0, MDI = 1, RFP = 2, SingleStep = 3,
            Automatic = 4, Other = 5, Handwheel = 6
        }

        private struct StatusSnapshot
        {
            public string ActiveProgram;
            public string CurrentState;
            public string CurrentToolName;
            public int CounterCurrent;
            public int CounterTarget;
            public double SpindleNominalSpeed;
            public string OperationMode;
            public double FeedOverride;
            public double RapidOverride;
            public double SpeedOverride;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Fields
        // ─────────────────────────────────────────────────────────────────────

        private UaClient _client;
        private Group _selectedNode;
        private readonly Dictionary<string, Group> _statusNodes = new();
        private CancellationTokenSource _monitorCts;

        // ─────────────────────────────────────────────────────────────────────
        // Constructor
        // ─────────────────────────────────────────────────────────────────────

        public Form1()
        {
            InitializeComponent();

            treeDevices.AfterSelect += TreeDevices_AfterSelect;
            treeDevices.AfterExpand += TreeDevices_AfterExpand;
            lstErrors.DoubleClick += LstErrors_DoubleClick;
            btnRefresh.Click += BtnRefresh_Click;
            btnCycleStart.Click += BtnCycleStart_Click;
            btnCycleStop.Click += BtnCycleStop_Click;
            btnSave.Click += BtnSave_Click;
            btnReconnect.Click += BtnReconnect_Click;
            btnDisconnectMonitor.Click += BtnDisconnectMonitor_Click;
            btnConnect.Click += BtnConnect_Click;
            btnDisconnect.Click += BtnDisconnectBar_Click;
            this.FormClosing += Form1_FormClosing;

            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.S) BtnSave_Click(s, EventArgs.Empty);
            };
        }

        // ─────────────────────────────────────────────────────────────────────
        // Connect / Disconnect (top bar)
        // ─────────────────────────────────────────────────────────────────────

        private void BtnConnect_Click(object sender, EventArgs e) => Connect();

        private void Connect()
        {
            if (string.IsNullOrWhiteSpace(txtServerCertPath.Text))
            {
                MessageBox.Show("Please select a Server Certificate (.der) file before connecting.",
                    "Missing Certificate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCertPath.Text))
            {
                MessageBox.Show("Please select a User Certificate (.der) file before connecting.",
                    "Missing Certificate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtKeyPath.Text))
            {
                MessageBox.Show("Please select a User Key (.key) file before connecting.",
                    "Missing Key", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                InstallServerCertificate();
                _client = new UaClient(
                    "HeidenhainClient",
                    txtServer.Text,
                    true, true,
                    txtCertPath.Text,
                    txtKeyPath.Text);

                _client.Connect(5, true);
                SetConnected(true);
                SetStatus("Connected to " + txtServer.Text);
                InitializeAsync();
            }
            catch (Exception ex)
            {
                SetConnected(false);
                SetStatus("Connect error: " + ex.Message);
                MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDisconnectBar_Click(object sender, EventArgs e) => Disconnect();

        private void Disconnect()
        {
            StopMonitor();
            try { _client?.Disconnect(); } catch { }
            SetConnected(false);
            treeDevices.Nodes.Clear();
            rtbCode.Clear();
            lstErrors.Items.Clear();
            _statusNodes.Clear();
            SetStatus("Disconnected.");
        }

        private void SetConnected(bool connected)
        {
            lblConnectionStatus.Text = connected ? "● Connected" : "● Disconnected";
            lblConnectionStatus.ForeColor = connected
                ? Color.FromArgb(76, 175, 80) : Color.Gray;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Initialize after connect
        // ─────────────────────────────────────────────────────────────────────

        private async void InitializeAsync()
        {
            await LoadDeviceTreeAsync();
            await LoadStatusNodesAsync();
            await StartMonitorAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Device tree
        // ─────────────────────────────────────────────────────────────────────

        private async Task LoadDeviceTreeAsync()
        {
            if (_client == null) return;
            treeDevices.Nodes.Clear();
            SetStatus("Loading device tree...");

            try
            {
                var devices = await Task.Run(() => _client.Devices(false));
                if (devices == null) return;

                treeDevices.BeginUpdate();
                foreach (var dev in devices)
                {
                    var devNode = new TreeNode($"{dev.Name}  [{dev.Address}]") { Tag = dev };
                    foreach (var g in dev.Groups)
                    {
                        var gn = new TreeNode(NodeLabel(g)) { Tag = g };
                        if (g.NodeClass != NodeClass.Variable && g.Groups.Count == 0)
                            gn.Nodes.Add(new TreeNode("Loading..."));
                        else
                            AddGroupNodes(gn, g.Groups, lazyLoad: true);
                        devNode.Nodes.Add(gn);
                    }
                    treeDevices.Nodes.Add(devNode);
                }
                treeDevices.EndUpdate();
                SetStatus("Device tree loaded.");
            }
            catch (Exception ex)
            {
                SetStatus("Tree load error: " + ex.Message);
            }
        }

        private void AddGroupNodes(TreeNode parent, List<Group> groups, bool lazyLoad = false)
        {
            foreach (var g in groups)
            {
                var tn = new TreeNode(NodeLabel(g)) { Tag = g };
                if (lazyLoad && g.NodeClass != NodeClass.Variable && g.Groups.Count == 0)
                    tn.Nodes.Add(new TreeNode("Loading..."));
                else
                    AddGroupNodes(tn, g.Groups, lazyLoad);
                parent.Nodes.Add(tn);
            }
        }

        private static string NodeLabel(Group g)
        {
            string name = g.Name ?? g.Address;
            return g.NodeClass == NodeClass.Unspecified ? name : $"{name}  [{g.NodeClass}]";
        }

        private async void TreeDevices_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var tn = e.Node;
            if (tn.Tag is not Group group) return;
            if (group.NodeClass == NodeClass.Variable) return;
            if (tn.Nodes.Count > 0 && tn.Nodes[0].Text != "Loading...") return;

            tn.Nodes.Clear();
            SetStatus($"Loading children of '{group.Name}'...");

            try
            {
                var children = await Task.Run(() =>
                    _client.Groups(group.Address, group.Identifier,
                                   group.NameSpaceIndex, recursive: false));

                treeDevices.BeginUpdate();
                foreach (var child in children)
                {
                    var cn = new TreeNode(NodeLabel(child)) { Tag = child };
                    if (child.NodeClass != NodeClass.Variable && child.Groups.Count == 0)
                        cn.Nodes.Add(new TreeNode("Loading..."));
                    else
                        AddGroupNodes(cn, child.Groups, lazyLoad: true);
                    tn.Nodes.Add(cn);
                }
                treeDevices.EndUpdate();
                SetStatus("Ready.");
            }
            catch (Exception ex)
            {
                SetStatus("Expand error: " + ex.Message);
                tn.Nodes.Add(new TreeNode("Loading..."));
            }
        }

        private void TreeDevices_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is not Group group) return;
            _selectedNode = group;

            try
            {
                if (group.NodeClass == NodeClass.Variable)
                {
                    var result = _client.Read(group);
                    rtbCode.Text =
                        $"Source Timestamp:   {result.SourceTimestamp}\r\n" +
                        $"Source Picoseconds: {result.SourcePicoseconds}\r\n" +
                        $"Server Timestamp:   {result.ServerTimestamp}\r\n" +
                        $"Server Picoseconds: {result.ServerPicoseconds}\r\n" +
                        $"Status Code:        {result.StatusCode}\r\n" +
                        $"Type:               {group.NodeClass}\r\n" +
                        $"Value:              {result.Value}";
                }
                else if (IsNodeFile(group))
                {
                    ReadProgramAsync(group);
                }
            }
            catch (Exception ex)
            {
                rtbCode.Text = "Read error: " + ex.Message;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Read program via OPC UA FileType Open / Read / Close
        // ─────────────────────────────────────────────────────────────────────

        private async void ReadProgramAsync(Group node)
        {
            rtbCode.Text = "// Reading program...";
            try
            {
                string content = await Task.Run(() =>
                {
                    var fileNodeId = new NodeId((uint)node.Identifier, (ushort)node.NameSpaceIndex);

                    int fileSize = -1;
                    var sizeVar = node.Groups.FirstOrDefault(g => g.Name == "Size");
                    if (sizeVar != null)
                    {
                        var sizeResult = _client.Read(sizeVar);
                        if (sizeResult?.Value != null)
                            fileSize = Convert.ToInt32(sizeResult.Value);
                    }

                    uint handle = _client.OpenFile(fileNodeId, "1");
                    if (handle == 0)
                        return "// Could not open file. It may be in use by the controller.";

                    string text = _client.ReadFile(fileNodeId, handle, fileSize);
                    _client.CloseFile(fileNodeId, handle);
                    return string.IsNullOrEmpty(text) ? "// File is empty." : text;
                });

                rtbCode.Text = content;
            }
            catch (Exception ex)
            {
                rtbCode.Text = "// Read error: " + ex.Message;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Status nodes — load once after connect
        // ─────────────────────────────────────────────────────────────────────

        private async Task LoadStatusNodesAsync()
        {
            if (_client == null) return;
            try
            {
                var statusGroups = await _client.GetStatusAsync();
                SetStatusNodes(statusGroups);
            }
            catch { /* non-fatal */ }
        }

        public void SetStatusNodes(List<Group> statusGroups)
        {
            void Walk(IEnumerable<Group> groups, string path = "")
            {
                foreach (var g in groups ?? Enumerable.Empty<Group>())
                {
                    string cur = string.IsNullOrEmpty(path) ? g.Name : $"{path}/{g.Name}";
                    _statusNodes[cur] = g;
                    if (g.Groups?.Count > 0) Walk(g.Groups, cur);
                }
            }
            _statusNodes.Clear();
            Walk(statusGroups);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Monitor loop
        // ─────────────────────────────────────────────────────────────────────

        private async Task StartMonitorAsync()
        {
            _monitorCts = new CancellationTokenSource();
            var ct = _monitorCts.Token;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var (errors, status) = await Task.Run(() =>
                    {
                        var errs = CollectErrors();
                        var snap = CollectStatus();
                        return (errs, snap);
                    }, ct);

                    if (!IsHandleCreated) break;
                    this.Invoke(() => ApplySnapshot(errors, status));
                }
                catch (OperationCanceledException) { break; }
                catch { /* keep polling */ }

                try { await Task.Delay(200, ct); }
                catch (OperationCanceledException) { break; }
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Data collection — background-thread safe
        // ─────────────────────────────────────────────────────────────────────

        private List<ErrorEntry> CollectErrors()
        {
            var result = new List<ErrorEntry>();
            if (_client == null || !_client.IsConnected) return result;

            try
            {
                var errorGroups = _client.GetErrorsAsync().GetAwaiter().GetResult();

                foreach (var group in errorGroups.Where(g => g?.Groups != null))
                {
                    var data = group.Groups.ToDictionary(
                        p => p.Name,
                        p => { try { return _client.Read(p)?.Value?.ToString(); } catch { return ""; } });

                    string FmtEnum<T>(string key) where T : Enum
                    {
                        if (data.TryGetValue(key, out var v) && int.TryParse(v, out int i))
                            return $"{i} ({(T)(object)i})";
                        return "None";
                    }

                    result.Add(new ErrorEntry
                    {
                        ErrorLocation = FmtEnum<ErrorLocation>("Location"),
                        ErrorGroup = FmtEnum<ErrorGroup>("Group"),
                        ErrorClass = FmtEnum<ErrorClass>("Class"),
                        Text = data.GetValueOrDefault("Text") ?? "",
                        Internals = data.GetValueOrDefault("Internals") ?? "",
                        Cause = data.GetValueOrDefault("Cause") ?? "",
                        Action = data.GetValueOrDefault("Action") ?? ""
                    });
                }
            }
            catch { /* swallow */ }

            return result;
        }

        private StatusSnapshot CollectStatus()
        {
            T SafeRead<T>(string path, T def = default)
            {
                if (_client == null || !_client.IsConnected) return def;
                if (!_statusNodes.TryGetValue(path, out var node) || node == null) return def;
                try
                {
                    var raw = _client.Read(node);
                    if (raw?.Value == null) return def;
                    if (typeof(T) == typeof(string)) return (T)(object)raw.Value.ToString()!;
                    return (T)Convert.ChangeType(raw.Value.ToString(), typeof(T),
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch { return def; }
            }

            string SafeReadEnum<T>(string path) where T : Enum
            {
                int val = SafeRead<int>(path, -1);
                return (val < 0 || val > 6) ? "Unknown" : $"{val} ({(T)(object)val})";
            }

            return new StatusSnapshot
            {
                ActiveProgram = SafeRead<string>("Program/CurrentCall", "N/A"),
                CurrentState = SafeRead<string>("Program/ExecutionState/CurrentState", "Unknown"),
                CurrentToolName = SafeRead<string>("CurrentTool/Name", "No Tool"),
                CounterCurrent = SafeRead<int>("Counter/CurrentValue", -1),
                CounterTarget = SafeRead<int>("Counter/TargetValue", -1),
                SpindleNominalSpeed = SafeRead<double>("Spindle/NominalSpeed", -1),
                OperationMode = SafeReadEnum<OperatingMode>("OperatingMode"),
                FeedOverride = SafeRead<double>("FeedOverride", -1),
                RapidOverride = SafeRead<double>("RapidOverride", -1),
                SpeedOverride = SafeRead<double>("SpeedOverride", -1),
            };
        }

        // ─────────────────────────────────────────────────────────────────────
        // Apply snapshot → UI thread only
        // ─────────────────────────────────────────────────────────────────────

        private void ApplySnapshot(List<ErrorEntry> errors, StatusSnapshot s)
        {
            lstErrors.Items.Clear();
            foreach (var err in errors) lstErrors.Items.Add(err);

            lblActiveProgramVal.Text = s.ActiveProgram;
            lblCurrentStateVal.Text = s.CurrentState;
            lblCurrentToolVal.Text = s.CurrentToolName;
            lblCounterVal.Text = $"{s.CounterCurrent} / {s.CounterTarget}";
            lblSpindleVal.Text = $"{s.SpindleNominalSpeed:F0} RPM";
            lblOperationModeVal.Text = s.OperationMode;

            pbFeed.Value = Clamp((int)s.FeedOverride, 0, 100);
            pbRapid.Value = Clamp((int)s.RapidOverride, 0, 100);
            pbSpeed.Value = Clamp((int)s.SpeedOverride, 0, 100);

            lblFeedPct.Text = $"{(int)s.FeedOverride}%";
            lblRapidPct.Text = $"{(int)s.RapidOverride}%";
            lblSpeedPct.Text = $"{(int)s.SpeedOverride}%";
        }

        private static int Clamp(int v, int min, int max) => v < min ? min : v > max ? max : v;

        private void InstallServerCertificate()
        {
            if (string.IsNullOrWhiteSpace(txtServerCertPath.Text))
                return;

            try
            {
                string trustedFolder =
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "pki", "trusted", "certs");

                System.IO.Directory.CreateDirectory(trustedFolder);

                string dest =
                    System.IO.Path.Combine(trustedFolder,
                    System.IO.Path.GetFileName(txtServerCertPath.Text));

                System.IO.File.Copy(txtServerCertPath.Text, dest, true);

                SetStatus("Server certificate installed to Trusted store.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to install server certificate:\n\n" + ex.Message,
                    "Certificate Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        // ─────────────────────────────────────────────────────────────────────
        // Button handlers
        // ─────────────────────────────────────────────────────────────────────

        private async void BtnRefresh_Click(object sender, EventArgs e)
            => await LoadDeviceTreeAsync();

        private void BtnCycleStart_Click(object sender, EventArgs e)
        {
            if (_selectedNode == null || !IsNodeFile(_selectedNode))
            {
                MessageBox.Show("Select a .h program file in the tree first.",
                    "No Program Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var objectId = new NodeId(100007, 1);
                var fileNodeId = new NodeId((uint)_selectedNode.Identifier,
                                             (ushort)_selectedNode.NameSpaceIndex);
                _client.CallMethod(objectId, new NodeId(100014, 1), fileNodeId);
                _client.CallMethod(objectId, new NodeId(100019, 1));
                SetStatus($"Cycle Start: '{_selectedNode.Name}'");
            }
            catch (Exception ex)
            {
                SetStatus("Cycle Start error: " + ex.Message);
                MessageBox.Show(ex.Message, "Cycle Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCycleStop_Click(object sender, EventArgs e)
        {
            try
            {
                var objectId = new NodeId(100007, 1);
                _client.CallMethod(objectId, new NodeId(100020, 1));
                _client.CallMethod(objectId, new NodeId(100021, 1));
                _client.CallMethod(objectId, new NodeId(100018, 1));
                SetStatus("Cycle Stop sent.");
            }
            catch (Exception ex)
            {
                SetStatus("Cycle Stop error: " + ex.Message);
                MessageBox.Show(ex.Message, "Cycle Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_selectedNode == null || !IsNodeFile(_selectedNode))
            {
                MessageBox.Show("Select a .h program file first.", "No File",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(rtbCode.Text)) return;

            try
            {
                var fileNodeId = new NodeId((uint)_selectedNode.Identifier,
                                             (ushort)_selectedNode.NameSpaceIndex);
                uint handle = _client.OpenFile(fileNodeId, "6");
                if (handle == 0)
                {
                    MessageBox.Show("Could not open file for writing. It may be in use.",
                        "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _client.WriteFile(fileNodeId, handle, rtbCode.Text);
                _client.CloseFile(fileNodeId, handle);

                SetStatus($"'{_selectedNode.Name}' saved.");
                MessageBox.Show($"'{_selectedNode.Name}' was successfully saved to the controller.",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SetStatus("Save error: " + ex.Message);
                MessageBox.Show(ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReconnect_Click(object sender, EventArgs e)
        {
            if (_client == null) { SetStatus("Not connected."); return; }
            Disconnect();
            Connect();
        }

        private void BtnDisconnectMonitor_Click(object sender, EventArgs e)
        {
            StopMonitor();
            try { _client?.Disconnect(); } catch { }
            treeDevices.Nodes.Clear();
            rtbCode.Clear();
            lstErrors.Items.Clear();
            _statusNodes.Clear();
            SetConnected(false);
            SetStatus("Disconnected.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // Error detail popup (double-click)
        // ─────────────────────────────────────────────────────────────────────

        private void LstErrors_DoubleClick(object sender, EventArgs e)
        {
            if (lstErrors.SelectedItem is not ErrorEntry err) return;

            string details =
                $"[ {err.ErrorClass} ]\r\n\r\n" +
                $"Location:    {err.ErrorLocation}\r\n" +
                $"Group:       {err.ErrorGroup}\r\n\r\n" +
                $"Description: {err.Text}\r\n\r\n" +
                $"Cause:       {err.Cause}\r\n" +
                $"Action:      {err.Action}\r\n\r\n" +
                $"Internals:\r\n{err.Internals}";

            MessageBox.Show(details, "Error Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────────────

        private static bool IsNodeFile(Group node)
        {
            if (node?.Name == null || node.Name.Length < 2) return false;
            string ext = node.Name.Substring(node.Name.Length - 2);
            return ext.Equals(".h", StringComparison.OrdinalIgnoreCase);
        }

        private void SetStatus(string message)
        {
            if (InvokeRequired) { Invoke(() => SetStatus(message)); return; }
            lblStatusBar.Text = " " + message;
        }

        private void StopMonitor()
        {
            _monitorCts?.Cancel();
            _monitorCts = null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMonitor();
            try { _client?.Disconnect(); } catch { }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Error record + enums
    // ─────────────────────────────────────────────────────────────────────────

    public class ErrorEntry
    {
        public string ErrorLocation { get; set; } = "";
        public string ErrorGroup { get; set; } = "";
        public string ErrorClass { get; set; } = "";
        public string Text { get; set; } = "";
        public string Internals { get; set; } = "";
        public string Cause { get; set; } = "";
        public string Action { get; set; } = "";

        public override string ToString() => string.IsNullOrEmpty(Text) ? "(no text)" : Text;
    }

    public enum ErrorLocation { None = 0, Machine = 1, Edit = 2, Oem = 3 }
    public enum ErrorGroup { None = 0, Operating = 1, Programming = 2, Plc = 3, General = 4, Remote = 5, Python = 6 }
    public enum ErrorClass { None = 0, Warning = 1, FeedHold = 2, ProgramHold = 3, ProgramAbort = 4, EmergencyStop = 5, Reset = 6, Info = 7, Error = 8, Note = 9 }
}