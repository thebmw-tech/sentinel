using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;
using System.Linq;

namespace Sentinel.Core.Repository
{
    public abstract class BaseVersionedRepository<T> : BaseRepository<T>, IVersionedRepository<T> where T : BaseVersionedEntity<T>, new()
    {
        protected readonly IRevisionRepository revisionRepository;

        protected BaseVersionedRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext)
        {
            this.revisionRepository = revisionRepository;
        }

        public IQueryable<T> GetCurrent()
        {
            var revisionId = revisionRepository.GetCurrentRevisionId();
            return GetForRevision(revisionId);
        }

        public IQueryable<T> GetInProgress()
        {
            var revisionId = revisionRepository.GetInProgressRevisionId();
            if (revisionId.HasValue)
            {
                return GetForRevision(revisionId.Value);
            }

            return null;
        }

        public IQueryable<T> GetSafe()
        {
            var revisionId = revisionRepository.GetSafeRevisionId();
            return GetForRevision(revisionId);
        }

        public IQueryable<T> GetForRevision(int revisionId)
        {
            return DbSet.Where(t => t.RevisionId == revisionId);
        }
    }
}