using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Services.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;
using System.IO;
using System.Linq;

namespace Sentinel.Core.Services
{
    public class InterfaceService : IInterfaceService
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IFirewallTableRepository firewallTableRepository;

        private readonly IMapper mapper;

        public InterfaceService(IInterfaceRepository interfaceRepository, IFirewallTableRepository firewallTableRepository,
            IMapper mapper)
        {
            this.interfaceRepository = interfaceRepository;
            this.firewallTableRepository = firewallTableRepository;
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
            return mapper.Map<InterfaceDTO>(@interface);
        }

        public void PrintInterfaceToTextWriter(int revisionId, InterfaceDTO @interface, TextWriter writer)
        {

            writer.WriteLine($"{@interface.Name}: ({@interface.InterfaceType})");
            writer.WriteLine($"  Enabled: {@interface.Enabled}");
            writer.WriteLine($"  Description: {@interface.Description}");

            writer.WriteLine($"  v4 Configuration: {@interface.IPv4ConfigurationType}");
            if (@interface.IPv4SubnetMask.HasValue)
            {
                writer.WriteLine($"  v4 Address: {@interface.IPv4Address}/{@interface.IPv4SubnetMask}");
            }

            writer.WriteLine($"  v6 Configuration: {@interface.IPv6ConfigurationType}");
            if (@interface.IPv6SubnetMask.HasValue)
            {
                writer.WriteLine($"  v6 Address: {@interface.IPv6Address}/{@interface.IPv6SubnetMask}");
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
    }
}