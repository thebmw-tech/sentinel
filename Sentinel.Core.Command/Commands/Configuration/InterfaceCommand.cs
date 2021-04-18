using System.IO;
using System.Linq;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Helpers;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Configuration
{
    [Command(CommandMode.Configuration, "interface", "Enter interface config mode")]
    public class InterfaceCommand : BaseCommand
    {
        private readonly IInterfaceService interfaceService;

        public InterfaceCommand(IShell shell, IInterfaceService interfaceService) : base(shell)
        {
            this.interfaceService = interfaceService;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            if (args.Length != 1)
            {
                error.WriteLine("Missing interface name");
                return 1;
            }

            shell.Environment["CONFIG_INTERFACE"] = GetOrNewInterface(args[0]);

            shell.SYS_SetCommandMode(CommandMode.Interface);
            return 0;
        }

        private InterfaceDTO GetOrNewInterface(string name)
        {
            var revisionId = (int)shell.Environment["CONFIG_REVISION_ID"];
            var @interface = interfaceService.GetInterfaceWithName(revisionId, name);

            if (@interface == null)
            {
                @interface = new InterfaceDTO()
                {
                    Name = name,
                };
            }

            return @interface;
        }

        public override string Suggest(string[] args)
        {
            if (args.Length == 0)
            {
                return "";
            }

            var interfaceNames = interfaceService.GetInterfacesInRevision(shell.GetEnvironment<int>("CONFIG_REVISION_ID"))
                .Where(i => i.Name.StartsWith(args[0])).Select(i=>i.Name).ToList();

            return HelperFunctions.LCDString(interfaceNames);
        }

        public override void Help(string[] args, TextWriter output)
        {
            var interfaces = interfaceService.GetInterfacesInRevision(shell.GetEnvironment<int>("CONFIG_REVISION_ID"));

            if (args.Length > 0)
            {
                interfaces = interfaces.Where(i => i.Name.StartsWith(args[0])).ToList();
            }

            foreach (var @interface in interfaces)
            {
                output.WriteLine($" {@interface.Name}");
            }
        }
    }
}