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
        [SubCommand("firewall", "set an firewall table value")]
        public class ScopedFirewallTableCommand : ScopedCommand<Firewall.Table.SetCommand>
        {
            public ScopedFirewallTableCommand(IShell shell, IServiceProvider serviceProvider,
                EnvironmentSetupFactory environmentSetupFactory) : base(CommandMode.FirewallTable, shell,
                environmentSetupFactory, serviceProvider)
            {
            }
        }
    }
}