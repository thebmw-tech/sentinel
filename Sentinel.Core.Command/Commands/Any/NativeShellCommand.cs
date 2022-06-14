using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Commands.Any
{
    [Command(CommandMode.Any, "execute", "Executes a native shell command")]
    public class NativeShellCommand : BaseCommand
    {
        private readonly ICommandExecutionHelper commandExecutionHelper;
        public NativeShellCommand(IShell shell, ICommandExecutionHelper commandExecutionHelper) : base(shell)
        {
            this.commandExecutionHelper = commandExecutionHelper;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            var cmd = args[0];
            var cmdArgs = string.Join(' ', args.Skip(1));
            var process = commandExecutionHelper.BuildProcess(cmd, cmdArgs, true, true);

            process.ErrorDataReceived += (sender, eventArgs) => error.WriteLine(eventArgs.Data);
            process.OutputDataReceived += (sender, eventArgs) => output.WriteLine(eventArgs.Data);

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            return process.ExitCode;
        }

        public override string Suggest(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}