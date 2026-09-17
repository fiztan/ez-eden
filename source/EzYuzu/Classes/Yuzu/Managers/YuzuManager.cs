using EzYuzu.Classes.Extensions;
using System.Diagnostics;

namespace EzYuzu.Classes.Yuzu.Managers
{
    public class EdenManager
    {
        private readonly IHttpClientFactory clientFactory;

        protected EdenManager(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        protected internal Action<int, string>? UpdateProgress;

        protected void RaiseUpdateProgress(int progressPercentage, string progressText)
        {
            UpdateProgress?.Invoke(progressPercentage, progressText);
        }

        protected internal string EdenDirectoryPath { get; set; } = "";

        protected internal string TempUpdateDirectoryPath { get; set; } = "";

        protected internal async Task DownloadPrerequisitesAsync()
        {
            CloseEden();
            PrepareTempUpdateFolder();
        }

        protected async Task ProcessEdenNewInstallationAsync()
        {
            Directory.CreateDirectory(Path.Combine(EdenDirectoryPath, "user", "keys"));
            Directory.CreateDirectory(Path.Combine(EdenDirectoryPath, "user", "config"));
        }

        protected void CleanUpDirectories()
        {
            if (Directory.Exists(TempUpdateDirectoryPath))
                Directory.Delete(TempUpdateDirectoryPath, true);
        }

        protected static void CopyFiles(string fromFolder, string toFolder, bool overwrite = false)
        {
            Directory
                .EnumerateFiles(fromFolder, "*.*", SearchOption.AllDirectories)
                .Where(file => (File.GetAttributes(file) & (FileAttributes.Hidden | FileAttributes.System)) == 0)
                .AsParallel()
                .ForAll(from =>
                {
                    var to = from.Replace(fromFolder, toFolder);
                    var toSubFolder = Path.GetDirectoryName(to);
                    if (!string.IsNullOrWhiteSpace(toSubFolder))
                    {
                        Directory.CreateDirectory(toSubFolder);
                    }
                    File.Copy(from, to, overwrite);
                });
        }

        private static void CloseEden()
        {
            string[] processNames = { "eden" };
            foreach (var name in processNames)
            {
                var procs = Process.GetProcessesByName(name);
                if (procs is null)
                    continue;

                foreach (var proc in procs)
                {
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                    proc.Dispose();
                }
            }
        }

        private void PrepareTempUpdateFolder()
        {
            Directory.CreateDirectory(TempUpdateDirectoryPath);
        }
    }
}
