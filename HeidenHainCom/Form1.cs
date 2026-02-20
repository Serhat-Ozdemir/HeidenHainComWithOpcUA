using Opc.Ua;
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

        private Group selectedGroup;
        private UaClient? _client;

        private uint _currentFileHandle = 0;
        private uint _currentFileNodeId = 0;
        public Form1()
        {
            InitializeComponent();
            selectedGroup = new Group();
            txtServer.Text = "opc.tcp://192.168.56.101:4840";
        }

        // Helpers

        private void Log(string message)
        {
            if (lstLog.InvokeRequired) { lstLog.Invoke(() => Log(message)); return; }
            lstLog.Text = $"[{DateTime.Now:HH:mm:ss}] {message}";
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
            btnDeselect.Enabled = connected;
            btnCancel.Enabled = connected;
            btnCreateDirectory.Enabled = connected;
            btnCreateFile.Enabled = connected;
            btnDelete.Enabled = connected;
            btnMoveOrCopy.Enabled = connected;
            btnOpen.Enabled = connected;
            btnClose.Enabled = connected;
            btnReadFile.Enabled = connected;
            btnWriteFile.Enabled = connected;
            btnGetPosition.Enabled = connected;
            btnSetPosition.Enabled = connected;

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

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _client?.Disconnect();
                SetConnected(false);
                Log("Disconnected from " + txtServer.Text);
            }
            catch
            {
                Log("Error during disconnect.");
                MessageBox.Show("Error during disconnect.", "Disconnect Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
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
                    //AddTags(deviceNode, device.Tags);

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
        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            if (selectedGroup == null || string.IsNullOrEmpty(txtValue.Text))
            {
                MessageBox.Show("Select a node and enter a value to write.");
                return;
            }

            if (selectedGroup.NodeClass != Opc.Ua.NodeClass.Variable)
            {
                MessageBox.Show("Selected node is not a variable.");
                return;
            }

            try
            {
                object valueToWrite = Convert.ChangeType(txtValue.Text, typeof(object));
                _client.Write(selectedGroup, valueToWrite);
                Log($"Wrote value '{txtValue.Text}' to {selectedGroup.Name}");
            }
            catch (Exception ex)
            {
                Log("Write error: " + ex.Message);
                MessageBox.Show(ex.Message, "Write Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void treeOpc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_client == null) return;

            if (e.Node?.Tag is Group group)
            {
                try
                {
                    selectedGroup = group;
                    var result = _client.Read(group);
                    txtNode.Text = $"'{group.NameSpaceIndex}, {group.Identifier}'";
                    txtValue.Text = result.Value?.ToString() ?? "null";
                    string message = $"Source Timestamp: {result.SourceTimestamp}, " +
                        $"\n Source Time PicoSeconds: {result.SourcePicoseconds}" +
                        $"\n Server Timestamp: {result.ServerTimestamp}, " +
                        $"\n Source Time PicoSeconds: {result.ServerPicoseconds}," +
                        $"\n Status Code: {result.StatusCode}," +
                        $"\n Type: {group.NodeClass.ToString()}," +
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
                //AddTags(groupNode, group.Tags);

                parentNode.Nodes.Add(groupNode);
            }
        }

        //private void AddTags(TreeNode parentNode, List<Tag> tags)
        //{
        //    foreach (var tag in tags)
        //    {
        //        TreeNode tagNode = new TreeNode(tag.Name);
        //        tagNode.Tag = tag;

        //        parentNode.Nodes.Add(tagNode);
        //    }
        //}


        // New: Select Program

        private void btnSelectProgram_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            // Ask the user for the full program path
            string programPath = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the full path of the NC program to select:\n(e.g. /mnt/tnc/nc_prog/demo/Start_demo.h)",
                "Select Program",
                "/mnt/tnc/nc_prog/"
            );

            if (string.IsNullOrWhiteSpace(programPath))
                return;

            try
            {
                // Object: ns=1;i=100010  Method: ns=1;i=100012
                var objectId = new NodeId(100007, 1);
                var methodId = new NodeId(100012, 1);

                _client.CallMethod(objectId, methodId, programPath);

                Log($"SelectProgram called: {programPath}");
                MessageBox.Show($"Program selected:\n{programPath}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("SelectProgram error: " + ex.Message);
                MessageBox.Show(ex.Message, "SelectProgram Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelectProgramByNodeId_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the file node:\n(e.g. 12345)",
                "Select Program By NodeId",
                ""
            );

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!uint.TryParse(input, out uint identifier))
            {
                MessageBox.Show("Invalid identifier. Please enter a numeric value.");
                return;
            }

            try
            {
                var objectId = new NodeId(100007, 1);
                var methodId = new NodeId(100014, 1);

                // Argument is a NodeId, so we pass a NodeId object directly
                var fileNodeId = new NodeId(identifier, 1);

                _client.CallMethod(objectId, methodId, fileNodeId);

                Log($"SelectProgramByNodeId called with NodeId: ns=1;i={identifier}");
            }
            catch (Exception ex)
            {
                Log("SelectProgramByNodeId error: " + ex.Message);
                MessageBox.Show(ex.Message, "SelectProgramByNodeId Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Start
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            try
            {
                var objectId = new NodeId(100007, 1);
                var methodId = new NodeId(100019, 1);

                _client.CallMethod(objectId, methodId);

                Log("Start called successfully.");
            }
            catch (Exception ex)
            {
                Log("Start error: " + ex.Message);
                MessageBox.Show(ex.Message, "Start Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Stop
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            try
            {
                var objectId = new NodeId(100007, 1);
                var methodId = new NodeId(100020, 1);

                _client.CallMethod(objectId, methodId);

                Log("Stop called successfully.");
            }
            catch (Exception ex)
            {
                Log("Stop error: " + ex.Message);
                MessageBox.Show(ex.Message, "Start Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Reset
        private void btnDeselect_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            try
            {
                var objectId = new NodeId(100007, 1);
                var methodId = new NodeId(100018, 1);

                _client.CallMethod(objectId, methodId);

                Log("Deselect called successfully.");
            }
            catch (Exception ex)
            {
                Log("Deselect error: " + ex.Message);
                MessageBox.Show(ex.Message, "Start Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            try
            {
                var objectId = new NodeId(100007, 1);
                var methodId = new NodeId(100021, 1);

                _client.CallMethod(objectId, methodId);

                Log("Cancel called successfully.");
            }
            catch (Exception ex)
            {
                Log("Cancel error: " + ex.Message);
                MessageBox.Show(ex.Message, "Start Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateDirectory_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            string directoryName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the name of the new directory:",
                "Create Directory",
                ""
            );

            if (string.IsNullOrWhiteSpace(directoryName))
                return;

            // This should be the NodeId of the parent directory you want to create inside.
            // e.g. the TNC folder node from your tree browser.
            string parentNodeInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the parent directory node:\n(e.g. 12345)",
                "Parent Directory NodeId",
                ""
            );

            if (!uint.TryParse(parentNodeInput, out uint parentIdentifier))
            {
                MessageBox.Show("Invalid identifier.");
                return;
            }

            try
            {
                var objectId = new NodeId(parentIdentifier, 1);
                var methodId = new NodeId(13387, 0); // FileDirectoryType_CreateDirectory (standard OPC UA)

                var result = _client.CallMethod(objectId, methodId, directoryName);

                // Output is the new directory's NodeId
                if (result != null && result.Count > 0)
                {
                    var newDirNodeId = result[0] as NodeId;
                    Log($"Directory '{directoryName}' created. NodeId: {newDirNodeId}");
                    MessageBox.Show($"Directory created successfully.\nNodeId: {newDirNodeId}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Log($"Directory '{directoryName}' created.");
                }
            }
            catch (Exception ex)
            {
                Log("CreateDirectory error: " + ex.Message);
                MessageBox.Show(ex.Message, "CreateDirectory Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            string fileName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the name of the new file:\n(e.g. MyProgram.h)",
                "Create File",
                ""
            );

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string parentNodeInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the parent directory node:",
                "Parent Directory NodeId",
                ""
            );

            if (!uint.TryParse(parentNodeInput, out uint parentIdentifier))
            {
                MessageBox.Show("Invalid identifier.");
                return;
            }

            bool requestFileOpen = MessageBox.Show(
                "Open the file after creation?\n(Returns a file handle for read/write operations)",
                "Request File Open",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;

            try
            {
                var objectId = new NodeId(parentIdentifier, 1);
                var methodId = new NodeId(13390, 0); // FileDirectoryType_CreateFile

                var result = _client.CallMethod(objectId, methodId, fileName, requestFileOpen);

                if (result != null && result.Count >= 2)
                {
                    var fileNodeId = result[0] as NodeId;
                    var fileHandle = Convert.ToUInt32(result[1]);

                    Log($"File '{fileName}' created. NodeId: {fileNodeId}, Handle: {fileHandle}");
                    MessageBox.Show(
                        $"File created successfully.\nNodeId: {fileNodeId}\nFile Handle: {fileHandle}" +
                        (requestFileOpen ? "\n\nFile is open — remember to close it when done." : ""),
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Log($"File '{fileName}' created.");
                }
            }
            catch (Exception ex)
            {
                Log("CreateFile error: " + ex.Message);
                MessageBox.Show(ex.Message, "CreateFile Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            string parentNodeInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the parent directory node:",
                "Parent Directory NodeId",
                ""
            );

            if (!uint.TryParse(parentNodeInput, out uint parentIdentifier))
            {
                MessageBox.Show("Invalid identifier.");
                return;
            }

            string deleteNode = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the file node:\n(e.g. 12345)",
                "Delete File",
                ""
            );

            if (string.IsNullOrWhiteSpace(deleteNode))
                return;

            if (!uint.TryParse(deleteNode, out uint deleteIdentifier))
            {
                MessageBox.Show("Invalid identifier. Please enter a numeric value.");
                return;
            }

            try
            {
                var objectId = new NodeId(parentIdentifier, 1);
                var methodId = new NodeId(13393, 0); // FileDirectoryType_CreateFile

                var fileNodeId = new NodeId(deleteIdentifier, 1);

                _client.CallMethod(objectId, methodId, fileNodeId);

                MessageBox.Show(
                        $"File {fileNodeId} deleted successfully from root {objectId}.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("CreateFile error: " + ex.Message);
                MessageBox.Show(ex.Message, "CreateFile Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMoveOrCopy_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            string parentNodeInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the parent directory node:",
                "Parent Directory NodeId",
                ""
            );

            if (!uint.TryParse(parentNodeInput, out uint parentIdentifier))
            {
                MessageBox.Show("Invalid identifier.");
                return;
            }

            string sourceInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the file/directory to move or copy:",
                "Source NodeId",
                ""
            );

            if (!uint.TryParse(sourceInput, out uint sourceIdentifier))
            {
                MessageBox.Show("Invalid source identifier.");
                return;
            }

            string targetInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the target directory:",
                "Target Directory NodeId",
                ""
            );

            if (!uint.TryParse(targetInput, out uint targetIdentifier))
            {
                MessageBox.Show("Invalid target identifier.");
                return;
            }

            string newName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the new name (leave blank to keep the original name):",
                "New Name",
                ""
            );

            bool createCopy = MessageBox.Show(
                "Create a copy?\n(Yes = Copy, No = Move)",
                "Copy or Move",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;

            try
            {
                // The object to call the method on is the SOURCE's parent directory
                // or the root FileSystem node — use the target directory node here
                // as MoveOrCopy is called on the FileSystem/directory object
                var objectId = new NodeId(parentIdentifier, 1);
                var methodId = new NodeId(13395, 0); // FileDirectoryType_MoveOrCopy

                var objectToMoveOrCopy = new NodeId(sourceIdentifier, 1);
                var targetDirectory = new NodeId(targetIdentifier, 1);

                var result = _client.CallMethod(
                    objectId,
                    methodId,
                    objectToMoveOrCopy,    // NodeId  - source
                    targetDirectory,        // NodeId  - destination directory
                    createCopy,             // Boolean - copy or move
                    newName                 // String  - new name (empty = keep original)
                );

                if (result != null && result.Count > 0)
                {
                    var newNodeId = result[0] as NodeId;
                    string operation = createCopy ? "copied" : "moved";
                    Log($"Node {sourceIdentifier} {operation} successfully. New NodeId: {newNodeId}");
                    MessageBox.Show(
                        $"Operation successful.\nNew NodeId: {newNodeId}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log("MoveOrCopy error: " + ex.Message);
                MessageBox.Show(ex.Message, "MoveOrCopy Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (_client == null) { MessageBox.Show("Not connected."); return; }

            string nodeInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the numeric identifier of the file node to open:",
                "File NodeId", ""
            );
            if (!uint.TryParse(nodeInput, out uint fileIdentifier)) { MessageBox.Show("Invalid identifier."); return; }

            string modeInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the open mode:\n  1 = Read\n  2 = Write (append)\n  6 = Write (erase existing)",
                "Open Mode", "1"
            );
            if (!byte.TryParse(modeInput, out byte mode)) { MessageBox.Show("Invalid mode."); return; }

            try
            {
                var objectId = new NodeId(fileIdentifier, 1);
                var methodId = new NodeId(11580, 0);

                // Be explicit — wrap in Variant with correct TypeInfo
                var inputArgs = new VariantCollection
        {
            new Variant(mode, new TypeInfo(BuiltInType.Byte, ValueRanks.Scalar))
        };

                _client._session.Call(
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

                if (results == null || results.Count == 0 || StatusCode.IsBad(results[0].StatusCode))
                    throw new Exception($"Open failed: {results?[0].StatusCode}");

                _currentFileHandle = Convert.ToUInt32(results[0].OutputArguments[0].Value);
                _currentFileNodeId = fileIdentifier;

                Log($"File opened. NodeId: ns=1;i={_currentFileNodeId}, Handle: {_currentFileHandle}");
                MessageBox.Show($"File opened.\nHandle: {_currentFileHandle}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("Open error: " + ex.Message);
                MessageBox.Show(ex.Message, "Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_client == null) { MessageBox.Show("Not connected."); return; }

            if (_currentFileHandle == 0 || _currentFileNodeId == 0)
            {
                MessageBox.Show("No file is currently open. Use Open first.");
                return;
            }

            try
            {
                var objectId = new NodeId(_currentFileNodeId, 1); // called ON the file node
                var methodId = new NodeId(11583, 0);              // FileType_Close

                _client.CallMethod(objectId, methodId, _currentFileHandle);

                Log($"File closed. Handle: {_currentFileHandle}");
                MessageBox.Show("File closed successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the stored handle
                _currentFileHandle = 0;
                _currentFileNodeId = 0;
            }
            catch (Exception ex)
            {
                Log("Close error: " + ex.Message);
                MessageBox.Show(ex.Message, "Close Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            if (_client == null) { MessageBox.Show("Not connected."); return; }


            if (_currentFileHandle == 0 || _currentFileNodeId == 0)
            {
                MessageBox.Show("No file is currently open. Use Open first.");
                return;
            }

            string lengthInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the number of bytes to read:\n(-1 = read entire file)",
                "Read Length", "-1"
            );
            if (!int.TryParse(lengthInput, out int length)) { MessageBox.Show("Invalid length."); return; }

            try
            {
                var objectId = new NodeId(_currentFileNodeId, 1); // called ON the file node
                var methodId = new NodeId(11585, 0);              // FileType_Read

                var result = _client.CallMethod(objectId, methodId, _currentFileHandle, length);

                if (result != null && result.Count > 0)
                {
                    byte[] data = result[0] as byte[];

                    if (data == null || data.Length == 0)
                    {
                        ShowValue("File is empty or no data returned.");
                        Log("Read complete — no data.");
                        return;
                    }

                    string content = System.Text.Encoding.UTF8.GetString(data);
                    ShowValue(content);
                    Log($"Read {data.Length} bytes from file handle {_currentFileHandle}.");
                }
            }
            catch (Exception ex)
            {
                Log("Read error: " + ex.Message);
                MessageBox.Show(ex.Message, "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            if (_client == null) { MessageBox.Show("Not connected."); return; }

            if (_currentFileHandle == 0 || _currentFileNodeId == 0)
            {
                MessageBox.Show("No file is currently open. Use Open first.");
                return;
            }

            string content = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the content to write to the file:",
                "Write File Content", ""
            );

            if (content == null) return;

            try
            {
                var objectId = new NodeId(_currentFileNodeId, 1);
                var methodId = new NodeId(11588, 0); // FileType_Write (corrected)

                byte[] data = System.Text.Encoding.UTF8.GetBytes(content);

                var inputArgs = new VariantCollection
        {
            new Variant(_currentFileHandle, new TypeInfo(BuiltInType.UInt32, ValueRanks.Scalar)),
            new Variant(data, new TypeInfo(BuiltInType.ByteString, ValueRanks.Scalar))
        };

                _client.Session.Call(
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

                if (results == null || results.Count == 0 || StatusCode.IsBad(results[0].StatusCode))
                    throw new Exception($"Write failed: {results?[0].StatusCode}");

                Log($"Wrote {data.Length} bytes to file handle {_currentFileHandle}.");
                MessageBox.Show($"Write successful.\n{data.Length} bytes written.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("Write error: " + ex.Message);
                MessageBox.Show(ex.Message, "Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGetPosition_Click(object sender, EventArgs e)
        {
            if (_client == null) { MessageBox.Show("Not connected."); return; }

            if (_currentFileHandle == 0 || _currentFileNodeId == 0)
            {
                MessageBox.Show("No file is currently open. Use Open first.");
                return;
            }

            try
            {
                var objectId = new NodeId(_currentFileNodeId, 1);
                var methodId = new NodeId(11590, 0); // FileType_GetPosition

                var inputArgs = new VariantCollection
        {
            new Variant(_currentFileHandle, new TypeInfo(BuiltInType.UInt32, ValueRanks.Scalar))
        };

                _client.Session.Call(
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

                if (results == null || results.Count == 0 || StatusCode.IsBad(results[0].StatusCode))
                    throw new Exception($"GetPosition failed: {results?[0].StatusCode}");

                ulong position = Convert.ToUInt64(results[0].OutputArguments[0].Value);

                Log($"Current position: {position} bytes.");
                MessageBox.Show($"Current file position: {position} bytes.", "GetPosition",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("GetPosition error: " + ex.Message);
                MessageBox.Show(ex.Message, "GetPosition Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSetPosition_Click(object sender, EventArgs e)
        {
            if (_client == null) { MessageBox.Show("Not connected."); return; }

            if (_currentFileHandle == 0 || _currentFileNodeId == 0)
            {
                MessageBox.Show("No file is currently open. Use Open first.");
                return;
            }

            string positionInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the byte position to seek to:\n(0 = beginning of file)",
                "Set Position", "0"
            );
            if (!ulong.TryParse(positionInput, out ulong position)) { MessageBox.Show("Invalid position."); return; }

            try
            {
                var objectId = new NodeId(_currentFileNodeId, 1);
                var methodId = new NodeId(11593, 0); // FileType_SetPosition

                var inputArgs = new VariantCollection
        {
            new Variant(_currentFileHandle, new TypeInfo(BuiltInType.UInt32, ValueRanks.Scalar)),
            new Variant(position, new TypeInfo(BuiltInType.UInt64, ValueRanks.Scalar))
        };

                _client._session.Call(
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

                if (results == null || results.Count == 0 || StatusCode.IsBad(results[0].StatusCode))
                    throw new Exception($"SetPosition failed: {results?[0].StatusCode}");

                Log($"Position set to {position} bytes.");
                MessageBox.Show($"Position set to {position} bytes.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("SetPosition error: " + ex.Message);
                MessageBox.Show(ex.Message, "SetPosition Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clear Log
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