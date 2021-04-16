using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("name", "Sets the interface name")]
        public void SetName(string[] args)
        {

        }
    }
}
