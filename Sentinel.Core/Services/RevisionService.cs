using AutoMapper;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Services
{
    public class RevisionService : IRevisionService
    {
        private readonly SentinelDatabaseContext dbContext;

        private readonly IRevisionRepository revisionRepository;

        private readonly IDestinationNatRuleRepository destinationNatRuleRepository;
        private readonly IFirewallRuleRepository firewallRuleRepository;
        private readonly IFirewallTableRepository firewallTableRepository;
        private readonly IInterfaceRepository interfaceRepository;
        private readonly ISourceNatRuleRepository sourceNatRuleRepository;
        private readonly ISystemConfigurationRepository systemConfigurationRepository;

        private readonly IMapper mapper;
        
        private readonly ILogger<RevisionService> logger;

        public RevisionService(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository,
            IDestinationNatRuleRepository destinationNatRuleRepository, IFirewallRuleRepository firewallRuleRepository,
            IFirewallTableRepository firewallTableRepository, IInterfaceRepository interfaceRepository,
            ISourceNatRuleRepository sourceNatRuleRepository, ISystemConfigurationRepository systemConfigurationRepository,
            IMapper mapper, ILogger<RevisionService> logger)
        {
            this.dbContext = dbContext;

            this.revisionRepository = revisionRepository;

            this.destinationNatRuleRepository = destinationNatRuleRepository;
            this.firewallRuleRepository = firewallRuleRepository;
            this.firewallTableRepository = firewallTableRepository;
            this.interfaceRepository = interfaceRepository;
            this.sourceNatRuleRepository = sourceNatRuleRepository;
            this.systemConfigurationRepository = systemConfigurationRepository;

            this.mapper = mapper;
            this.logger = logger;
        }

        public RevisionDTO CreateRevisionForEditing()
        {
            using var transaction = dbContext.Database.BeginTransaction();

            var revision = revisionRepository.CreateNewRevision();

            destinationNatRuleRepository.CopySafeToRevision(revision.Id);
            firewallRuleRepository.CopySafeToRevision(revision.Id);
            firewallTableRepository.CopySafeToRevision(revision.Id);
            interfaceRepository.CopySafeToRevision(revision.Id);
            sourceNatRuleRepository.CopySafeToRevision(revision.Id);
            systemConfigurationRepository.CopySafeToRevision(revision.Id);

            transaction.Commit();

            return mapper.Map<RevisionDTO>(revision);
        }

        public RevisionDTO GetRevisionById(int revisionId)
        {
            var revision = revisionRepository.Find(r => r.Id == revisionId);
            return mapper.Map<RevisionDTO>(revision);
        }

        public RevisionDTO GetSafe()
        {
            var revisionId = revisionRepository.GetSafeRevisionId();
            return GetRevisionById(revisionId);
        }
    }
}