using System;
using System.IO;
using Newtonsoft.Json;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "show", "Shows information about the current interface.")]
    public class ShowCommand : BaseCommand
    {
        private readonly IInterfaceService interfaceService;

        public ShowCommand(IShell shell, IInterfaceService interfaceService) : base(shell)
        {
            this.interfaceService = interfaceService;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            var revisionId = (int)shell.Environment["CONFIG_REVISION_ID"];
            var interfaceName = (string)shell.Environment["CONFIG_INTERFACE_NAME"];

            var @interface = interfaceService.GetInterfaceWithName(revisionId, interfaceName);

            interfaceService.PrintInterfaceToTextWriter(revisionId, @interface, output);
            
            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}