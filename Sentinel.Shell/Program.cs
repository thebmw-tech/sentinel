using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Models;
using Sentinel.Shell.Services;

namespace Sentinel.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sentinel Shell v0.1");
            Console.WriteLine("by thebmw");
            Console.WriteLine();

            Console.Write("Loading...");
            var services = new ServiceCollection()
                .UseSentinelDi()
                .AddTransient<CommandInterpreter>()
                .AddTransient(typeof(SubCommandInterpreter<>))
                .AddTransient<Services.Shell>()
                .BuildServiceProvider();


            var shell = services.GetService<Services.Shell>();

            var getPrompt = new Func<string>(() =>
            {
                //var systemConfigurationRepository = services.GetService<ISystemConfigurationRepository>();
                var prompt = "hostname> ";

                return prompt;
            });

            Console.WriteLine(" Done!");

            shell.ShellLoop(CommandMode.Shell, new ShellContext(), getPrompt);
        }
    }
}
