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
        [SubCommand("interface", "Enter interface config mode")]
        public class EditInterfaceCommand : BaseCommand
        {
            private readonly IInterfaceRepository interfaceRepository;
            private readonly InterfaceEnvironment interfaceEnvironment;

            public EditInterfaceCommand(IShell shell, IInterfaceRepository interfaceRepository, InterfaceEnvironment interfaceEnvironment) : base(shell)
            {
                this.interfaceRepository = interfaceRepository;
                this.interfaceEnvironment = interfaceEnvironment;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                interfaceEnvironment.Setup(shell, args);
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

                var revisionId = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);

                var interfaceNames = interfaceRepository.GetForRevision(revisionId)
                    .Where(i => i.Name.StartsWith(args[0])).Select(i => i.Name).ToList();

                return $"{args[0]} {HelperFunctions.LCDString(interfaceNames)}";
            }

            public override void Help(string[] args, TextWriter output)
            {
                if (args.Length == 0)
                {
                    foreach (var interfaceTypeName in Enum.GetNames<InterfaceType>())
                    {
                        output.WriteLine(interfaceTypeName);
                    }

                    return;
                }

                var revisionId = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);

                if (args.Length == 1)
                {
                    var interfaceTypes = Enum.GetNames<InterfaceType>().Select(t => t.ToLower()).Where(t => t.StartsWith(args[0].ToLower())).ToList();
                    switch (interfaceTypes.Count)
                    {
                        case > 1:
                            foreach (var interfaceTypeName in interfaceTypes)
                            {
                                output.WriteLine(interfaceTypeName);
                            }
                            break;
                        case 0:
                            output.WriteLine("\a");
                            break;
                        case 1:
                            var interfaceType = Enum
                                .GetValues<InterfaceType>().First(i => i.ToString().ToLower() == interfaceTypes.First());
                            var interfaceNames = interfaceRepository.GetForRevision(revisionId).Where(i => i.InterfaceType == interfaceType).Select(i => i.Name).ToList();

                            foreach (var interfaceName in interfaceNames)
                            {
                                output.WriteLine(interfaceName);
                            }
                            break;
                    }

                    return;
                }

                
            }
        }
    }
}