using System;
using System.IO;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Factories;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Commands
{
    public abstract class ScopedCommand<TCommand> : BaseCommand where TCommand : BaseCommand
    {
        private readonly IEnvironmentSetup environmentSetup;
        private readonly ICommand command;

        public ScopedCommand(CommandMode commandMode, IShell shell, EnvironmentSetupFactory environmentSetupFactory, IServiceProvider serviceProvider) : base(shell)
        {
            environmentSetup = environmentSetupFactory.Build(commandMode);
            command = CommandHelper.GetCommandInstance(typeof(TCommand), shell, serviceProvider);
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            try
            {
                var commandArgs = environmentSetup.Setup(shell, args);
                return command.Main(commandArgs, input, output, error);
            }
            finally
            {
                environmentSetup.Cleanup(shell, Array.Empty<string>());
            }
        }

        public override string Suggest(string[] args)
        {
            try
            {
                var commandArgs = environmentSetup.Setup(shell, args);
                return command.Suggest(commandArgs);
            }
            finally
            {
                environmentSetup.Cleanup(shell, Array.Empty<string>());
            }
        }

        public override void Help(string[] args, TextWriter output)
        {
            try
            {
                var commandArgs = environmentSetup.Setup(shell, args);
                command.Help(commandArgs, output);
            }
            finally
            {
                environmentSetup.Cleanup(shell, Array.Empty<string>());
            }
        }
    }
}