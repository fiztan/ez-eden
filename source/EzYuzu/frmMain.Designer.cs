namespace EzYuzu
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            lblEdenLocation = new Label();
            txtEdenLocation = new TextBox();
            btnBrowse = new Button();
            pbarCurrentProgress = new ProgressBar();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            generalToolStripMenuItem = new ToolStripMenuItem();
            updateEdenToolStripMenuItem = new ToolStripMenuItem();
            autoupdateOnEzEdenStartToolStripMenuItem = new ToolStripMenuItem();
            exitAfterUpdateToolStripMenuItem = new ToolStripMenuItem();
            launchEdenAfterUpdateToolStripMenuItem = new ToolStripMenuItem();
            enterDownloadURLToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            edenWebsiteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            btnProcess = new Button();
            lblProgress = new Label();
            toolTip1 = new ToolTip(components);
            lblUpdateVersion = new Label();
            cboUpdateVersion = new ComboBox();
            grpOptions = new GroupBox();
            lblChangelog = new Label();
            txtChangelog = new RichTextBox();
            menuStrip1.SuspendLayout();
            grpOptions.SuspendLayout();
            SuspendLayout();
            //
            // lblEdenLocation
            //
            lblEdenLocation.AutoSize = true;
            lblEdenLocation.Location = new Point(13, 37);
            lblEdenLocation.Margin = new Padding(4, 0, 4, 0);
            lblEdenLocation.Name = "lblEdenLocation";
            lblEdenLocation.Size = new Size(196, 15);
            lblEdenLocation.TabIndex = 0;
            lblEdenLocation.Text = "Browse to Eden.exe Folder Location:";
            toolTip1.SetToolTip(lblEdenLocation, "Click and browse to your Eden.exe Folder Location");
            lblEdenLocation.Click += LblEdenLocation_Click;
            //
            // txtEdenLocation
            //
            txtEdenLocation.Location = new Point(14, 55);
            txtEdenLocation.Margin = new Padding(4, 3, 4, 3);
            txtEdenLocation.Name = "txtEdenLocation";
            txtEdenLocation.ReadOnly = true;
            txtEdenLocation.Size = new Size(363, 23);
            txtEdenLocation.TabIndex = 1;
            toolTip1.SetToolTip(txtEdenLocation, "Click and browse to your Eden.exe Folder Location");
            txtEdenLocation.Click += TxtEdenLocation_Click;
            //
            // btnBrowse
            //
            btnBrowse.Location = new Point(385, 53);
            btnBrowse.Margin = new Padding(4, 3, 4, 3);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(28, 27);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "...";
            toolTip1.SetToolTip(btnBrowse, "Click and browse to your Eden.exe Folder Location");
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += BtnBrowse_ClickAsync;
            //
            // pbarCurrentProgress
            //
            pbarCurrentProgress.Location = new Point(14, 598);
            pbarCurrentProgress.Margin = new Padding(4, 3, 4, 3);
            pbarCurrentProgress.Name = "pbarCurrentProgress";
            pbarCurrentProgress.Size = new Size(399, 23);
            pbarCurrentProgress.TabIndex = 3;
            toolTip1.SetToolTip(pbarCurrentProgress, "Progress completed of current action");
            //
            // menuStrip1
            //
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(427, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            //
            // fileToolStripMenuItem
            //
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            //
            // exitToolStripMenuItem
            //
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(92, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_ClickAsync;
            //
            // optionsToolStripMenuItem
            //
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generalToolStripMenuItem, enterDownloadURLToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            //
            // generalToolStripMenuItem
            //
            generalToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { updateEdenToolStripMenuItem });
            generalToolStripMenuItem.Name = "generalToolStripMenuItem";
            generalToolStripMenuItem.Size = new Size(180, 22);
            generalToolStripMenuItem.Text = "General";
            //
            // updateEdenToolStripMenuItem
            //
            updateEdenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { autoupdateOnEzEdenStartToolStripMenuItem, exitAfterUpdateToolStripMenuItem, launchEdenAfterUpdateToolStripMenuItem });
            updateEdenToolStripMenuItem.Name = "updateEdenToolStripMenuItem";
            updateEdenToolStripMenuItem.Size = new Size(180, 22);
            updateEdenToolStripMenuItem.Text = "Update Eden";
            //
            // autoupdateOnEzEdenStartToolStripMenuItem
            //
            autoupdateOnEzEdenStartToolStripMenuItem.CheckOnClick = true;
            autoupdateOnEzEdenStartToolStripMenuItem.Name = "autoupdateOnEzEdenStartToolStripMenuItem";
            autoupdateOnEzEdenStartToolStripMenuItem.Size = new Size(227, 22);
            autoupdateOnEzEdenStartToolStripMenuItem.Text = "Auto-Update on EzEden Start";
            autoupdateOnEzEdenStartToolStripMenuItem.ToolTipText = "Automatically update Eden when EzEden is launched";
            //
            // exitAfterUpdateToolStripMenuItem
            //
            exitAfterUpdateToolStripMenuItem.CheckOnClick = true;
            exitAfterUpdateToolStripMenuItem.Name = "exitAfterUpdateToolStripMenuItem";
            exitAfterUpdateToolStripMenuItem.Size = new Size(227, 22);
            exitAfterUpdateToolStripMenuItem.Text = "Exit EzEden After Update";
            exitAfterUpdateToolStripMenuItem.ToolTipText = "Exit EzEden after Eden has been updated";
            //
            // launchEdenAfterUpdateToolStripMenuItem
            //
            launchEdenAfterUpdateToolStripMenuItem.CheckOnClick = true;
            launchEdenAfterUpdateToolStripMenuItem.Name = "launchEdenAfterUpdateToolStripMenuItem";
            launchEdenAfterUpdateToolStripMenuItem.Size = new Size(227, 22);
            launchEdenAfterUpdateToolStripMenuItem.Text = "Launch Eden After Update";
            launchEdenAfterUpdateToolStripMenuItem.ToolTipText = "Launch Eden after Update or New Install is complete";
            //
            // enterDownloadURLToolStripMenuItem
            //
            enterDownloadURLToolStripMenuItem.Name = "enterDownloadURLToolStripMenuItem";
            enterDownloadURLToolStripMenuItem.Size = new Size(227, 22);
            enterDownloadURLToolStripMenuItem.Text = "Enter Download URL...";
            enterDownloadURLToolStripMenuItem.ToolTipText = "Manually enter an Eden download URL (use when offline)";
            enterDownloadURLToolStripMenuItem.Click += EnterDownloadURLToolStripMenuItem_Click;
            //
            // helpToolStripMenuItem
            //
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { edenWebsiteToolStripMenuItem, toolStripSeparator1, aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            //
            // edenWebsiteToolStripMenuItem
            //
            edenWebsiteToolStripMenuItem.Name = "edenWebsiteToolStripMenuItem";
            edenWebsiteToolStripMenuItem.Size = new Size(145, 22);
            edenWebsiteToolStripMenuItem.Text = "Eden Website";
            edenWebsiteToolStripMenuItem.Click += EdenWebsiteToolStripMenuItem_Click;
            //
            // toolStripSeparator1
            //
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(142, 6);
            //
            // aboutToolStripMenuItem
            //
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(145, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            //
            // btnProcess
            //
            btnProcess.Enabled = false;
            btnProcess.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnProcess.Location = new Point(14, 264);
            btnProcess.Margin = new Padding(4, 3, 4, 3);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(399, 73);
            btnProcess.TabIndex = 7;
            btnProcess.Text = "Select the Directory containing Eden.exe";
            toolTip1.SetToolTip(btnProcess, "Click to download the latest version of Eden");
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += BtnProcess_ClickAsync;
            //
            // lblProgress
            //
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(13, 580);
            lblProgress.Margin = new Padding(4, 0, 4, 0);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(0, 15);
            lblProgress.TabIndex = 9;
            //
            // lblChangelog
            //
            lblChangelog.AutoSize = true;
            lblChangelog.Location = new Point(14, 344);
            lblChangelog.Name = "lblChangelog";
            lblChangelog.Size = new Size(120, 15);
            lblChangelog.TabIndex = 16;
            lblChangelog.Text = "Changelog (latest):";
            //
            // txtChangelog
            //
            txtChangelog.BackColor = SystemColors.Window;
            txtChangelog.Location = new Point(14, 362);
            txtChangelog.Name = "txtChangelog";
            txtChangelog.ReadOnly = true;
            txtChangelog.Size = new Size(399, 212);
            txtChangelog.TabIndex = 17;
            txtChangelog.Text = "";
            //
            // lblUpdateVersion
            //
            lblUpdateVersion.AutoSize = true;
            lblUpdateVersion.Location = new Point(7, 19);
            lblUpdateVersion.Name = "lblUpdateVersion";
            lblUpdateVersion.Size = new Size(89, 15);
            lblUpdateVersion.TabIndex = 14;
            lblUpdateVersion.Text = "Update Version:";
            //
            // cboUpdateVersion
            //
            cboUpdateVersion.DropDownHeight = 75;
            cboUpdateVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUpdateVersion.Enabled = false;
            cboUpdateVersion.FormattingEnabled = true;
            cboUpdateVersion.IntegralHeight = false;
            cboUpdateVersion.Location = new Point(6, 37);
            cboUpdateVersion.MaxDropDownItems = 10;
            cboUpdateVersion.Name = "cboUpdateVersion";
            cboUpdateVersion.Size = new Size(389, 23);
            cboUpdateVersion.TabIndex = 15;
            cboUpdateVersion.SelectedIndexChanged += CboUpdateVersion_SelectedIndexChangedAsync;
            //
            // grpOptions
            //
            grpOptions.Controls.Add(cboUpdateVersion);
            grpOptions.Controls.Add(lblUpdateVersion);
            grpOptions.Location = new Point(12, 86);
            grpOptions.Name = "grpOptions";
            grpOptions.Size = new Size(403, 70);
            grpOptions.TabIndex = 12;
            grpOptions.TabStop = false;
            grpOptions.Text = "Options";
            //
            // FrmMain
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(427, 630);
            Controls.Add(txtChangelog);
            Controls.Add(lblChangelog);
            Controls.Add(grpOptions);
            Controls.Add(lblProgress);
            Controls.Add(btnProcess);
            Controls.Add(pbarCurrentProgress);
            Controls.Add(btnBrowse);
            Controls.Add(txtEdenLocation);
            Controls.Add(lblEdenLocation);
            Controls.Add(menuStrip1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EzEden - Eden Portable Updater";
            FormClosed += FrmMain_FormClosedAsync;
            Load += FrmMain_LoadAsync;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            grpOptions.ResumeLayout(false);
            grpOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblEdenLocation;
        private System.Windows.Forms.TextBox txtEdenLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ProgressBar pbarCurrentProgress;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edenWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateEdenToolStripMenuItem;
        private ToolStripMenuItem launchEdenAfterUpdateToolStripMenuItem;
        private Label lblUpdateVersion;
        private ComboBox cboUpdateVersion;
        private GroupBox grpOptions;
        private ToolStripMenuItem exitAfterUpdateToolStripMenuItem;
        private ToolStripMenuItem autoupdateOnEzEdenStartToolStripMenuItem;
        private ToolStripMenuItem enterDownloadURLToolStripMenuItem;
        private Label lblChangelog;
        private RichTextBox txtChangelog;
    }
}
