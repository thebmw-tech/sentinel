using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class SetCommand
    {
        [SubCommand("firewall", "Sets the interface firewall")]
        public class SetFirewallCommand : BaseCommand
        {
            private readonly IInterfaceRepository interfaceRepository;
            private readonly IFirewallTableRepository firewallTableRepository;

            private readonly string[] firewallTypes = new string[] {"in", "out", "local"};
            public SetFirewallCommand(IShell shell, IInterfaceRepository interfaceRepository, IFirewallTableRepository firewallTableRepository) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
                this.firewallTableRepository = firewallTableRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 2)
                {
                    error.WriteLine("Wrong number of arguments");
                    return 1;
                }

                if (!firewallTypes.Contains(args[0]))
                {
                    error.WriteLine("Only in, out, and local are valid firewall types");
                    return 1;
                }
                
                // TODO validate incoming value
                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var interfaceName = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");

                var firewallTable = firewallTableRepository.Find(t => t.RevisionId == revisionId && t.Name == args[1]);
                if (firewallTable == null)
                {
                    error.WriteLine("No firewall table with name \"{args[1]}\" found.");
                    return 1;
                }

                interfaceRepository.Modify(i => i.RevisionId == revisionId && i.Name == interfaceName,
                    i =>
                    {
                        switch (args[0])
                        {
                            case "out":
                                i.OutboundFirewallTableId = firewallTable.Id;
                                break;
                            case "in":
                                i.InboundFirewallTableId = firewallTable.Id;
                                break;
                            case "local":
                                i.LocalFirewallTableId = firewallTable.Id;
                                break;
                        }
                    });

                return 0;
            }

            public override string Suggest(string[] args)
            {
                if (args.Length == 1)
                {
                    var type = args[0];
                    var fullType = firewallTypes.FirstOrDefault(t => t.StartsWith(type));
                    return fullType ?? " ";
                }

                if (args.Length == 2)
                {
                    var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                    var firewallTableNames = firewallTableRepository.Filter(t => t.RevisionId == revisionId && t.Name.StartsWith(args[1])).Select(t => t.Name).ToList();
                    return $"{args[0]} {HelperFunctions.LCDString(firewallTableNames)}";
                }

                return " ";
            }

            
        }
    }
}
