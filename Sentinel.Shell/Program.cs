using System;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentry;

namespace Sentinel.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            using var _ = SentrySdk.Init("https://70d83df83cd940cfa93bf0cc7ecc723f@sentry.thebmw.tech/5");

            Console.WriteLine("Sentinel ConsoleShell v0.1");
            Console.WriteLine("by thebmw");
            Console.WriteLine();
            

            Console.Write("Loading...");
            var services = new ServiceCollection()
                .RegisterSentinelCore()
                .RegisterSentinelCoreCommand()
                .AddTransient<ConsoleShell>()
                .BuildServiceProvider();

            HelperFunctions.VerifyFirstRun(services);

            var shell = services.GetService<ConsoleShell>();

            var getPrompt = new Func<CommandMode, string>((mode) =>
            {
                var systemConfigurationRepository = services.GetService<ISystemConfigurationRepository>();
                var configuration = systemConfigurationRepository.GetCurrent();

                switch (mode)
                {
                    case CommandMode.Shell:
                        return $"{configuration.Hostname}> ";
                    case CommandMode.Configuration:
                        return $"{configuration.Hostname}(config)# ";
                    default:
                        return "";
                }
            });

            Console.WriteLine(" Done!");

            shell.ShellLoop(getPrompt);
        }
    }
}
