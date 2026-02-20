namespace HeidenHainCom
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtServer = new TextBox();
            txtNode = new TextBox();
            txtValue = new TextBox();
            lblStatus = new Label();
            btnConnect = new Button();
            btnRead = new Button();
            txtStatus = new TextBox();
            lblServer = new Label();
            lblNode = new Label();
            lblValue = new Label();
            lblProgramPath = new Label();
            txtProgramPath = new TextBox();
            btnSelectProgram = new Button();
            btnStart = new Button();
            btnStop = new Button();
            btnDeselect = new Button();
            lstLog = new ListBox();
            lblLog = new Label();
            btnClearLog = new Button();
            lblSep = new Label();
            treeOpc = new TreeView();
            rtbValue = new RichTextBox();
            btnDisconnect = new Button();
            btnWrite = new Button();
            btnCancel = new Button();
            btnCreateDirectory = new Button();
            btnCreateFile = new Button();
            btnDelete = new Button();
            btnMoveOrCopy = new Button();
            btnWriteFile = new Button();
            btnReadFile = new Button();
            btnClose = new Button();
            btnOpen = new Button();
            btnSetPosition = new Button();
            btnGetPosition = new Button();
            SuspendLayout();
            // 
            // txtServer
            // 
            txtServer.Location = new Point(156, 15);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(294, 31);
            txtServer.TabIndex = 1;
            // 
            // txtNode
            // 
            txtNode.Location = new Point(156, 65);
            txtNode.Name = "txtNode";
            txtNode.Size = new Size(294, 31);
            txtNode.TabIndex = 5;
            // 
            // txtValue
            // 
            txtValue.BackColor = Color.White;
            txtValue.Location = new Point(664, 65);
            txtValue.Name = "txtValue";
            txtValue.Size = new Size(206, 31);
            txtValue.TabIndex = 8;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Location = new Point(727, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(143, 25);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "● Disconnected";
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(76, 175, 80);
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(465, 13);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(120, 35);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnRead
            // 
            btnRead.Location = new Point(465, 63);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(120, 35);
            btnRead.TabIndex = 6;
            btnRead.Text = "Read";
            btnRead.Click += btnRead_Click;
            // 
            // txtStatus
            // 
            txtStatus.BackColor = Color.White;
            txtStatus.Font = new Font("Segoe UI", 8.5F);
            txtStatus.Location = new Point(20, 110);
            txtStatus.Name = "txtStatus";
            txtStatus.ReadOnly = true;
            txtStatus.Size = new Size(850, 30);
            txtStatus.TabIndex = 9;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(20, 18);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(101, 25);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server URL:";
            // 
            // lblNode
            // 
            lblNode.AutoSize = true;
            lblNode.Location = new Point(20, 68);
            lblNode.Name = "lblNode";
            lblNode.Size = new Size(130, 25);
            lblNode.TabIndex = 4;
            lblNode.Text = "Node Address:";
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Location = new Point(600, 68);
            lblValue.Name = "lblValue";
            lblValue.Size = new Size(58, 25);
            lblValue.TabIndex = 7;
            lblValue.Text = "Value:";
            // 
            // lblProgramPath
            // 
            lblProgramPath.AutoSize = true;
            lblProgramPath.Location = new Point(20, 188);
            lblProgramPath.Name = "lblProgramPath";
            lblProgramPath.Size = new Size(114, 25);
            lblProgramPath.TabIndex = 11;
            lblProgramPath.Text = "NC Program:";
            // 
            // txtProgramPath
            // 
            txtProgramPath.Location = new Point(156, 185);
            txtProgramPath.Name = "txtProgramPath";
            txtProgramPath.Size = new Size(294, 31);
            txtProgramPath.TabIndex = 12;
            txtProgramPath.Text = "TNC:\\nc_prog\\test.h";
            // 
            // btnSelectProgram
            // 
            btnSelectProgram.BackColor = Color.FromArgb(33, 150, 243);
            btnSelectProgram.Enabled = false;
            btnSelectProgram.FlatStyle = FlatStyle.Flat;
            btnSelectProgram.ForeColor = Color.White;
            btnSelectProgram.Location = new Point(465, 183);
            btnSelectProgram.Name = "btnSelectProgram";
            btnSelectProgram.Size = new Size(151, 35);
            btnSelectProgram.TabIndex = 13;
            btnSelectProgram.Text = "Select Program";
            btnSelectProgram.UseVisualStyleBackColor = false;
            btnSelectProgram.Click += btnSelectProgramByNodeId_Click;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(76, 175, 80);
            btnStart.Enabled = false;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(110, 235);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(130, 40);
            btnStart.TabIndex = 14;
            btnStart.Text = "▶  START";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(244, 67, 54);
            btnStop.Enabled = false;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(255, 235);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(130, 40);
            btnStop.TabIndex = 15;
            btnStop.Text = "■  STOP";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnDeselect
            // 
            btnDeselect.BackColor = Color.FromArgb(255, 152, 0);
            btnDeselect.Enabled = false;
            btnDeselect.FlatStyle = FlatStyle.Flat;
            btnDeselect.ForeColor = Color.White;
            btnDeselect.Location = new Point(400, 235);
            btnDeselect.Name = "btnDeselect";
            btnDeselect.Size = new Size(130, 40);
            btnDeselect.TabIndex = 16;
            btnDeselect.Text = "↺  DESELECT";
            btnDeselect.UseVisualStyleBackColor = false;
            btnDeselect.Click += btnDeselect_Click;
            // 
            // lstLog
            // 
            lstLog.ItemHeight = 25;
            lstLog.Location = new Point(20, 318);
            lstLog.Name = "lstLog";
            lstLog.ScrollAlwaysVisible = true;
            lstLog.Size = new Size(850, 179);
            lstLog.TabIndex = 18;
            // 
            // lblLog
            // 
            lblLog.AutoSize = true;
            lblLog.Location = new Point(20, 295);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(109, 25);
            lblLog.TabIndex = 17;
            lblLog.Text = "Activity Log:";
            // 
            // btnClearLog
            // 
            btnClearLog.Location = new Point(20, 528);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(100, 30);
            btnClearLog.TabIndex = 19;
            btnClearLog.Text = "Clear Log";
            btnClearLog.Click += btnClearLog_Click;
            // 
            // lblSep
            // 
            lblSep.Location = new Point(0, 0);
            lblSep.Name = "lblSep";
            lblSep.Size = new Size(100, 23);
            lblSep.TabIndex = 10;
            // 
            // treeOpc
            // 
            treeOpc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            treeOpc.Font = new Font("Segoe UI", 9F);
            treeOpc.Location = new Point(20, 578);
            treeOpc.Name = "treeOpc";
            treeOpc.Size = new Size(400, 509);
            treeOpc.Sorted = true;
            treeOpc.TabIndex = 0;
            // 
            // rtbValue
            // 
            rtbValue.Location = new Point(526, 587);
            rtbValue.Name = "rtbValue";
            rtbValue.Size = new Size(663, 500);
            rtbValue.TabIndex = 20;
            rtbValue.Text = "";
            // 
            // btnDisconnect
            // 
            btnDisconnect.BackColor = Color.Red;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.ForeColor = Color.White;
            btnDisconnect.Location = new Point(591, 13);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(120, 35);
            btnDisconnect.TabIndex = 21;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnWrite
            // 
            btnWrite.Location = new Point(889, 65);
            btnWrite.Name = "btnWrite";
            btnWrite.Size = new Size(120, 35);
            btnWrite.TabIndex = 22;
            btnWrite.Text = "Write";
            btnWrite.Click += btnWrite_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Thistle;
            btnCancel.Enabled = false;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(548, 235);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 40);
            btnCancel.TabIndex = 23;
            btnCancel.Text = "X  CANCEL";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnCreateDirectory
            // 
            btnCreateDirectory.BackColor = Color.FromArgb(33, 150, 243);
            btnCreateDirectory.Enabled = false;
            btnCreateDirectory.FlatStyle = FlatStyle.Flat;
            btnCreateDirectory.ForeColor = Color.White;
            btnCreateDirectory.Location = new Point(1225, 600);
            btnCreateDirectory.Name = "btnCreateDirectory";
            btnCreateDirectory.Size = new Size(160, 35);
            btnCreateDirectory.TabIndex = 24;
            btnCreateDirectory.Text = "Create Directory";
            btnCreateDirectory.UseVisualStyleBackColor = false;
            btnCreateDirectory.Click += btnCreateDirectory_Click;
            // 
            // btnCreateFile
            // 
            btnCreateFile.BackColor = Color.FromArgb(33, 150, 243);
            btnCreateFile.Enabled = false;
            btnCreateFile.FlatStyle = FlatStyle.Flat;
            btnCreateFile.ForeColor = Color.White;
            btnCreateFile.Location = new Point(1225, 650);
            btnCreateFile.Name = "btnCreateFile";
            btnCreateFile.Size = new Size(160, 35);
            btnCreateFile.TabIndex = 25;
            btnCreateFile.Text = "Create File";
            btnCreateFile.UseVisualStyleBackColor = false;
            btnCreateFile.Click += btnCreateFile_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(33, 150, 243);
            btnDelete.Enabled = false;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(1225, 700);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(160, 35);
            btnDelete.TabIndex = 26;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnMoveOrCopy
            // 
            btnMoveOrCopy.BackColor = Color.FromArgb(33, 150, 243);
            btnMoveOrCopy.Enabled = false;
            btnMoveOrCopy.FlatStyle = FlatStyle.Flat;
            btnMoveOrCopy.ForeColor = Color.White;
            btnMoveOrCopy.Location = new Point(1225, 750);
            btnMoveOrCopy.Name = "btnMoveOrCopy";
            btnMoveOrCopy.Size = new Size(160, 35);
            btnMoveOrCopy.TabIndex = 27;
            btnMoveOrCopy.Text = "MoveOrCopy";
            btnMoveOrCopy.UseVisualStyleBackColor = false;
            btnMoveOrCopy.Click += btnMoveOrCopy_Click;
            // 
            // btnWriteFile
            // 
            btnWriteFile.BackColor = Color.FromArgb(33, 150, 243);
            btnWriteFile.Enabled = false;
            btnWriteFile.FlatStyle = FlatStyle.Flat;
            btnWriteFile.ForeColor = Color.White;
            btnWriteFile.Location = new Point(1430, 750);
            btnWriteFile.Name = "btnWriteFile";
            btnWriteFile.Size = new Size(160, 35);
            btnWriteFile.TabIndex = 31;
            btnWriteFile.Text = "Write";
            btnWriteFile.UseVisualStyleBackColor = false;
            btnWriteFile.Click += btnWriteFile_Click;
            // 
            // btnReadFile
            // 
            btnReadFile.BackColor = Color.FromArgb(33, 150, 243);
            btnReadFile.Enabled = false;
            btnReadFile.FlatStyle = FlatStyle.Flat;
            btnReadFile.ForeColor = Color.White;
            btnReadFile.Location = new Point(1430, 700);
            btnReadFile.Name = "btnReadFile";
            btnReadFile.Size = new Size(160, 35);
            btnReadFile.TabIndex = 30;
            btnReadFile.Text = "Read";
            btnReadFile.UseVisualStyleBackColor = false;
            btnReadFile.Click += btnReadFile_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(33, 150, 243);
            btnClose.Enabled = false;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1430, 650);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(160, 35);
            btnClose.TabIndex = 29;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnOpen
            // 
            btnOpen.BackColor = Color.FromArgb(33, 150, 243);
            btnOpen.Enabled = false;
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.ForeColor = Color.White;
            btnOpen.Location = new Point(1430, 600);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(160, 35);
            btnOpen.TabIndex = 28;
            btnOpen.Text = "Open";
            btnOpen.UseVisualStyleBackColor = false;
            btnOpen.Click += btnOpen_Click;
            // 
            // btnSetPosition
            // 
            btnSetPosition.BackColor = Color.FromArgb(33, 150, 243);
            btnSetPosition.Enabled = false;
            btnSetPosition.FlatStyle = FlatStyle.Flat;
            btnSetPosition.ForeColor = Color.White;
            btnSetPosition.Location = new Point(1430, 850);
            btnSetPosition.Name = "btnSetPosition";
            btnSetPosition.Size = new Size(160, 35);
            btnSetPosition.TabIndex = 33;
            btnSetPosition.Text = "Set Position";
            btnSetPosition.UseVisualStyleBackColor = false;
            btnSetPosition.Click += btnSetPosition_Click;
            // 
            // btnGetPosition
            // 
            btnGetPosition.BackColor = Color.FromArgb(33, 150, 243);
            btnGetPosition.Enabled = false;
            btnGetPosition.FlatStyle = FlatStyle.Flat;
            btnGetPosition.ForeColor = Color.White;
            btnGetPosition.Location = new Point(1430, 800);
            btnGetPosition.Name = "btnGetPosition";
            btnGetPosition.Size = new Size(160, 35);
            btnGetPosition.TabIndex = 32;
            btnGetPosition.Text = "Get Position";
            btnGetPosition.UseVisualStyleBackColor = false;
            btnGetPosition.Click += btnGetPosition_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1896, 1099);
            Controls.Add(btnSetPosition);
            Controls.Add(btnGetPosition);
            Controls.Add(btnWriteFile);
            Controls.Add(btnReadFile);
            Controls.Add(btnClose);
            Controls.Add(btnOpen);
            Controls.Add(btnMoveOrCopy);
            Controls.Add(btnDelete);
            Controls.Add(btnCreateFile);
            Controls.Add(btnCreateDirectory);
            Controls.Add(btnCancel);
            Controls.Add(btnWrite);
            Controls.Add(btnDisconnect);
            Controls.Add(rtbValue);
            Controls.Add(treeOpc);
            Controls.Add(lblServer);
            Controls.Add(txtServer);
            Controls.Add(btnConnect);
            Controls.Add(lblStatus);
            Controls.Add(lblNode);
            Controls.Add(txtNode);
            Controls.Add(btnRead);
            Controls.Add(lblValue);
            Controls.Add(txtValue);
            Controls.Add(txtStatus);
            Controls.Add(lblSep);
            Controls.Add(lblProgramPath);
            Controls.Add(txtProgramPath);
            Controls.Add(btnSelectProgram);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(btnDeselect);
            Controls.Add(lblLog);
            Controls.Add(lstLog);
            Controls.Add(btnClearLog);
            Name = "Form1";
            Text = "HeidenHain TNC7 – OPC UA Controller";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        // ── Existing ──────────────────────────────────────────────────
        private TextBox txtServer;
        private TextBox txtNode;
        private TextBox txtValue;
        private Label lblStatus;
        private Button btnConnect;
        private Button btnRead;
        private TextBox txtStatus;

        // ── New ───────────────────────────────────────────────────────
        private Label lblServer;
        private Label lblNode;
        private Label lblValue;
        private Label lblProgramPath;
        private TextBox txtProgramPath;
        private Button btnSelectProgram;
        private Button btnStart;
        private Button btnStop;
        private Button btnDeselect;
        private Label lblLog;
        private ListBox lstLog;
        private Button btnClearLog;
        private Label lblSep;
        private TreeView treeOpc;
        private RichTextBox rtbValue;
        private Button btnDisconnect;
        private Button btnWrite;
        private Button btnCancel;
        private Button btnCreateDirectory;
        private Button btnCreateFile;
        private Button btnDelete;
        private Button btnMoveOrCopy;
        private Button btnWriteFile;
        private Button btnReadFile;
        private Button btnClose;
        private Button btnOpen;
        private Button btnSetPosition;
        private Button btnGetPosition;
    }
}