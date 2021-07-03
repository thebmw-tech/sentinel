using System;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using System.IO;
using System.Linq;
using Sentinel.Core.Enums;
using Sentinel.Core.Factories;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Command.Commands.Configuration
{
    public partial class DeleteCommand : ParentCommand<DeleteCommand>
    {
        [SubCommand("interface", "Delete an interface")]
        public class DeleteInterfaceCommand : ScopedCommand<Interface.DeleteCommand>
        {
            private readonly IInterfaceRepository interfaceRepository;
            private readonly IInterfaceService interfaceService;
            public DeleteInterfaceCommand(IShell shell, IInterfaceService interfaceService, IInterfaceRepository interfaceRepository, EnvironmentSetupFactory environmentSetupFactory, IServiceProvider serviceProvider) : base(CommandMode.Interface, shell, environmentSetupFactory, serviceProvider)
            {
                this.interfaceService = interfaceService;
                this.interfaceRepository = interfaceRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length <= 2)
                {
                    return DeleteInterface(args, input, output, error);
                }

                return base.Main(args, input, output, error);
            }

            private int DeleteInterface(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 2)
                {
                    error.WriteLine("Missing interface name");
                    return 1;
                }

                var interfaceTypes = Enum.GetNames<InterfaceType>().Where(t => t.ToLower().StartsWith(args[0].ToLower())).ToList();

                if (interfaceTypes.Count != 1)
                {
                    error.WriteLine("invalid interface type");
                    return 1;
                }

                var interfaceType = Enum.Parse<InterfaceType>(interfaceTypes.First());

                var interfaceName = args[1];

                var revisionId = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);

                var @interface = interfaceRepository.Find(i => i.RevisionId == revisionId && i.Name == interfaceName && i.InterfaceType == interfaceType);

                if (@interface == null)
                {
                    error.WriteLine("Can't delete an interface that doesn't exist.");
                    return 1;
                }

                interfaceService.RemoveInterface(revisionId, interfaceName);
                output.WriteLine($"Removed \"{interfaceName}\"");

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

                if (args.Length == 2)
                {
                    var revisionId = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);

                    var interfaceNames = interfaceRepository.GetForRevision(revisionId)
                        .Where(i => i.Name.StartsWith(args[0])).Select(i => i.Name).ToList();

                    return $"{args[0]} {HelperFunctions.LCDString(interfaceNames)}";
                }

                return base.Suggest(args);
            }
        }
    }
}