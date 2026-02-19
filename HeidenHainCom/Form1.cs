using OPCUaClient;
using OPCUaClient.Objects;

namespace HeidenHainCom
{
    public partial class Form1 : Form
    {

        // TNC7 Tag Addresses
        // Run "Browse" on your simulation first to confirm exact addresses.
        // Common TNC7 OPC UA tag paths are listed below – adjust if needed.

        private const string TAG_SELECT_PROGRAM = "/Plc/Program/SelectProgram";
        private const string TAG_PGM_SELECT = "/Plc/Program/PgmSelect";
        private const string TAG_START = "/Plc/Program/Start";
        private const string TAG_STOP = "/Plc/Program/Stop";
        private const string TAG_RESET = "/Plc/Program/Reset";

        private UaClient? _client;

        public Form1()
        {
            InitializeComponent();
            txtServer.Text = "opc.tcp://192.168.56.101:4840";
        }

        // Helpers

        private void Log(string message)
        {
            if (lstLog.InvokeRequired) { lstLog.Invoke(() => Log(message)); return; }
            lstLog.Text =  $"[{DateTime.Now:HH:mm:ss}] {message}";
            txtStatus.Text = message;
        }
        private void ShowValue(string message)
        {
            if (rtbValue.InvokeRequired) { lstLog.Invoke(() => ShowValue(message)); return; }
            rtbValue.Text = message;
        }

        private void SetConnected(bool connected)
        {
            btnSelectProgram.Enabled = connected;
            btnStart.Enabled = connected;
            btnStop.Enabled = connected;
            btnReset.Enabled = connected;

            lblStatus.Text = connected ? "Connected" : "Disconnected";
            lblStatus.ForeColor = connected ? Color.FromArgb(76, 175, 80) : Color.Gray;
        }

        // Existing: Connect

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _client = new UaClient(
    "MyWinFormsClient",
    txtServer.Text,
    true,   // security MUST be true
    true
);
                _client.Connect(5, true);
                SetConnected(true);
                Log("Connected to " + txtServer.Text);
            }
            catch (Exception ex)
            {
                SetConnected(false);
                Log("Connect error: " + ex.Message);
                MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Existing: Read 

        //private void btnRead_Click(object sender, EventArgs e)
        //{
        //    //if (_client == null) { MessageBox.Show("Not connected."); return; }
        //    //try
        //    //{
        //    //    //var tag = _client.Read(txtNode.Text);
        //    //    //txtValue.Text = tag.Value?.ToString() ?? "(null)";
        //    //    //Log($"Read '{txtNode.Text}' -> {tag.Value}");
        //    //    var devices = _client.Devices(true);
        //    //    Log($"Devices: {string.Join(", ", devices.Select(d => d.Name))}");
        //    //    //var tag = _client.Read("51000");
        //    //    //Log($"Read 'HEIDENHAIN NC' -> {tag.Name}");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Log("Read error: " + ex.Message);
        //    //    MessageBox.Show(ex.Message, "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    //}

        //}
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            try
            {
                treeOpc.Nodes.Clear();

                var devices = _client.Devices(true);

                foreach (var device in devices)
                {
                    TreeNode deviceNode = new TreeNode(device.Name);
                    deviceNode.Tag = device;

                    AddGroups(deviceNode, device.Groups);
                    AddTags(deviceNode, device.Tags);

                    treeOpc.Nodes.Add(deviceNode);
                    
                }
                treeOpc.AfterSelect += treeOpc_AfterSelect;
                treeOpc.CollapseAll();
                Log("Tree loaded.");
            }
            catch (Exception ex)
            {
                Log("Read error: " + ex.Message);
                MessageBox.Show(ex.Message, "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void treeOpc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_client == null) return;

            if (e.Node.Tag is Group group)
            {
                try
                {
                    var result = _client.Read(group);
                    string message = $"Source Timestamp: {result.SourceTimestamp}, " +
                        $"\n Source Time PicoSeconds: {result.SourcePicoseconds}" +
                        $"\n Server Timestamp: {result.ServerTimestamp}, " +
                        $"\n Source Time PicoSeconds: {result.ServerPicoseconds}," +
                        $"\n Status Code: {result.StatusCode}," +
                        $"\n Value: {result.Value}";
                    ShowValue(message);
                }
                catch (Exception ex)
                {
                    ShowValue("Read error: " + ex.Message);
                }
            }
        }
        private void AddGroups(TreeNode parentNode, List<Group> groups)
        {
            foreach (var group in groups)
            {
                TreeNode groupNode = new TreeNode(group.Name);
                groupNode.Tag = group;

                AddGroups(groupNode, group.Groups);
                AddTags(groupNode, group.Tags);

                parentNode.Nodes.Add(groupNode);
            }
        }

        private void AddTags(TreeNode parentNode, List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                TreeNode tagNode = new TreeNode(tag.Name);
                tagNode.Tag = tag;

                parentNode.Nodes.Add(tagNode);
            }
        }


        // New: Select Program

        private async void btnSelectProgram_Click(object sender, EventArgs e)
        {
            string path = txtProgramPath.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Enter an NC program path first.\nExample: TNC:\\nc_prog\\test.h");
                return;
            }

            try
            {
                // Step 1 – write the program path string
                Log($"Writing program path: {path}");
                await _client!.WriteAsync(TAG_SELECT_PROGRAM, path);

                // Step 2 – small delay, then trigger the selection
                await Task.Delay(200);
                await _client!.WriteAsync(TAG_PGM_SELECT, true);

                Log("SelectProgram sent successfully.");
            }
            catch (Exception ex)
            {
                Log("SelectProgram error: " + ex.Message);
                MessageBox.Show(ex.Message, "Select Program Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // New: Start

        private async void btnStart_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Log("Sending Start");
            //    await _client!.WriteAsync(TAG_START, true);
            //    Log("START sent.");
            //}
            //catch (Exception ex)
            //{
            //    Log("Start error: " + ex.Message);
            //    MessageBox.Show(ex.Message, "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            _client.CallMethod(1, 1, new object[6]);
        }

        // New: Stop

        private async void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                Log("Sending STOP");
                await _client!.WriteAsync(TAG_STOP, true);
                Log("STOP sent.");
            }
            catch (Exception ex)
            {
                Log("Stop error: " + ex.Message);
                MessageBox.Show(ex.Message, "Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // New Reset

        private async void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Send RESET to TNC7?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                Log("Sending RESET");
                await _client!.WriteAsync(TAG_RESET, true);
                Log("RESET sent.");
            }
            catch (Exception ex)
            {
                Log("Reset error: " + ex.Message);
                MessageBox.Show(ex.Message, "Reset Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //  New Clear Log

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
        }

        //  Cleanup on close

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { _client?.Disconnect(); } catch { /* ignored */ }
        }
    }
}