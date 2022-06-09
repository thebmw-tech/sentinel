using System;
using System.Linq;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Environments
{
    public class InterfaceEnvironment : IEnvironmentSetup
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IVlanInterfaceRepository vlanInterfaceRepository;

        public InterfaceEnvironment(IInterfaceRepository interfaceRepository,
            IVlanInterfaceRepository vlanInterfaceRepository)
        {
            this.interfaceRepository = interfaceRepository;
            this.vlanInterfaceRepository = vlanInterfaceRepository;
        }

        public void Cleanup(IShell shell, string[] args)
        {
            var keysToDelete = shell.Environment.Keys.Where(s => s.StartsWith("CONFIG_INTERFACE"));
            foreach (var key in keysToDelete)
            {
                shell.Environment.Remove(key);
            }

            shell.SYS_SetCommandMode(CommandMode.Configuration);
        }

        public string GetPrompt(IShell shell, string hostname)
        {
            var revision = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);
            var i = shell.GetEnvironment<string>("CONFIG_INTERFACE_NAME");

            return $"{hostname}(config[r{revision:X}]-int[{i}])#";
        }

        public string[] Setup(IShell shell, string[] args)
        {
            if (args.Length < 2)
            {
                throw new Exception("Missing Interface Name");
            }

            var interfaceTypes = Enum.GetNames<InterfaceType>().Where(t => t.ToLower().StartsWith(args[0].ToLower())).ToList();

            if (interfaceTypes.Count != 1)
            {
                throw new Exception("Invalid interface type");
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
                    throw new Exception("Missing VLAN Configuration");
                }
            }

            shell.Environment["CONFIG_INTERFACE_NAME"] = @interface.Name;
            shell.Environment["CONFIG_INTERFACE_TYPE"] = @interface.InterfaceType;

            shell.SYS_SetCommandMode(CommandMode.Interface);

            return args.Skip(2).ToArray();
        }
    }
}