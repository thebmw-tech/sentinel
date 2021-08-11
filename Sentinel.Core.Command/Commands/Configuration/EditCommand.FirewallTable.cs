using System;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using System.IO;
using System.Linq;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Enums;
using Sentinel.Core.Environments;
using Sentinel.Core.Factories;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Configuration
{
    public partial class EditCommand : ParentCommand<EditCommand>
    {
        [SubCommand("firewall", "Enter firewall config mode")]
        public class EditFirewallTableCommand : BaseCommand
        {
            private readonly IInterfaceRepository interfaceRepository;
            private readonly FirewallTableEnvironment firewallTableEnvironment;

            public EditFirewallTableCommand(IShell shell, IInterfaceRepository interfaceRepository, FirewallTableEnvironment firewallTableEnvironment) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
                this.firewallTableEnvironment = firewallTableEnvironment;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                firewallTableEnvironment.Setup(shell, args);
                return 0;
            }

            public override string Suggest(string[] args)
            {
                if (args.Length == 0)
                {
                    return "";
                }

                if (args.Length == 1)
                {
                    var interfaceTypes = Enum.GetNames<InterfaceType>().Select(t => t.ToLower()).Where(t => t.StartsWith(args[0].ToLower())).ToList();
                    return HelperFunctions.LCDString(interfaceTypes);
                }

                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");

                var interfaceNames = interfaceRepository.GetForRevision(revisionId)
                    .Where(i => i.Name.StartsWith(args[0])).Select(i => i.Name).ToList();

                return $"{args[0]} {HelperFunctions.LCDString(interfaceNames)}";
            }

            public override void Help(string[] args, TextWriter output)
            {

            }
        }
    }
}