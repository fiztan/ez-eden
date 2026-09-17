namespace EzYuzu.Classes.Updaters
{
    public sealed class AppUpdater
    {
        public enum CurrentVersion
        {
            LatestVersion,
            Undetectable
        }

        public async Task<CurrentVersion> CheckVersionAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync("https://git.eden-emu.dev/eden-ci/nightly/releases");
                return response.IsSuccessStatusCode
                    ? CurrentVersion.LatestVersion
                    : CurrentVersion.Undetectable;
            }
            catch
            {
                return CurrentVersion.Undetectable;
            }
        }
    }
}
