using System.Reflection;
using Sentinel.Core.Command.Attributes;

namespace Sentinel.Core.Command.Commands.Shell
{
    public partial class ShowCommand
    {
        [SubCommand("version", "Shows Version Information")]
        public void Version(string command)
        {
            var shellAsm = Assembly.GetAssembly(GetType());
            var coreAsm = Assembly.GetAssembly(typeof(SentinelDatabaseContext));

            shell.Out.WriteLine("Version Info");
            shell.Out.WriteLine($"ConsoleShell Version: {shellAsm.GetName().Version}");
            shell.Out.WriteLine($"Core Version: {coreAsm.GetName().Version}");
        }
    }
}