using CommandLine;

namespace EzYuzu.Classes.CLOptions
{
    public sealed class CommandLineInstallOptions
    {
        [Option('p', "path", Default = "", Required = true, HelpText = "Set Eden location directory path for new install/update.")]
        public string? EdenLocationPath { get; set; }

        [Option('l', "launch-eden", Default = false, HelpText = "Launch Eden after new install/update.")]
        public bool LaunchEdenAfterUpdate { get; set; }
    }
}
