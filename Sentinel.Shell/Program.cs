﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;
using Sentry;

namespace Sentinel.Shell
{
    class Program
    {
        private const string SENTRY_DSN = "https://70d83df83cd940cfa93bf0cc7ecc723f@sentry.thebmw.tech/5";
        static void Main(string[] args)
        {
#if DEBUG
            var environmentName = "DEBUG";
#else
             var environmentName = "production";
#endif
            using var _ = SentrySdk.Init(options =>
            {
                options.Dsn = SENTRY_DSN;
                options.Environment = environmentName;
            });

            Console.WriteLine("Sentinel ConsoleShell v0.1");
            Console.WriteLine("by thebmw");
            Console.WriteLine();
            

            Console.Write("Loading...");
            var services = new ServiceCollection()
                .RegisterSentinelCore()
                .RegisterSentinelCoreCommand()
                .AddTransient<ConsoleShell>()
                .BuildServiceProvider();
            Console.WriteLine(" Done!");

            
            Console.Write("Setup...");
            HelperFunctions.VerifyFirstRun(services);

            var reviceService = services.GetService<IRevisionService>();
            reviceService.CleanupOldLocks();

            var shell = services.GetService<ConsoleShell>();

            Console.WriteLine(" Done!");

            shell.ShellLoop();
        }
    }
}
