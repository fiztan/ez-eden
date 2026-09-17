using EzYuzu.Classes.Yuzu.Detectors;
using EzYuzu.Classes.Yuzu.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace EzYuzu.Classes.CLOptions
{
    public sealed class EdenCommandLineUpdater
    {
        private readonly IHttpClientFactory? clientFactory;

        public EdenCommandLineUpdater(IServiceProvider serviceProvider)
        {
            clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }

        public async Task ProcessEdenDirectory(string? edenLocationPath, bool launchEden)
        {
            if (edenLocationPath is null)
                return;

            if (edenLocationPath.EndsWith("eden.exe", StringComparison.OrdinalIgnoreCase))
                edenLocationPath = Path.GetDirectoryName(edenLocationPath);

            var branchDetector = new EdenBranchDetector(clientFactory!)
            {
                EdenDirectoryPath = edenLocationPath!
            };

            var availableVersions = await branchDetector.GetAvailableUpdateVersionsAsync();
            var latestVersion = availableVersions.First();

            var stateDetector = new EdenInstallationStateDetector(edenLocationPath!);
            var installationState = await stateDetector.GetEdenInstallationStateAsync(latestVersion.Key);

            if (installationState == EdenInstallationStateDetector.EdenInstallationState.LatestVersionInstalled && launchEden)
            {
                Process.Start(new ProcessStartInfo(Path.Combine(edenLocationPath!, "eden.exe"))
                {
                    UseShellExecute = true
                })?.Dispose();
                return;
            }

            var edenManager = new EdenNightlyManager(clientFactory!, latestVersion.Key, latestVersion.Value);
            edenManager.EdenDirectoryPath = edenLocationPath!;
            edenManager.TempUpdateDirectoryPath = Path.Combine(edenLocationPath!, "TempUpdate");
            await edenManager.DownloadPrerequisitesAsync();

            if (installationState == EdenInstallationStateDetector.EdenInstallationState.NoInstallDetected)
                await edenManager.ProcessEdenNewInstallationAsync();
            else if (installationState == EdenInstallationStateDetector.EdenInstallationState.UpdateAvailable)
                await edenManager.ProcessEdenUpdateAsync();

            if (launchEden)
            {
                Process.Start(new ProcessStartInfo(Path.Combine(edenLocationPath!, "eden.exe"))
                {
                    UseShellExecute = true
                })?.Dispose();
            }

            Directory.Delete(edenManager.TempUpdateDirectoryPath, true);
        }
    }
}
