using System;
using System.Reflection;
using Sentinel.Core;
using Sentinel.Shell.Attributes;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Commands.Shell
{
    public partial class ShowCommand
    {
        [SubCommand("version", "Shows Version Information")]
        public void Version(ShellContext context, string command)
        {
            var shellAsm = Assembly.GetAssembly(GetType());
            var coreAsm = Assembly.GetAssembly(typeof(SentinelDatabaseContext));

            Console.WriteLine("Version Info");
            Console.WriteLine($"Shell Version: {shellAsm.GetName().Version}");
            Console.WriteLine($"Core Version: {coreAsm.GetName().Version}");
        }
    }
}