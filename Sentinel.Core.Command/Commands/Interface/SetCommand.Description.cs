using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("description", "Sets the interface description")]
        public class SetDescriptionCommand : BaseCommand
        {
            public SetDescriptionCommand(IShell shell) : base(shell)
            {

            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }

                // TODO validate incoming value
                shell.GetEnvironment<InterfaceDTO>("CONFIG_INTERFACE").Description = args[0];
                return 0;
            }

            public override string Suggest(string[] args)
            {
                if (args.Length > 0)
                {
                    return args[0];
                }

                return "";
            }
        }
    }
}
