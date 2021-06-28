using System.IO;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;

namespace Sentinel.Core.Command.Commands
{
    public abstract class ParentCommand<TCommand> : BaseCommand
    {
        protected readonly SubCommandInterpreter<TCommand> subCommandInterpreter;

        public ParentCommand(IShell shell, SubCommandInterpreter<TCommand> subCommandInterpreter) : base(shell)
        {
            this.subCommandInterpreter = subCommandInterpreter;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            return subCommandInterpreter.Execute(shell, args, input, output, error);
        }

        public override string Suggest(string[] args)
        {
            return subCommandInterpreter.Suggest(shell, args);
        }

        public override void Help(string[] args, TextWriter output)
        {
            subCommandInterpreter.Help(shell, args);
        }
    }
}