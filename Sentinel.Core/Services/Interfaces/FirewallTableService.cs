using Sentinel.Models;
using System;
using AutoMapper;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Services.Interfaces
{
    public class FirewallTableService : IFirewallTableService
    {
        private IFirewallTableRepository firewallTableRepository;
        private IMapper mapper;

        public FirewallTableService(IFirewallTableRepository firewallTableRepository, IMapper mapper)
        {
            this.firewallTableRepository = firewallTableRepository;
            this.mapper = mapper;
        }

        public FirewallTableDTO GetFirewallTableById(int revisionId, Guid firewallTableId)
        {
            var firewallTable = firewallTableRepository.Find(t => t.RevisionId == revisionId && t.Id == firewallTableId);
            return mapper.Map<FirewallTableDTO>(firewallTable);
        }
    }
}