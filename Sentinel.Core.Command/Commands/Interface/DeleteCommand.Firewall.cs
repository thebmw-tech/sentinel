using System.IO;
using System.Linq;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Interface
{
    public partial class DeleteCommand
    {
        [SubCommand("firewall", "")]
        public class DeleteFirewallCommand : BaseCommand
        {
            private readonly IInterfaceRepository interfaceRepository;

            private readonly string[] firewallTypes = new string[] { "in", "out", "local" };

            public DeleteFirewallCommand(IShell shell, IInterfaceRepository interfaceRepository) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
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

                interfaceRepository.Modify(i => i.RevisionId == revisionId && i.Name == interfaceName,
                    i =>
                    {
                        switch (args[0])
                        {
                            case "out":
                                i.OutboundFirewallTableId = null;
                                break;
                            case "in":
                                i.InboundFirewallTableId = null;
                                break;
                            case "local":
                                i.LocalFirewallTableId = null;
                                break;
                        }
                    });
                return 0;
            }

            public override string Suggest(string[] args)
            {
                return "";
            }
        }
    }
}