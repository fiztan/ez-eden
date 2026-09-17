using CommandLine;
using EzYuzu.Classes.CLOptions;
using EzYuzu.Classes.Updaters;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace EzYuzu
{
    internal static class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            if (args is null || args.Length <= 0)
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmMain(serviceProvider));
                return;
            }

            await ProcessCommandLineArgsAsync(serviceProvider, args);
        }

        private static IServiceProvider BuildServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHttpClient();
            services.AddHttpClient("Gitea-Api", client =>
            {
                client.BaseAddress = new Uri("https://git.eden-emu.dev/api/v1/");
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("user-agent", "EzEden");
            });
            return services.BuildServiceProvider();
        }

        private static async Task ProcessCommandLineArgsAsync(IServiceProvider serviceProvider, string[] args)
        {
            using var helpWriter = new StringWriter();
            var parser = new Parser(config => config.HelpWriter = helpWriter);

            var parserResults = parser.ParseArguments<CommandLineInstallOptions>(args);
            await parserResults.WithParsedAsync(async options =>
            {
                var updater = new EdenCommandLineUpdater(serviceProvider);
                await updater.ProcessEdenDirectory(options.EdenLocationPath, options.LaunchEdenAfterUpdate);
            });
            parserResults.WithNotParsed(options =>
            {
                if (options.IsVersion() || options.IsHelp())
                {
                    MessageBox.Show(helpWriter.ToString(), "EzEden", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }
    }
}
