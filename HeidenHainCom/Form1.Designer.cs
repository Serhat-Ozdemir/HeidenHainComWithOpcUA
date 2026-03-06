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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Top bar controls ──────────────────────────────────────────────
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblServerLabel = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.btnDisconnectMonitor = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();

            // ── Cert / Key pickers ────────────────────────────────────────────
            this.lblServerCertLabel = new System.Windows.Forms.Label();
            this.txtServerCertPath = new System.Windows.Forms.TextBox();
            this.btnBrowseServerCert = new System.Windows.Forms.Button();
            this.lblCertLabel = new System.Windows.Forms.Label();
            this.txtCertPath = new System.Windows.Forms.TextBox();
            this.btnBrowseCert = new System.Windows.Forms.Button();
            this.lblKeyLabel = new System.Windows.Forms.Label();
            this.txtKeyPath = new System.Windows.Forms.TextBox();
            this.btnBrowseKey = new System.Windows.Forms.Button();

            // ── Main split ────────────────────────────────────────────────────
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitLeft = new System.Windows.Forms.SplitContainer();

            // ── Left-top: device tree ─────────────────────────────────────────
            this.pnlTree = new System.Windows.Forms.Panel();
            this.lblTreeTitle = new System.Windows.Forms.Label();
            this.treeDevices = new System.Windows.Forms.TreeView();
            this.btnRefresh = new System.Windows.Forms.Button();

            // ── Left-bottom: error list ───────────────────────────────────────
            this.pnlErrors = new System.Windows.Forms.Panel();
            this.lblErrorsTitle = new System.Windows.Forms.Label();
            this.lstErrors = new System.Windows.Forms.ListBox();


            // ── Right panel ───────────────────────────────────────────────────
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlCode = new System.Windows.Forms.Panel();
            this.lblCodeTitle = new System.Windows.Forms.Label();
            this.rtbCode = new System.Windows.Forms.RichTextBox();
            this.pnlCodeButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCycleStart = new System.Windows.Forms.Button();
            this.btnCycleStop = new System.Windows.Forms.Button();

            // ── Status panel (right-bottom) ───────────────────────────────────
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatusTitle = new System.Windows.Forms.Label();

            this.lblActiveProgramLbl = new System.Windows.Forms.Label();
            this.lblActiveProgramVal = new System.Windows.Forms.Label();
            this.lblCurrentStateLbl = new System.Windows.Forms.Label();
            this.lblCurrentStateVal = new System.Windows.Forms.Label();
            this.lblCurrentToolLbl = new System.Windows.Forms.Label();
            this.lblCurrentToolVal = new System.Windows.Forms.Label();
            this.lblCounterLbl = new System.Windows.Forms.Label();
            this.lblCounterVal = new System.Windows.Forms.Label();
            this.lblSpindleLbl = new System.Windows.Forms.Label();
            this.lblSpindleVal = new System.Windows.Forms.Label();
            this.lblOperationModeLbl = new System.Windows.Forms.Label();
            this.lblOperationModeVal = new System.Windows.Forms.Label();

            this.lblFeedLbl = new System.Windows.Forms.Label();
            this.pbFeed = new System.Windows.Forms.ProgressBar();
            this.lblFeedPct = new System.Windows.Forms.Label();
            this.lblRapidLbl = new System.Windows.Forms.Label();
            this.pbRapid = new System.Windows.Forms.ProgressBar();
            this.lblRapidPct = new System.Windows.Forms.Label();
            this.lblSpeedLbl = new System.Windows.Forms.Label();
            this.pbSpeed = new System.Windows.Forms.ProgressBar();
            this.lblSpeedPct = new System.Windows.Forms.Label();

            // ── Status bar ────────────────────────────────────────────────────
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusBar = new System.Windows.Forms.ToolStripStatusLabel();

            // ─────────────────────────────────────────────────────────────────
            // Begin suspend layout
            // ─────────────────────────────────────────────────────────────────
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitMain).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitLeft).BeginInit();
            this.splitLeft.Panel1.SuspendLayout();
            this.splitLeft.Panel2.SuspendLayout();
            this.splitLeft.SuspendLayout();
            this.pnlTree.SuspendLayout();
            this.pnlErrors.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlCode.SuspendLayout();
            this.pnlCodeButtons.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // ─────────────────────────────────────────────────────────────────
            // Color palette
            // ─────────────────────────────────────────────────────────────────
            var clrBackground = System.Drawing.Color.FromArgb(30, 30, 30);
            var clrPanel = System.Drawing.Color.FromArgb(40, 40, 40);
            var clrPanelLight = System.Drawing.Color.FromArgb(50, 50, 50);
            var clrBorder = System.Drawing.Color.FromArgb(65, 65, 65);
            var clrAccent = System.Drawing.Color.FromArgb(0, 150, 136);   // teal
            var clrAccentHover = System.Drawing.Color.FromArgb(0, 188, 166);
            var clrDanger = System.Drawing.Color.FromArgb(211, 47, 47);
            var clrForeground = System.Drawing.Color.FromArgb(220, 220, 220);
            var clrMuted = System.Drawing.Color.FromArgb(150, 150, 150);

            var fontNormal = new System.Drawing.Font("Segoe UI", 9F);
            var fontBold = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            var fontSmall = new System.Drawing.Font("Segoe UI", 8F);
            var fontTitle = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            var fontMono = new System.Drawing.Font("Consolas", 9.5F);

            // ─────────────────────────────────────────────────────────────────
            // TOP BAR
            // ─────────────────────────────────────────────────────────────────
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Height = 110;
            this.pnlTopBar.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = clrAccent;
            this.lblTitle.Location = new System.Drawing.Point(10, 13);
            this.lblTitle.Text = "HeidenhainCom";

            this.lblServerLabel.AutoSize = true;
            this.lblServerLabel.Font = fontNormal;
            this.lblServerLabel.ForeColor = clrMuted;
            this.lblServerLabel.Location = new System.Drawing.Point(160, 15);
            this.lblServerLabel.Text = "Server:";

            this.txtServer.BackColor = clrPanelLight;
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServer.Font = fontNormal;
            this.txtServer.ForeColor = clrForeground;
            this.txtServer.Location = new System.Drawing.Point(208, 12);
            this.txtServer.Size = new System.Drawing.Size(280, 23);
            this.txtServer.Text = "opc.tcp://localhost:4840";

            // Helper to style flat buttons
            void StyleBtn(System.Windows.Forms.Button b, System.Drawing.Color bg, string text, int x)
            {
                b.BackColor = bg;
                b.FlatAppearance.BorderSize = 0;
                b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                b.Font = fontBold;
                b.ForeColor = System.Drawing.Color.White;
                b.Location = new System.Drawing.Point(x, 10);
                b.Size = new System.Drawing.Size(95, 26);
                b.Text = text;
                b.UseVisualStyleBackColor = false;
                b.Cursor = System.Windows.Forms.Cursors.Hand;
            }

            StyleBtn(this.btnConnect, clrAccent, "Connect", 500);
            StyleBtn(this.btnDisconnect, clrBorder, "Disconnect", 602);
            StyleBtn(this.btnReconnect, System.Drawing.Color.FromArgb(70, 70, 70), "Reconnect", 704);
            StyleBtn(this.btnDisconnectMonitor, System.Drawing.Color.FromArgb(70, 70, 70), "Stop Monitor", 806);

            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = fontBold;
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblConnectionStatus.Location = new System.Drawing.Point(910, 15);
            this.lblConnectionStatus.Text = "● Disconnected";

            // ── Second row: certificates + key pickers ────────────────────────
            this.lblServerCertLabel = new System.Windows.Forms.Label();
            this.txtServerCertPath = new System.Windows.Forms.TextBox();
            this.btnBrowseServerCert = new System.Windows.Forms.Button();

            this.lblServerCertLabel.AutoSize = true;
            this.lblServerCertLabel.Font = fontNormal;
            this.lblServerCertLabel.ForeColor = clrMuted;
            this.lblServerCertLabel.Location = new System.Drawing.Point(10, 78);
            this.lblServerCertLabel.Text = "Server Cert (.der):";

            this.txtServerCertPath.BackColor = clrPanelLight;
            this.txtServerCertPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerCertPath.Font = fontSmall;
            this.txtServerCertPath.ForeColor = clrForeground;
            this.txtServerCertPath.Location = new System.Drawing.Point(118, 75);
            this.txtServerCertPath.Size = new System.Drawing.Size(320, 22);
            this.txtServerCertPath.ReadOnly = true;

            this.btnBrowseServerCert.BackColor = clrPanelLight;
            this.btnBrowseServerCert.FlatAppearance.BorderSize = 0;
            this.btnBrowseServerCert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseServerCert.Font = fontSmall;
            this.btnBrowseServerCert.ForeColor = clrAccent;
            this.btnBrowseServerCert.Location = new System.Drawing.Point(446, 75);
            this.btnBrowseServerCert.Size = new System.Drawing.Size(60, 22);
            this.btnBrowseServerCert.Text = "Browse…";
            this.btnBrowseServerCert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseServerCert.UseVisualStyleBackColor = false;
            this.btnBrowseServerCert.Click += (s, e) =>
            {
                using var dlg = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "Select Server Certificate (.der)",
                    Filter = "DER Certificate (*.der)|*.der|All files (*.*)|*.*"
                };

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.txtServerCertPath.Text = dlg.FileName;
            };

            this.lblCertLabel.AutoSize = true;
            this.lblCertLabel.Font = fontNormal;
            this.lblCertLabel.ForeColor = clrMuted;
            this.lblCertLabel.Location = new System.Drawing.Point(10, 54);
            this.lblCertLabel.Text = "User Cert (.der):";

            this.txtCertPath.BackColor = clrPanelLight;
            this.txtCertPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCertPath.Font = fontSmall;
            this.txtCertPath.ForeColor = clrForeground;
            this.txtCertPath.Location = new System.Drawing.Point(118, 51);
            this.txtCertPath.Size = new System.Drawing.Size(320, 22);
            this.txtCertPath.ReadOnly = true;
            this.txtCertPath.Text = "";

            this.btnBrowseCert.BackColor = clrPanelLight;
            this.btnBrowseCert.FlatAppearance.BorderSize = 0;
            this.btnBrowseCert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseCert.Font = fontSmall;
            this.btnBrowseCert.ForeColor = clrAccent;
            this.btnBrowseCert.Location = new System.Drawing.Point(446, 51);
            this.btnBrowseCert.Size = new System.Drawing.Size(60, 22);
            this.btnBrowseCert.Text = "Browse…";
            this.btnBrowseCert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseCert.UseVisualStyleBackColor = false;
            this.btnBrowseCert.Click += (s, e) =>
            {
                using var dlg = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "Select User Certificate (.der)",
                    Filter = "DER Certificate (*.der)|*.der|All files (*.*)|*.*"
                };
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.txtCertPath.Text = dlg.FileName;
            };

            this.lblKeyLabel.AutoSize = true;
            this.lblKeyLabel.Font = fontNormal;
            this.lblKeyLabel.ForeColor = clrMuted;
            this.lblKeyLabel.Location = new System.Drawing.Point(520, 54);
            this.lblKeyLabel.Text = "User Key (.key):";

            this.txtKeyPath.BackColor = clrPanelLight;
            this.txtKeyPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKeyPath.Font = fontSmall;
            this.txtKeyPath.ForeColor = clrForeground;
            this.txtKeyPath.Location = new System.Drawing.Point(626, 51);
            this.txtKeyPath.Size = new System.Drawing.Size(320, 22);
            this.txtKeyPath.ReadOnly = true;
            this.txtKeyPath.Text = "";

            this.btnBrowseKey.BackColor = clrPanelLight;
            this.btnBrowseKey.FlatAppearance.BorderSize = 0;
            this.btnBrowseKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseKey.Font = fontSmall;
            this.btnBrowseKey.ForeColor = clrAccent;
            this.btnBrowseKey.Location = new System.Drawing.Point(954, 51);
            this.btnBrowseKey.Size = new System.Drawing.Size(60, 22);
            this.btnBrowseKey.Text = "Browse…";
            this.btnBrowseKey.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseKey.UseVisualStyleBackColor = false;
            this.btnBrowseKey.Click += (s, e) =>
            {
                using var dlg = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "Select User Key (.key)",
                    Filter = "Key file (*.key)|*.key|All files (*.*)|*.*"
                };
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.txtKeyPath.Text = dlg.FileName;
            };

            this.pnlTopBar.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblServerCertLabel,
                this.txtServerCertPath,
                this.btnBrowseServerCert,
                this.lblTitle, this.lblServerLabel, this.txtServer,
                this.btnConnect, this.btnDisconnect, this.btnReconnect,
                this.btnDisconnectMonitor, this.lblConnectionStatus,
                this.lblCertLabel, this.txtCertPath, this.btnBrowseCert,
                this.lblKeyLabel,  this.txtKeyPath,  this.btnBrowseKey
            });

            // ─────────────────────────────────────────────────────────────────
            // MAIN SPLIT
            // ─────────────────────────────────────────────────────────────────
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.SplitterDistance = 320;
            this.splitMain.SplitterWidth = 4;
            this.splitMain.BackColor = clrBorder;

            // ── LEFT SPLIT ────────────────────────────────────────────────────
            this.splitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitLeft.SplitterDistance = 380;
            this.splitLeft.SplitterWidth = 4;
            this.splitLeft.BackColor = clrBorder;

            // ── TREE PANEL ────────────────────────────────────────────────────
            this.pnlTree.BackColor = clrPanel;
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTree.Padding = new System.Windows.Forms.Padding(6);

            this.lblTreeTitle.AutoSize = false;
            this.lblTreeTitle.BackColor = clrPanelLight;
            this.lblTreeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTreeTitle.Font = fontTitle;
            this.lblTreeTitle.ForeColor = clrAccent;
            this.lblTreeTitle.Height = 28;
            this.lblTreeTitle.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.lblTreeTitle.Text = "Device Tree";

            this.treeDevices.BackColor = clrPanel;
            this.treeDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeDevices.Font = fontNormal;
            this.treeDevices.ForeColor = clrForeground;
            this.treeDevices.LineColor = clrBorder;

            this.btnRefresh.BackColor = clrPanelLight;
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = fontBold;
            this.btnRefresh.ForeColor = clrAccent;
            this.btnRefresh.Height = 28;
            this.btnRefresh.Text = "⟳  Refresh Tree";
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlTree.Controls.Add(this.treeDevices);
            this.pnlTree.Controls.Add(this.btnRefresh);
            this.pnlTree.Controls.Add(this.lblTreeTitle);

            // ── ERRORS PANEL ──────────────────────────────────────────────────
            this.pnlErrors.BackColor = clrPanel;
            this.pnlErrors.Dock = System.Windows.Forms.DockStyle.Fill;

            this.lblErrorsTitle.AutoSize = false;
            this.lblErrorsTitle.BackColor = clrPanelLight;
            this.lblErrorsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblErrorsTitle.Font = fontTitle;
            this.lblErrorsTitle.ForeColor = clrDanger;
            this.lblErrorsTitle.Height = 28;
            this.lblErrorsTitle.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.lblErrorsTitle.Text = "Errors / Alarms";

            this.lstErrors.BackColor = clrPanel;
            this.lstErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstErrors.Font = fontSmall;
            this.lstErrors.ForeColor = System.Drawing.Color.FromArgb(255, 138, 128);
            this.lstErrors.IntegralHeight = false;

            this.pnlErrors.Controls.Add(this.lstErrors);
            this.pnlErrors.Controls.Add(this.lblErrorsTitle);

            this.splitLeft.Panel1.Controls.Add(this.pnlTree);
            this.splitLeft.Panel2.Controls.Add(this.pnlErrors);
            this.splitLeft.Panel1.BackColor = clrPanel;
            this.splitLeft.Panel2.BackColor = clrPanel;

            this.splitMain.Panel1.Controls.Add(this.splitLeft);
            this.splitMain.Panel1.BackColor = clrPanel;

            // ─────────────────────────────────────────────────────────────────
            // RIGHT PANEL
            // ─────────────────────────────────────────────────────────────────
            this.pnlRight.BackColor = clrBackground;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── CODE PANEL ────────────────────────────────────────────────────
            this.pnlCode.BackColor = clrPanel;
            this.pnlCode.Dock = System.Windows.Forms.DockStyle.Fill;

            this.lblCodeTitle.AutoSize = false;
            this.lblCodeTitle.BackColor = clrPanelLight;
            this.lblCodeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCodeTitle.Font = fontTitle;
            this.lblCodeTitle.ForeColor = clrAccent;
            this.lblCodeTitle.Height = 28;
            this.lblCodeTitle.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.lblCodeTitle.Text = "Program / Node Value";

            this.rtbCode.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.rtbCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCode.Font = fontMono;
            this.rtbCode.ForeColor = System.Drawing.Color.FromArgb(204, 204, 204);
            this.rtbCode.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.rtbCode.WordWrap = false;

            // ── CODE BUTTONS ──────────────────────────────────────────────────
            this.pnlCodeButtons.BackColor = clrPanelLight;
            this.pnlCodeButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCodeButtons.Height = 42;
            this.pnlCodeButtons.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);

            this.btnSave.BackColor = clrAccent;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = fontBold;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(6, 7);
            this.btnSave.Size = new System.Drawing.Size(110, 28);
            this.btnSave.Text = "💾  Save (Ctrl+S)";
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.UseVisualStyleBackColor = false;

            this.btnCycleStart.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnCycleStart.FlatAppearance.BorderSize = 0;
            this.btnCycleStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleStart.Font = fontBold;
            this.btnCycleStart.ForeColor = System.Drawing.Color.White;
            this.btnCycleStart.Location = new System.Drawing.Point(124, 7);
            this.btnCycleStart.Size = new System.Drawing.Size(110, 28);
            this.btnCycleStart.Text = "▶  Cycle Start";
            this.btnCycleStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCycleStart.UseVisualStyleBackColor = false;

            this.btnCycleStop.BackColor = clrDanger;
            this.btnCycleStop.FlatAppearance.BorderSize = 0;
            this.btnCycleStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleStop.Font = fontBold;
            this.btnCycleStop.ForeColor = System.Drawing.Color.White;
            this.btnCycleStop.Location = new System.Drawing.Point(242, 7);
            this.btnCycleStop.Size = new System.Drawing.Size(110, 28);
            this.btnCycleStop.Text = "■  Cycle Stop";
            this.btnCycleStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCycleStop.UseVisualStyleBackColor = false;

            this.pnlCodeButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnSave, this.btnCycleStart, this.btnCycleStop
            });

            this.pnlCode.Controls.Add(this.rtbCode);
            this.pnlCode.Controls.Add(this.pnlCodeButtons);
            this.pnlCode.Controls.Add(this.lblCodeTitle);

            // ─────────────────────────────────────────────────────────────────
            // STATUS PANEL (bottom of right)
            // ─────────────────────────────────────────────────────────────────
            this.pnlStatus.BackColor = clrPanel;
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Height = 200;
            this.pnlStatus.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);

            this.lblStatusTitle.AutoSize = false;
            this.lblStatusTitle.BackColor = clrPanelLight;
            this.lblStatusTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatusTitle.Font = fontTitle;
            this.lblStatusTitle.ForeColor = clrAccent;
            this.lblStatusTitle.Height = 28;
            this.lblStatusTitle.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.lblStatusTitle.Text = "Machine Status";

            // Helper for status label pairs
            (System.Windows.Forms.Label lbl, System.Windows.Forms.Label val)
                MakePair(string caption, int x, int y)
            {
                var l = new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Font = fontSmall,
                    ForeColor = clrMuted,
                    Location = new System.Drawing.Point(x, y),
                    Text = caption
                };
                var v = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Font = fontBold,
                    ForeColor = clrForeground,
                    Location = new System.Drawing.Point(x, y + 16),
                    Size = new System.Drawing.Size(210, 18),
                    Text = "–"
                };
                return (l, v);
            }

            // Helper for override rows
            (System.Windows.Forms.Label lbl, System.Windows.Forms.ProgressBar pb,
             System.Windows.Forms.Label pct) MakeOverride(string caption, int y)
            {
                var l = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Font = fontSmall,
                    ForeColor = clrMuted,
                    Location = new System.Drawing.Point(10, y),
                    Size = new System.Drawing.Size(80, 16),
                    Text = caption
                };
                var p = new System.Windows.Forms.ProgressBar
                {
                    Location = new System.Drawing.Point(90, y),
                    Maximum = 100,
                    Minimum = 0,
                    Size = new System.Drawing.Size(180, 14),
                    Style = System.Windows.Forms.ProgressBarStyle.Continuous,
                    Value = 0
                };
                var pct = new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Font = fontSmall,
                    ForeColor = clrForeground,
                    Location = new System.Drawing.Point(278, y),
                    Text = "0%"
                };
                return (l, p, pct);
            }

            var (lLbl, lVal) = MakePair("Active Program", 10, 36);
            var (sLbl, sVal) = MakePair("Current State", 230, 36);
            var (tLbl, tVal) = MakePair("Current Tool", 450, 36);
            var (cLbl, cVal) = MakePair("Counter", 670, 36);
            var (spLbl, spVal) = MakePair("Spindle Speed", 10, 80);
            var (omLbl, omVal) = MakePair("Operation Mode", 230, 80);

            this.lblActiveProgramLbl = lLbl; this.lblActiveProgramVal = lVal;
            this.lblCurrentStateLbl = sLbl; this.lblCurrentStateVal = sVal;
            this.lblCurrentToolLbl = tLbl; this.lblCurrentToolVal = tVal;
            this.lblCounterLbl = cLbl; this.lblCounterVal = cVal;
            this.lblSpindleLbl = spLbl; this.lblSpindleVal = spVal;
            this.lblOperationModeLbl = omLbl; this.lblOperationModeVal = omVal;

            var (fl, fp, fpc) = MakeOverride("Feed Override", 120);
            var (rl, rp, rpc) = MakeOverride("Rapid Override", 142);
            var (sl2, sp2, spc2) = MakeOverride("Speed Override", 164);  // renamed to avoid clash

            this.lblFeedLbl = fl; this.pbFeed = fp; this.lblFeedPct = fpc;
            this.lblRapidLbl = rl; this.pbRapid = rp; this.lblRapidPct = rpc;
            this.lblSpeedLbl = sl2; this.pbSpeed = sp2; this.lblSpeedPct = spc2;

            this.pnlStatus.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblStatusTitle,
                this.lblActiveProgramLbl, this.lblActiveProgramVal,
                this.lblCurrentStateLbl,  this.lblCurrentStateVal,
                this.lblCurrentToolLbl,   this.lblCurrentToolVal,
                this.lblCounterLbl,       this.lblCounterVal,
                this.lblSpindleLbl,       this.lblSpindleVal,
                this.lblOperationModeLbl, this.lblOperationModeVal,
                this.lblFeedLbl,  this.pbFeed,  this.lblFeedPct,
                this.lblRapidLbl, this.pbRapid, this.lblRapidPct,
                this.lblSpeedLbl, this.pbSpeed, this.lblSpeedPct,
            });

            this.pnlRight.Controls.Add(this.pnlCode);
            this.pnlRight.Controls.Add(this.pnlStatus);

            this.splitMain.Panel2.Controls.Add(this.pnlRight);
            this.splitMain.Panel2.BackColor = clrBackground;

            // ─────────────────────────────────────────────────────────────────
            // STATUS STRIP
            // ─────────────────────────────────────────────────────────────────
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.statusStrip.Items.Add(this.lblStatusBar);
            this.statusStrip.SizingGrip = false;

            this.lblStatusBar.ForeColor = clrMuted;
            this.lblStatusBar.Font = fontSmall;
            this.lblStatusBar.Text = " Ready.";

            // ─────────────────────────────────────────────────────────────────
            // FORM
            // ─────────────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = clrBackground;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Font = fontNormal;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Text = "HeidenhainCom – OPC UA Monitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.statusStrip);

            // ─────────────────────────────────────────────────────────────────
            // Resume layout
            // ─────────────────────────────────────────────────────────────────
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.splitLeft.Panel1.ResumeLayout(false);
            this.splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitLeft).EndInit();
            this.splitLeft.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitMain).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlTree.ResumeLayout(false);
            this.pnlErrors.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlCode.ResumeLayout(false);
            this.pnlCodeButtons.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ── Top bar ──────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblServerLabel;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Button btnDisconnectMonitor;
        private System.Windows.Forms.Label lblConnectionStatus;

        // ── Main layout ──────────────────────────────────────────────────────
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.SplitContainer splitLeft;

        // ── Tree ─────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlTree;
        private System.Windows.Forms.Label lblTreeTitle;
        private System.Windows.Forms.TreeView treeDevices;
        private System.Windows.Forms.Button btnRefresh;

        // ── Errors ───────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlErrors;
        private System.Windows.Forms.Label lblErrorsTitle;
        private System.Windows.Forms.ListBox lstErrors;

        // ── Right area ───────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlCode;
        private System.Windows.Forms.Label lblCodeTitle;
        private System.Windows.Forms.RichTextBox rtbCode;
        private System.Windows.Forms.Panel pnlCodeButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCycleStart;
        private System.Windows.Forms.Button btnCycleStop;

        // ── Status panel ─────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatusTitle;

        private System.Windows.Forms.Label lblActiveProgramLbl;
        private System.Windows.Forms.Label lblActiveProgramVal;
        private System.Windows.Forms.Label lblCurrentStateLbl;
        private System.Windows.Forms.Label lblCurrentStateVal;
        private System.Windows.Forms.Label lblCurrentToolLbl;
        private System.Windows.Forms.Label lblCurrentToolVal;
        private System.Windows.Forms.Label lblCounterLbl;
        private System.Windows.Forms.Label lblCounterVal;
        private System.Windows.Forms.Label lblSpindleLbl;
        private System.Windows.Forms.Label lblSpindleVal;
        private System.Windows.Forms.Label lblOperationModeLbl;
        private System.Windows.Forms.Label lblOperationModeVal;

        private System.Windows.Forms.Label lblFeedLbl;
        private System.Windows.Forms.ProgressBar pbFeed;
        private System.Windows.Forms.Label lblFeedPct;
        private System.Windows.Forms.Label lblRapidLbl;
        private System.Windows.Forms.ProgressBar pbRapid;
        private System.Windows.Forms.Label lblRapidPct;
        private System.Windows.Forms.Label lblSpeedLbl;
        private System.Windows.Forms.ProgressBar pbSpeed;
        private System.Windows.Forms.Label lblSpeedPct;

        // ── Certs / Key pickers ───────────────────────────────────────────────
        private System.Windows.Forms.Label lblServerCertLabel;
        private System.Windows.Forms.TextBox txtServerCertPath;
        private System.Windows.Forms.Button btnBrowseServerCert;
        private System.Windows.Forms.Label lblCertLabel;
        private System.Windows.Forms.TextBox txtCertPath;
        private System.Windows.Forms.Button btnBrowseCert;
        private System.Windows.Forms.Label lblKeyLabel;
        private System.Windows.Forms.TextBox txtKeyPath;
        private System.Windows.Forms.Button btnBrowseKey;

        // ── Status strip ─────────────────────────────────────────────────────
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusBar;
    }
}