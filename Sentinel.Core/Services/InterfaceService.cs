﻿using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Services.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;
using System.IO;
using System.Linq;
using Sentinel.Core.Enums;

namespace Sentinel.Core.Services
{
    public class InterfaceService : IInterfaceService
    {
        private readonly IInterfaceRepository interfaceRepository;
        private readonly IFirewallTableRepository firewallTableRepository;
        private readonly IInterfaceAddressRepository interfaceAddressRepository;

        private readonly IMapper mapper;

        public InterfaceService(IInterfaceRepository interfaceRepository, IFirewallTableRepository firewallTableRepository,
            IInterfaceAddressRepository interfaceAddressRepository, IMapper mapper)
        {
            this.interfaceRepository = interfaceRepository;
            this.firewallTableRepository = firewallTableRepository;
            this.interfaceAddressRepository = interfaceAddressRepository;
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

            writer.WriteLine($"  Address Configuration: {@interface.IPv4ConfigurationType}");
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
                    writer.WriteLine($"  - {address.AddressConfigurationType.ToString()}");
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
    }
}