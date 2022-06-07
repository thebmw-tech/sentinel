using System;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Factories;

namespace Sentinel.Core.Command.Commands.Configuration;

public partial class ShowCommand : ParentCommand<ShowCommand>
{
    [SubCommand("interface", "Show an interface value")]
    public class ShowInterfaceCommand : ScopedCommand<Interface.ShowCommand>
    {
        public ShowInterfaceCommand(IShell shell, EnvironmentSetupFactory environmentSetupFactory, IServiceProvider serviceProvider) : base(CommandMode.Interface, shell, environmentSetupFactory, serviceProvider)
        {
        }
    }
}