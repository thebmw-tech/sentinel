using System;
using System.Collections.Generic;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using System.Linq;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Enums;
using Sentinel.Core.Factories;
using Sentinel.Core.Repository;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Configuration
{
    public partial class SetCommand : ParentCommand<SetCommand>
    {
        [SubCommand("interface", "set an interface value")]
        public class ScopedInterfaceCommand : ScopedCommand<Interface.SetCommand>
        {
            public ScopedInterfaceCommand(IShell shell, IServiceProvider serviceProvider,
                EnvironmentSetupFactory environmentSetupFactory) : base(CommandMode.Interface, shell,
                environmentSetupFactory, serviceProvider)
            {
            }
        }
    }
}