using System.IO;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class DeleteCommand
    {
        [SubCommand("disable", "")]
        public class DeleteDisabledCommand : BaseCommand
        {
            private readonly IInterfaceRepository interfaceRepository;
            public DeleteDisabledCommand(IShell shell, IInterfaceRepository interfaceRepository) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length > 0)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }


                // TODO validate incoming value
                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var interfaceName = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");

                interfaceRepository.Modify(i => i.RevisionId == revisionId && i.Name == interfaceName,
                    i => { i.Enabled = true; });
                return 0;
            }

            public override string Suggest(string[] args)
            {
                return "";
            }
        }
    }
}