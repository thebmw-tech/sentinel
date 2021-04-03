using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sentinel ConsoleShell v0.1");
            Console.WriteLine("by thebmw");
            Console.WriteLine();
            

            Console.Write("Loading...");
            var services = new ServiceCollection()
                .UseSentinelDi()
                .UseSentinelShellDi()
                .AddTransient<ConsoleShell>()
                .BuildServiceProvider();


            var shell = services.GetService<ConsoleShell>();

            var getPrompt = new Func<CommandMode, string>((mode) =>
            {
                //var systemConfigurationRepository = services.GetService<ISystemConfigurationRepository>();
                switch (mode)
                {
                    case CommandMode.Shell:
                        return "hostname> ";
                    case CommandMode.Configuration:
                        return "hostname(config)# ";
                    default:
                        return "";
                }
            });

            Console.WriteLine(" Done!");

            shell.ShellLoop(getPrompt);
        }
    }
}
