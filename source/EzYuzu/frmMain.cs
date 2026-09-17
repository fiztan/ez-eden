using EzYuzu.Classes.Settings;
using EzYuzu.Classes.Yuzu.Detectors;
using EzYuzu.Classes.Yuzu.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;
using static EzYuzu.Classes.Yuzu.Detectors.EdenInstallationStateDetector;

namespace EzYuzu
{
    public partial class FrmMain : Form
    {
        private readonly IHttpClientFactory? clientFactory;
        private readonly EdenBranchDetector? branchDetector;
        private bool networkAvailable;
        private readonly Dictionary<string, string> changelogs = new();

        public FrmMain()
        {
            InitializeComponent();
            LoadApplicationSettings(IsControlDown());
        }

        public FrmMain(IServiceProvider serviceProvider) : this()
        {
            clientFactory = serviceProvider.GetService<IHttpClientFactory>();
            branchDetector = new EdenBranchDetector(clientFactory!);
        }

        private async void FrmMain_LoadAsync(object sender, EventArgs e)
        {
            await RefreshVersionsAsync();

            if (!string.IsNullOrWhiteSpace(txtEdenLocation.Text) && networkAvailable)
            {
                var installationState = await RefreshDetectedEdenInstallationStateAsync();

                if (installationState == EdenInstallationState.UpdateAvailable && autoupdateOnEzEdenStartToolStripMenuItem.Checked)
                    BtnProcess_ClickAsync(sender, e);

                if (installationState == EdenInstallationState.LatestVersionInstalled && launchEdenAfterUpdateToolStripMenuItem.Checked)
                    LaunchEden(txtEdenLocation.Text);
            }
        }

        private async void FrmMain_FormClosedAsync(object sender, FormClosedEventArgs e)
        {
            await SaveApplicationSettings();
        }

        private async void CboUpdateVersion_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            if (!cboUpdateVersion.Enabled || !networkAvailable)
                return;

            ShowChangelog(cboUpdateVersion.Text);
            await RefreshDetectedEdenInstallationStateAsync();
        }

        private void ShowChangelog(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName) || !changelogs.TryGetValue(tagName, out var text))
            {
                txtChangelog.Clear();
                return;
            }

            txtChangelog.Clear();
            txtChangelog.Text = text;
            txtChangelog.SelectionStart = 0;
            txtChangelog.ScrollToCaret();
        }

        private void LblEdenLocation_Click(object sender, EventArgs e)
        {
            BtnBrowse_ClickAsync(sender, e);
        }

        private void TxtEdenLocation_Click(object sender, EventArgs e)
        {
            BtnBrowse_ClickAsync(sender, e);
        }

        private async void BtnBrowse_ClickAsync(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
            txtEdenLocation.Text = await ShowFolderBrowserDialogWindowAndGetResultAsync();

            if (networkAvailable)
            {
                await RefreshDetectedEdenInstallationUpdateVersionsAsync();
                await RefreshDetectedEdenInstallationStateAsync();
            }
        }

        private async void BtnProcess_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEdenLocation.Text))
            {
                MessageBox.Show("Please browse and select your Eden.exe folder location first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string edenLocation = txtEdenLocation.Text;
            string edenTagName;
            string edenDownloadUrl;

            if (networkAvailable && cboUpdateVersion.SelectedValue?.ToString() is not null)
            {
                edenTagName = cboUpdateVersion.Text;
                edenDownloadUrl = cboUpdateVersion.SelectedValue?.ToString()!;
            }
            else
            {
                var result = ShowManualUrlDialog();
                if (result is null)
                    return;
                edenTagName = result.Value.tagName;
                edenDownloadUrl = result.Value.downloadUrl;
            }

            ToggleUiControls(false);
            btnProcess.Enabled = false;

            var edenManager = new EdenNightlyManager(clientFactory!, edenTagName, edenDownloadUrl);
            edenManager.EdenDirectoryPath = edenLocation;
            edenManager.TempUpdateDirectoryPath = Path.Combine(edenLocation, "TempUpdate");
            edenManager.UpdateProgress += EzEdenDownloader_UpdateCurrentProgress;
            await edenManager.DownloadPrerequisitesAsync();

            if (btnProcess.Text == "New Install")
                await edenManager.ProcessEdenNewInstallationAsync();
            else if (btnProcess.Text.StartsWith("Update", StringComparison.Ordinal))
                await edenManager.ProcessEdenUpdateAsync();

            if (networkAvailable)
            {
                await RefreshDetectedEdenInstallationUpdateVersionsAsync();
                await RefreshDetectedEdenInstallationStateAsync();
            }

            lblProgress.Text = "Done!";
            ToggleUiControls(true);

            if (launchEdenAfterUpdateToolStripMenuItem.Checked)
                LaunchEden(edenLocation);

            if (exitAfterUpdateToolStripMenuItem.Checked)
                Application.Exit();
        }

        private void EzEdenDownloader_UpdateCurrentProgress(int progressPercentage, string progressText)
        {
            pbarCurrentProgress.Value = progressPercentage;
            pbarCurrentProgress.Refresh();
            lblProgress.Text = progressText;
            lblProgress.Refresh();
        }

        //// ====================================================
        ////  MENUSTRIP CONTROLS
        //// ====================================================

        private void EdenWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://eden-emu.dev/")
            {
                UseShellExecute = true,
                Verb = "Open"
            })?.Dispose();
        }

        private async void ExitToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            await SaveApplicationSettings();
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new FrmAbout();
            f.ShowDialog(this);
        }

        private void EnterDownloadURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEdenLocation.Text))
            {
                MessageBox.Show("Please browse and select your Eden.exe folder location first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = ShowManualUrlDialog();
            if (result is null)
                return;

            BtnProcess_ClickAsync(sender, e);
        }

        //// ====================================================
        ////  METHODS
        //// ====================================================

        private async Task RefreshVersionsAsync()
        {
            if (branchDetector is null) return;

            lblProgress.Text = "Checking for updates...";
            var versionsWithChangelog = await branchDetector.GetAvailableUpdateVersionsWithChangelogAsync();

            if (versionsWithChangelog.Count == 0)
            {
                networkAvailable = false;
                lblProgress.Text = "No connection to git.eden-emu.dev. Use Options > Enter Download URL.";
                cboUpdateVersion.DataSource = null;
                cboUpdateVersion.Items.Clear();
                cboUpdateVersion.Items.Add("(No connection - use manual URL)");
                cboUpdateVersion.SelectedIndex = 0;
                cboUpdateVersion.Enabled = false;
                btnProcess.Text = "Download Eden";
                btnProcess.Enabled = !string.IsNullOrWhiteSpace(txtEdenLocation.Text);
                toolTip1.SetToolTip(btnProcess, "Click to download Eden using a manual URL");
                txtChangelog.Clear();
                return;
            }

            networkAvailable = true;
            changelogs.Clear();
            var versions = new List<KeyValuePair<string, string>>();
            foreach (var v in versionsWithChangelog)
            {
                versions.Add(new KeyValuePair<string, string>(v.tagName, v.url));
                changelogs[v.tagName] = v.changelog;
            }

            cboUpdateVersion.DataSource = versions;
            cboUpdateVersion.DisplayMember = "Key";
            cboUpdateVersion.ValueMember = "Value";
            cboUpdateVersion.Enabled = true;
            ShowChangelog(cboUpdateVersion.Text);
            await RefreshDetectedEdenInstallationStateAsync();
        }

        private (string tagName, string downloadUrl)? ShowManualUrlDialog()
        {
            using var form = new Form()
            {
                Text = "Enter Eden Download URL",
                Size = new Size(550, 180),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new Label() { Text = "Paste the download URL for Eden Windows (amd64-clang-pgo.zip):", Location = new Point(10, 10), Size = new Size(520, 20) };
            var txtUrl = new TextBox() { Location = new Point(10, 35), Size = new Size(510, 23) };
            var lblTag = new Label() { Text = "Version tag (e.g. v1789157782.8a22f1845b):", Location = new Point(10, 65), Size = new Size(520, 20) };
            var txtTag = new TextBox() { Location = new Point(10, 90), Size = new Size(510, 23) };
            var btnOk = new Button() { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(350, 120), Size = new Size(80, 28) };
            var btnCancel = new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(440, 120), Size = new Size(80, 28) };

            form.Controls.AddRange(new Control[] { lbl, txtUrl, lblTag, txtTag, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            if (form.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txtUrl.Text) && !string.IsNullOrWhiteSpace(txtTag.Text))
            {
                return (txtTag.Text.Trim(), txtUrl.Text.Trim());
            }
            return null;
        }

        private void LoadApplicationSettings(bool isLaunchedInSafeMode)
        {
            if (isLaunchedInSafeMode)
                this.Text = $"{this.Text} - Safe Mode";

            if (!File.Exists("EzEden.settings.json"))
                return;

            string json = File.ReadAllText("EzEden.settings.json");
            var settings = JsonSerializer.Deserialize<EzEdenSettings>(json)!;
            txtEdenLocation.Text = settings.EdenLocation;

            if (!isLaunchedInSafeMode)
            {
                launchEdenAfterUpdateToolStripMenuItem.Checked = settings.LaunchEdenAfterUpdate;
                exitAfterUpdateToolStripMenuItem.Checked = settings.ExitEdenAfterUpdate;
                autoupdateOnEzEdenStartToolStripMenuItem.Checked = settings.AutoUpdateEdenOnEzEdenLaunch;
                if (!Directory.Exists(txtEdenLocation.Text))
                {
                    txtEdenLocation.Clear();
                }
            }
        }

        private void ToggleUiControls(bool value)
        {
            lblEdenLocation.Enabled = value;
            txtEdenLocation.Enabled = value;
            btnBrowse.Enabled = value;
            grpOptions.Enabled = value;
            optionsToolStripMenuItem.Enabled = value;
        }

        private async Task SaveApplicationSettings()
        {
            var appSettings = new EzEdenSettings()
            {
                EdenLocation = txtEdenLocation.Text,
                LaunchEdenAfterUpdate = launchEdenAfterUpdateToolStripMenuItem.Checked,
                ExitEdenAfterUpdate = exitAfterUpdateToolStripMenuItem.Checked,
                AutoUpdateEdenOnEzEdenLaunch = autoupdateOnEzEdenStartToolStripMenuItem.Checked
            };
            await using var file = File.Create(Path.Combine(AppContext.BaseDirectory, "EzEden.settings.json"));
            await JsonSerializer.SerializeAsync(file, appSettings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        private async Task RefreshDetectedEdenInstallationUpdateVersionsAsync()
        {
            if (branchDetector is null) return;
            var versionsWithChangelog = await branchDetector.GetAvailableUpdateVersionsWithChangelogAsync();
            changelogs.Clear();
            var versions = new List<KeyValuePair<string, string>>();
            foreach (var v in versionsWithChangelog)
            {
                versions.Add(new KeyValuePair<string, string>(v.tagName, v.url));
                changelogs[v.tagName] = v.changelog;
            }
            cboUpdateVersion.DataSource = versions;
            cboUpdateVersion.DisplayMember = "Key";
            cboUpdateVersion.ValueMember = "Value";
            ShowChangelog(cboUpdateVersion.Text);
        }

        private async Task<EdenInstallationState> RefreshDetectedEdenInstallationStateAsync()
        {
            if (string.IsNullOrWhiteSpace(txtEdenLocation.Text))
                return EdenInstallationState.NoInstallDetected;

            string edenDirectoryPath = txtEdenLocation.Text;
            string latestVersionAvailable = cboUpdateVersion.Text;

            var stateDetector = new EdenInstallationStateDetector(edenDirectoryPath);
            var installationState = await stateDetector.GetEdenInstallationStateAsync(latestVersionAvailable);
            var currentInstalledVersion = await stateDetector.GetCurrentEdenInstalledVersion();

            switch (installationState)
            {
                case EdenInstallationState.LatestVersionInstalled:
                    btnProcess.Text = "Eden is currently Up-To-Date!";
                    toolTip1.SetToolTip(btnProcess, "The latest version of Eden is currently installed");
                    btnProcess.Enabled = false;
                    break;
                case EdenInstallationState.UpdateAvailable:
                    btnProcess.Text = $"Update from {currentInstalledVersion} to {latestVersionAvailable}";
                    toolTip1.SetToolTip(btnProcess, "Click to download the latest version of Eden");
                    btnProcess.Enabled = true;
                    break;
                case EdenInstallationState.NoInstallDetected:
                default:
                    btnProcess.Text = "New Install";
                    toolTip1.SetToolTip(btnProcess, "Eden not detected, click to download a fresh copy of Eden");
                    btnProcess.Enabled = true;
                    break;
            }
            return installationState;
        }

        private static async Task<string> ShowFolderBrowserDialogWindowAndGetResultAsync()
        {
            var folder = new TaskCompletionSource<string>();
            using var fbd = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true,
                Description = "Browse to the folder containing eden.exe"
            };

            Thread t = new(() =>
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    folder.SetResult(fbd.SelectedPath.Trim());
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            return await folder.Task;
        }

        private static bool IsControlDown()
        {
            return (ModifierKeys & Keys.Control) == Keys.Control;
        }

        private static void LaunchEden(string edenLocationPath)
        {
            string exePath = Path.Combine(edenLocationPath, "eden.exe");
            if (File.Exists(exePath))
            {
                Process.Start(new ProcessStartInfo(exePath)
                {
                    UseShellExecute = true
                })?.Dispose();
                Application.Exit();
            }
        }
    }
}
