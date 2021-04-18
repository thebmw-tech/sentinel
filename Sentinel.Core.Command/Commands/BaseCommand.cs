using System;
using System.IO;
using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Command.Commands
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

        public virtual void Help(string[] args, TextWriter output)
        {
            //throw new NotImplementedException();
        }
    }
}