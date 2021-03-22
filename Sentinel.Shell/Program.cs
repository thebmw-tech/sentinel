using System;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core;

namespace Sentinel.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .UseSentinelDi()
                .AddTransient<CommandInterpreter>()
                .AddTransient<Shell>()
                .BuildServiceProvider();


            var shell = services.GetService<Shell>();

            shell.ShellLoop();
        }
    }
}
