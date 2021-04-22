using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;
using System.Linq;
using System.Linq.Expressions;

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

        public override T Create(T entity)
        {
            var e =  base.Create(entity);
            revisionRepository.MarkRevisionHasChanges(e.RevisionId);
            return e;
        }

        public void Modify(Expression<Func<T, bool>> predicate, Action<T> updateAction)
        {
            var entity = Find(predicate);
            revisionRepository.MarkRevisionHasChanges(entity.RevisionId);
            updateAction(entity);
            Update(entity);
            SaveChanges();
        }

        public void CopySafeToRevision(int revisionId)
        {
            var safeItems = GetSafe();
            foreach (var item in safeItems)
            {
                Create(item.GetCopyForRevision(revisionId));
            }

            SaveChanges();
        }
    }
}