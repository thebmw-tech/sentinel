using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class SystemConfigurationRepository : BaseRepository<SystemConfiguration>, ISystemConfigurationRepository
    {
        private readonly IRevisionRepository revisionRepository;

        public SystemConfigurationRepository(SentinelDatabaseContext databaseContext, IRevisionRepository revisionRepository) : base(databaseContext)
        {
            this.revisionRepository = revisionRepository;
        }

        public void CopySafeToRevision(int revisionId)
        {
            var safe = GetSafe();
            var newCopy = safe.GetCopyForRevision(revisionId);
            Create(newCopy);
            SaveChanges();
        }

        public SystemConfiguration GetCurrent()
        {
            var revisionId = revisionRepository.GetCurrentRevisionId();
            var configuration = GetConfigurationForRevision(revisionId);
            return configuration;
        }

        public SystemConfiguration GetInProgress()
        {
            var revisionId = revisionRepository.GetInProgressRevisionId();
            if (revisionId.HasValue)
            {
                var configuration = GetConfigurationForRevision(revisionId.Value);
                return configuration;
            }

            return null;
        }

        public SystemConfiguration GetSafe()
        {
            var revisionId = revisionRepository.GetSafeRevisionId();
            var configuration = GetConfigurationForRevision(revisionId);
            return configuration;
        }

        private SystemConfiguration GetConfigurationForRevision(int revisionId)
        {
            var configuration = DbSet.FirstOrDefault(sc => sc.RevisionId == revisionId);
            return configuration;
        }

        
    }
}