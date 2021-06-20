using System;
using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Shell
{
    [Command(CommandMode.Shell, "show", "Shows Configuration")]
    public class ShowCommand : BaseCommand
    {
        private readonly IInterfaceService interfaceService;
        private readonly IRevisionService revisionService;
        private readonly SubCommandInterpreter<ShowCommand> subCommandInterpreter;

        public ShowCommand(IShell shell, SubCommandInterpreter<ShowCommand> subCommandInterpreter,
            IRevisionService revisionService, IInterfaceService interfaceService) : base(shell)
        {
            this.subCommandInterpreter = subCommandInterpreter;
            this.revisionService = revisionService;
            this.interfaceService = interfaceService;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            return subCommandInterpreter.Execute(shell, args, input, output, error);
        }

        public override string Suggest(string[] args)
        {
                throw new NotImplementedException();
        }

        //[SubCommand("interfaces", "Show information about interfaces")]
        //public int ShowInterfaces(string[] args, TextReader input, TextWriter output, TextWriter error)
        //{
        //    var safeRevision = revisionService.GetSafe();
        //    var interfaces = interfaceService.GetInterfacesInRevision(safeRevision.Id);
        //    foreach (var @interface in interfaces)
        //    {
        //        interfaceService.PrintInterfaceToTextWriter(safeRevision.Id, @interface, output);
        //    }

        //    return 0;
        //}

        public string ShowInterfacesSuggestions(string[] args, TextWriter error)
        {
            throw new NotImplementedException();
        }
    }
}