﻿using System;
using System.IO;
using System.Linq;
using NLog.Targets;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Enums;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Command.Commands.Configuration
{
    [Command(CommandMode.Configuration, "interface", "Enter interface config mode")]
    public class InterfaceCommand : BaseCommand
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IVlanInterfaceRepository vlanInterfaceRepository;

        public InterfaceCommand(IShell shell, IInterfaceRepository interfaceRepository, IVlanInterfaceRepository vlanInterfaceRepository) : base(shell)
        {
            this.interfaceRepository = interfaceRepository;
            this.vlanInterfaceRepository = vlanInterfaceRepository;
        }

        public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
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

            var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");

            var @interface = interfaceRepository.Find(i => i.RevisionId == revisionId && i.Name == interfaceName && i.InterfaceType == interfaceType);
            if (@interface == null)
            {
                @interface = new Entities.Interface()
                {
                    Name = interfaceName,
                    InterfaceType = interfaceType,
                    RevisionId = revisionId,
                    Enabled = true
                };

                interfaceRepository.Create(@interface);
                interfaceRepository.SaveChanges();
            }

            if (@interface.InterfaceType == InterfaceType.Vlan)
            {
                var vlanInterface =
                    vlanInterfaceRepository.Find(v => v.RevisionId == revisionId && v.InterfaceName == interfaceName);
                if (vlanInterface == null)
                {

                }
            }

            shell.Environment["CONFIG_INTERFACE_NAME"] = @interface.Name;
            shell.Environment["CONFIG_INTERFACE_TYPE"] = @interface.InterfaceType;

            shell.SYS_SetCommandMode(CommandMode.Interface);
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
                .Where(i => i.Name.StartsWith(args[0])).Select(i=>i.Name).ToList();

            return $"{args[0]} {HelperFunctions.LCDString(interfaceNames)}";
        }

        public override void Help(string[] args, TextWriter output)
        {
            
        }
    }
}