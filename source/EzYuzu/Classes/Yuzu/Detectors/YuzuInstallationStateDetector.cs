namespace EzYuzu.Classes.Yuzu.Detectors
{
    public sealed class EdenInstallationStateDetector
    {
        private readonly string edenDirectoryPath;
        private readonly string versionFilePath;

        public EdenInstallationStateDetector(string edenDirectoryPath)
        {
            this.edenDirectoryPath = edenDirectoryPath;
            this.versionFilePath = Path.Combine(edenDirectoryPath, "version");
        }

        public enum EdenInstallationState
        {
            LatestVersionInstalled,
            UpdateAvailable,
            NoInstallDetected
        }

        public async Task<EdenInstallationState> GetEdenInstallationStateAsync(string latestVersionAvailable)
        {
            if (!EdenExists())
                return EdenInstallationState.NoInstallDetected;

            if (!File.Exists(versionFilePath))
                return EdenInstallationState.UpdateAvailable;

            string currentVersion = await GetCurrentEdenInstalledVersion();
            return currentVersion == latestVersionAvailable
                ? EdenInstallationState.LatestVersionInstalled
                : EdenInstallationState.UpdateAvailable;
        }

        public async Task<string> GetCurrentEdenInstalledVersion()
        {
            if (!File.Exists(versionFilePath))
                return "";

            string? line;
            using var reader = new StreamReader(versionFilePath);
            while ((line = await reader.ReadLineAsync()) is not null)
            {
                return line.Trim();
            }

            return "";
        }

        private bool EdenExists()
        {
            return File.Exists(Path.Combine(edenDirectoryPath, "eden.exe"));
        }
    }
}
