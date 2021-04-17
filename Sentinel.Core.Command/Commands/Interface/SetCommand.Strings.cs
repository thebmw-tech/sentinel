using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("description", "Sets the interface name")]
        public int SetDescription(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            if (args.Length != 1)
            {
                error.WriteLine("Wrong number of arguments");
                return 1;
            }

            // TODO validate incoming value
            GetInterface().Description = args[0];
            return 0;
        }
    }
}
