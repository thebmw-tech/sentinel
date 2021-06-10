using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("parent-interface", "Sets the interface description")]
        public class SetParentInterfaceCommand : BaseCommand, IFilteredCommand
        {
            private readonly IInterfaceRepository interfaceRepository;
            public SetParentInterfaceCommand(IShell shell, IInterfaceRepository interfaceRepository) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }


                // TODO validate incoming value
                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var interfaceName = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");


                return 0;
            }

            public override string Suggest(string[] args)
            {
                if (args.Length > 0)
                {
                    if (string.IsNullOrEmpty(args[0]))
                    {
                        return " ";
                    }
                    return args[0];
                }

                return " ";
            }

            public bool ShouldShow(IShell shell)
            {
                var interfaceType = shell.GetEnvironment<InterfaceType>("CONFIG_INTERFACE_TYPE");
                return interfaceType == InterfaceType.Vlan;
            }
        }
    }
}
