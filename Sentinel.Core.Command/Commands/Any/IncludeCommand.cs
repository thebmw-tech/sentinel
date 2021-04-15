using System.IO;
using System.Text.RegularExpressions;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Commands;

namespace Sentinel.Core.Command.Commands.Any
{
    [Command(CommandMode.Any, "include", "Help")]
    public class IncludeCommand : BaseCommand
    {
        public IncludeCommand(IShell shell) : base(shell)
        {

        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            if (args.Length != 1)
            {
                error.WriteLine("include: Missing Arguments");
                return 1;
            }

            var regex = new Regex(args[0], RegexOptions.IgnoreCase);

            string line;
            while ((line = input.ReadLine()) != null)
            {
                if (regex.IsMatch(line))
                {
                    output.WriteLine(line);
                }
            }
            return 0;
        }

        public override string Suggest(string[] args)
        {
            return "include";
        }
    }
}