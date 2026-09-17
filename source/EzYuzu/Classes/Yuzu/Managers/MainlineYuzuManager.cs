using EzYuzu.Classes.Extensions;
using System.IO.Compression;

namespace EzYuzu.Classes.Yuzu.Managers
{
    public sealed class EdenNightlyManager : EdenManager
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly string tagName;
        private readonly string downloadUrl;

        public EdenNightlyManager(IHttpClientFactory clientFactory, string tagName, string downloadUrl) : base(clientFactory)
        {
            this.clientFactory = clientFactory;
            this.tagName = tagName;
            this.downloadUrl = downloadUrl;
        }

        public new async Task ProcessEdenNewInstallationAsync()
        {
            await base.ProcessEdenNewInstallationAsync();
            await ProcessEdenUpdateAsync();
        }

        public async Task ProcessEdenUpdateAsync()
        {
            string zipPath = Path.Combine(TempUpdateDirectoryPath, "eden.zip");
            var client = clientFactory.CreateClient();

            await using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                var progressReporter = new Progress<float>(progress =>
                {
                    var progressPercentage = (int)(progress * 100);
                    RaiseUpdateProgress(progressPercentage, $"Downloading Eden Nightly {tagName} ...");
                });
                await client.DownloadAsync(downloadUrl, file, progressReporter);
            }

            RaiseUpdateProgress(0, $@"Extracting Eden Nightly {tagName} ...");
            ZipFile.ExtractToDirectory(zipPath, EdenDirectoryPath, overwriteFiles: true);

            var versionFilePath = Path.Combine(EdenDirectoryPath, "version");
            await File.WriteAllTextAsync(versionFilePath, tagName);
            RaiseUpdateProgress(100, $@"Extracting Eden Nightly {tagName} ...");

            RaiseUpdateProgress(0, "Cleaning up ...");
            CleanUpDirectories();
            RaiseUpdateProgress(100, "Cleaning up ...");
        }
    }
}
