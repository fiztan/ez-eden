namespace EzYuzu.Classes.Settings
{
    public sealed class EzEdenSettings
    {
        public string EdenLocation { get; set; } = "";

        public bool LaunchEdenAfterUpdate { get; set; } = false;

        public bool ExitEdenAfterUpdate { get; set; } = false;

        public bool AutoUpdateEdenOnEzEdenLaunch { get; set; } = false;
    }
}
