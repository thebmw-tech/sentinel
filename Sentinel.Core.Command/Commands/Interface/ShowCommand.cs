using System.IO;
using Newtonsoft.Json;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    [Command(CommandMode.Interface, "show", "Shows information about the current interface.")]
    public class ShowCommand : BaseCommand
    {
        public ShowCommand(IShell shell) : base(shell)
        {
            
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            var @interface = (InterfaceDTO)shell.Environment["CONFIG_INTERFACE"];
            var intJson = JsonConvert.SerializeObject(@interface, Formatting.Indented);
            output.WriteLine(intJson);
            return 0;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}