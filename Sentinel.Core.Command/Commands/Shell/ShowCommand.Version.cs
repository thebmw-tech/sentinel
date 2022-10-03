using System;
using System.IO;
using System.Reflection;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    public partial class ShowCommand
    {
        [SubCommand("version", "Shows the version information")]
        public class ShowVerisonCommand : BaseCommand
        {
            public ShowVerisonCommand(IShell shell) : base(shell)
            {
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
#if DEBUG
                var releaseType = "DEBUG";
#else
                var releaseType = "RELEASE";
#endif
                var sentinelVersion = Assembly.GetCallingAssembly().GetName().Version;
                output.WriteLine($"Sentinel Version: v{sentinelVersion}-{releaseType}");
                output.WriteLine($"dotnet Version: {Environment.Version}");
                output.WriteLine($"OS Version: {Environment.OSVersion}");
                return 0;
            }

            public override string Suggest(string[] args)
            {
                throw new NotImplementedException();
            }
        }
    }
}