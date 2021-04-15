using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
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

            shell.Environment["INTERFACE"] = GetOrNewInterface(args[0]);

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
            return "interface";
        }
    }
}