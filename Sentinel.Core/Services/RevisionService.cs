using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Entities;
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
        private readonly IInterfaceAddressRepository interfaceAddressRepository;
        private readonly IInterfaceRepository interfaceRepository;
        private readonly ISourceNatRuleRepository sourceNatRuleRepository;
        private readonly ISystemConfigurationRepository systemConfigurationRepository;
        private readonly IVlanInterfaceRepository vlanInterfaceRepository;

        private readonly IConfigurationGeneratorService configurationGeneratorService;

        private readonly IMapper mapper;
        
        private readonly ILogger<RevisionService> logger;

        public RevisionService(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository,
            IDestinationNatRuleRepository destinationNatRuleRepository, IFirewallRuleRepository firewallRuleRepository,
            IFirewallTableRepository firewallTableRepository, IInterfaceAddressRepository interfaceAddressRepository,
            IInterfaceRepository interfaceRepository, ISourceNatRuleRepository sourceNatRuleRepository,
            ISystemConfigurationRepository systemConfigurationRepository, IVlanInterfaceRepository vlanInterfaceRepository,
            IConfigurationGeneratorService configurationGeneratorService, IMapper mapper, ILogger<RevisionService> logger)
        {
            this.dbContext = dbContext;

            this.revisionRepository = revisionRepository;

            this.destinationNatRuleRepository = destinationNatRuleRepository;
            this.firewallRuleRepository = firewallRuleRepository;
            this.firewallTableRepository = firewallTableRepository;
            this.interfaceAddressRepository = interfaceAddressRepository;
            this.interfaceRepository = interfaceRepository;
            this.sourceNatRuleRepository = sourceNatRuleRepository;
            this.systemConfigurationRepository = systemConfigurationRepository;
            this.vlanInterfaceRepository = vlanInterfaceRepository;

            this.configurationGeneratorService = configurationGeneratorService;

            this.mapper = mapper;
            this.logger = logger;
        }

        public void CleanupOldLocks()
        {
            using var transaction = dbContext.Database.BeginTransaction();

            var revisionsWithLocks = revisionRepository.Filter(r => r.Locked.HasValue).ToList();

            foreach (var revsion in revisionsWithLocks)
            {
                if ((DateTime.UtcNow - revsion.Locked).Value.TotalMinutes > SentinelConstants.LOCK_EXP_MIN)
                {
                    revsion.Locked = null;
                    revisionRepository.Update(revsion);
                }
            }

            revisionRepository.SaveChanges();

            transaction.Commit();
        }

        private void RollbackRevision(Revision revision)
        {
            revision.CommitDate = null;
            revisionRepository.Update(revision);
            revisionRepository.SaveChanges();
        }

        public void RollbackRevision(int revisionId)
        {
            var revision = revisionRepository.Find(r => r.Id == revisionId);
            if (revision.ConfirmDate.HasValue)
            {
                throw new ArgumentOutOfRangeException(nameof(revisionId), "Can't Rollback Confirmed Revision");
            }
            RollbackRevision(revision);
        }

        public void RollbackIfNotConfirmed(int revisionId)
        {
            var revision = revisionRepository.Find(r => r.Id == revisionId);
            if (revision.ConfirmDate.HasValue)
            {
                return;
            }
            RollbackRevision(revision);
            configurationGeneratorService.GenerateAndApply();
        }

        public void CommitRevision(int revisionId)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            var revision = revisionRepository.Find(r => r.Id == revisionId);
            revision.CommitDate = DateTime.UtcNow;
            revisionRepository.Update(revision);
            revisionRepository.SaveChanges();

            try
            {
                configurationGeneratorService.Generate();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error generating new configurations");
                RollbackRevision(revisionId);
                configurationGeneratorService.Generate();
            }

            try
            {
                configurationGeneratorService.Apply();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error applying new configuration");
                RollbackRevision(revisionId);
                configurationGeneratorService.GenerateAndApply();
            }

            transaction.Commit();
        }

        public void ConfirmRevision(int revisionId)
        {
            var revision = revisionRepository.Find(r => r.Id == revisionId);
            revision.ConfirmDate = DateTime.UtcNow;
            revision.Locked = null;
            revisionRepository.Update(revision);
            revisionRepository.SaveChanges();
        }

        public RevisionDTO CreateRevisionForEditing()
        {
            var inProgress = revisionRepository.GetInProgressRevisionId();
            if (inProgress.HasValue)
            {
                var inProgressRevision = revisionRepository.Find(r => r.Id == inProgress);
                if (inProgressRevision.Locked.HasValue)
                {
                    // TODO create custom exception for here.
                    throw new Exception("Someone else is editing the configuration already");
                }
                if (!inProgressRevision.Deleted && !inProgressRevision.HasChanges)
                {
                    inProgressRevision.Locked = DateTime.UtcNow;
                    revisionRepository.Update(inProgressRevision);
                    revisionRepository.SaveChanges();

                    return mapper.Map<RevisionDTO>(inProgressRevision);
                }
                // TODO create custom exception for here
                throw new Exception("Unlocked Configuration In Progress");
            }


            using var transaction = dbContext.Database.BeginTransaction();

            var revision = revisionRepository.CreateNewRevision();

            destinationNatRuleRepository.CopySafeToRevision(revision.Id);
            firewallRuleRepository.CopySafeToRevision(revision.Id);
            firewallTableRepository.CopySafeToRevision(revision.Id);
            interfaceRepository.CopySafeToRevision(revision.Id);
            interfaceAddressRepository.CopySafeToRevision(revision.Id);
            sourceNatRuleRepository.CopySafeToRevision(revision.Id);
            systemConfigurationRepository.CopySafeToRevision(revision.Id);
            vlanInterfaceRepository.CopySafeToRevision(revision.Id);

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

        public void UpdateRevisionLock(int revisionId)
        {
            var revision = revisionRepository.Find(r => r.Id == revisionId);
            if (revision != null)
            {
                revision.Locked = DateTime.UtcNow;
                revisionRepository.Update(revision);
                revisionRepository.SaveChanges();
            }
        }

        public void UnlockRevision(int revisionId)
        {
            var revision = revisionRepository.Find(r => r.Id == revisionId);
            if (revision != null)
            {
                revision.Locked = null;
                revisionRepository.Update(revision);
                revisionRepository.SaveChanges();
            }
        }

        public void CleanupAllLocks()
        {
            using var transaction = dbContext.Database.BeginTransaction();

            var revisionsWithLocks = revisionRepository.Filter(r => r.Locked.HasValue).ToList();

            foreach (var revsion in revisionsWithLocks)
            {
                revsion.Locked = null;
                revisionRepository.Update(revsion);
            }

            revisionRepository.SaveChanges();

            transaction.Commit();
        }

        public RevisionDTO ResumeEditingRevision()
        {
            var inProgress = revisionRepository.GetInProgressRevisionId();
            if (inProgress.HasValue)
            {
                var inProgressRevision = revisionRepository.Find(r => r.Id == inProgress);
                if (inProgressRevision.Locked.HasValue)
                {
                    // TODO create custom exception for here.
                    throw new Exception("Someone else is editing the configuration already");
                }

                inProgressRevision.Locked = DateTime.UtcNow;
                revisionRepository.Update(inProgressRevision);
                revisionRepository.SaveChanges();

                return mapper.Map<RevisionDTO>(inProgressRevision);
            }

            return null;
        }
    }
}