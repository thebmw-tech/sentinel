using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Services.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;
using System.IO;
using System.Linq;
using Sentinel.Core.Enums;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Services
{
    public class InterfaceService : IInterfaceService
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IFirewallTableRepository firewallTableRepository;
        private readonly IInterfaceAddressRepository interfaceAddressRepository;
        private readonly IVlanInterfaceRepository vlanInterfaceRepository;
        private readonly ISourceNatRuleRepository sourceNatRuleRepository;
        private readonly IDestinationNatRuleRepository destinationNatRuleRepository;
        private readonly IRouteRepository routeRepository;

        private readonly IMapper mapper;

        public InterfaceService(IInterfaceRepository interfaceRepository, IFirewallTableRepository firewallTableRepository,
            IInterfaceAddressRepository interfaceAddressRepository, IVlanInterfaceRepository vlanInterfaceRepository,
            ISourceNatRuleRepository sourceNatRuleRepository, IDestinationNatRuleRepository destinationNatRuleRepository,
            IRouteRepository routeRepository, IMapper mapper)
        {
            this.interfaceRepository = interfaceRepository;
            this.firewallTableRepository = firewallTableRepository;
            this.interfaceAddressRepository = interfaceAddressRepository;
            this.vlanInterfaceRepository = vlanInterfaceRepository;
            this.sourceNatRuleRepository = sourceNatRuleRepository;
            this.destinationNatRuleRepository = destinationNatRuleRepository;
            this.routeRepository = routeRepository;
            this.mapper = mapper;
        }

        public List<InterfaceDTO> GetInterfacesInRevision(int revisionId)
        {
            var interfaces = interfaceRepository.GetForRevision(revisionId).ToList();
            return mapper.Map<List<InterfaceDTO>>(interfaces);
        }

        public InterfaceDTO GetInterfaceWithName(int revisionId, string name)
        {
            var @interface = interfaceRepository.Find(i => i.Name == name && i.RevisionId == revisionId);

            return @interface == null ? null : mapper.Map<InterfaceDTO>(@interface);
        }

        public bool InterfaceHasVlan(int revisionId, string interfaceName, ushort vlanId)
        {
            return vlanInterfaceRepository.Exists(v =>
                v.RevisionId == revisionId && v.ParentInterfaceName == interfaceName && v.VlanId == vlanId);
        }

        public void PrintInterfaceToTextWriter(int revisionId, InterfaceDTO @interface, TextWriter writer)
        {
            if (@interface == null)
            {
                writer.WriteLine("Interface Not Configured");
                return;
            }

            var interfaceType = Enum.Parse<InterfaceType>(@interface.InterfaceType);
            writer.WriteLine($"{@interface.Name}: ({@interface.InterfaceType})");
            writer.WriteLine($"  Enabled: {@interface.Enabled}");
            writer.WriteLine($"  Description: {@interface.Description}");


            switch (interfaceType)
            {
                case InterfaceType.Vlan:
                    var vlanInterface = vlanInterfaceRepository.Find(v => v.InterfaceName == @interface.Name && v.RevisionId == revisionId);
                    writer.WriteLine("  Vlan:");
                    writer.WriteLine($"    Parent Interface: {vlanInterface.ParentInterfaceName}");
                    writer.WriteLine($"    Vlan Id: {vlanInterface.VlanId}");
                    break;
                
            }

            writer.WriteLine($"  Addresses:");
            var addresses = interfaceAddressRepository.GetForRevision(revisionId)
                .Where(a => a.InterfaceName == @interface.Name);
            foreach (var address in addresses)
            {
                if (address.AddressConfigurationType == AddressConfigurationType.Static)
                {
                    writer.WriteLine($"  - {address.Address}/{address.SubnetMask}");
                }
                else
                {
                    writer.WriteLine($"  - {address.AddressConfigurationType}");
                }
            }

            var inFirewallTable = firewallTableRepository.Find(t =>
                t.RevisionId == revisionId && t.Id == @interface.InboundFirewallTableId);
            var outFirewallTable = firewallTableRepository.Find(t =>
                t.RevisionId == revisionId && t.Id == @interface.OutboundFirewallTableId);
            var localFirewallTable = firewallTableRepository.Find(t =>
                t.RevisionId == revisionId && t.Id == @interface.LocalFirewallTableId);

            writer.WriteLine("  Firewall Tables:");
            writer.WriteLine($"    In: {inFirewallTable?.Name ?? "None"}");
            writer.WriteLine($"    Out: {outFirewallTable?.Name ?? "None"}");
            writer.WriteLine($"    Local: {localFirewallTable?.Name ?? "None"}");
        }

        public void RemoveInterface(int revisionId, string name)
        {
            var @interface = interfaceRepository.Find(i => i.RevisionId == revisionId && i.Name == name);

            if (@interface == null)
            {
                throw new Exception("Can't delete an interface that doesn't exist");
            }

            interfaceAddressRepository.Delete(a => a.RevisionId == revisionId && a.InterfaceName == name);
            sourceNatRuleRepository.Delete(s => s.RevisionId == revisionId && s.OutboundInterfaceName == name);
            destinationNatRuleRepository.Delete(d => d.RevisionId == revisionId && d.InboundInterfaceName == name);
            routeRepository.Delete(r => r.RevisionId == revisionId && r.InterfaceName == name);

            var vlanSubInterfaces = vlanInterfaceRepository.GetForRevision(revisionId)
                .Where(v => v.ParentInterfaceName == name).ToList();
            foreach (var vlanSubInterface in vlanSubInterfaces)
            {
                RemoveVlan(revisionId, name, vlanSubInterface.VlanId);
            }


            interfaceRepository.Delete(@interface);

            interfaceRepository.SaveChanges();
        }

        public void RemoveVlan(int revisionId, string parentInterfaceName, ushort vlandId)
        {
            var vlanInterface = vlanInterfaceRepository.Find(v =>
                v.RevisionId == revisionId && v.ParentInterfaceName == parentInterfaceName && v.VlanId == vlandId);

            if (vlanInterface == null)
            {
                throw new Exception("");
            }

            if (interfaceRepository.Exists(i => i.RevisionId == revisionId && i.Name == vlanInterface.InterfaceName))
            {
                RemoveInterface(revisionId, vlanInterface.InterfaceName);
            }

            vlanInterfaceRepository.Delete(vlanInterface);

            interfaceRepository.SaveChanges();
        }
    }
}