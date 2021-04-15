using System.IO;
using System.Threading.Tasks;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected readonly IShell shell;

        public BaseCommand(IShell shell)
        {
            this.shell = shell;
        }

        public abstract int Main(string[] args, TextReader input, TextWriter output, TextWriter error);

        public abstract string Suggest(string[] args);
    }
}