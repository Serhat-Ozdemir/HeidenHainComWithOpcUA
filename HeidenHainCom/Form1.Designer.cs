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
            btnReset = new Button();
            lstLog = new ListBox();
            lblLog = new Label();
            btnClearLog = new Button();
            lblSep = new Label();
            treeOpc = new TreeView();
            rtbValue = new RichTextBox();
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
            txtValue.Location = new Point(650, 65);
            txtValue.Name = "txtValue";
            txtValue.ReadOnly = true;
            txtValue.Size = new Size(220, 31);
            txtValue.TabIndex = 8;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Location = new Point(600, 20);
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
            btnSelectProgram.Size = new Size(140, 35);
            btnSelectProgram.TabIndex = 13;
            btnSelectProgram.Text = "Select Program";
            btnSelectProgram.UseVisualStyleBackColor = false;
            btnSelectProgram.Click += btnSelectProgram_Click;
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
            // btnReset
            // 
            btnReset.BackColor = Color.FromArgb(255, 152, 0);
            btnReset.Enabled = false;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(400, 235);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(130, 40);
            btnReset.TabIndex = 16;
            btnReset.Text = "↺  RESET";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1896, 1099);
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
            Controls.Add(btnReset);
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
        private Button btnReset;
        private Label lblLog;
        private ListBox lstLog;
        private Button btnClearLog;
        private Label lblSep;
        private TreeView treeOpc;
        private RichTextBox rtbValue;
    }
}