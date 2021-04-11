using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly SentinelDatabaseContext databaseContext;
        private readonly IRevisionRepository revisionRepository;

        public SystemConfigurationRepository(SentinelDatabaseContext databaseContext, IRevisionRepository revisionRepository)
        {
            this.databaseContext = databaseContext;
            this.revisionRepository = revisionRepository;
        }

        public SystemConfiguration GetCurrentConfiguration()
        {
            var revisionId = revisionRepository.GetCurrentRevisionId();
            var configuration = GetConfigurationForRevision(revisionId);
            return configuration;
        }

        public SystemConfiguration GetInProgressConfiguration()
        {
            var revisionId = revisionRepository.GetInProgressRevisionId();
            if (revisionId.HasValue)
            {
                var configuration = GetConfigurationForRevision(revisionId.Value);
                return configuration;
            }

            return null;
        }

        public SystemConfiguration GetSafeConfiguration()
        {
            var revisionId = revisionRepository.GetSafeRevisionId();
            var configuration = GetConfigurationForRevision(revisionId);
            return configuration;
        }

        private SystemConfiguration GetConfigurationForRevision(int revisionId)
        {
            var configuration = databaseContext.SystemConfigurations.FirstOrDefault(sc => sc.RevisionId == revisionId);

            return configuration;
        }
    }
}