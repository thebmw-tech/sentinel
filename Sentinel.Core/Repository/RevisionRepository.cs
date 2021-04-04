using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class RevisionRepository : IRevisionRepository
    {
        private readonly SentinelDatabaseContext databaseContext;

        public RevisionRepository(SentinelDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public int GetSafeCurrentRevision()
        {
            var revision = databaseContext.Revisions.Where(r => r.CommitDate.HasValue && r.ConfirmDate.HasValue)
                .OrderBy(r => r.Id).Reverse().First();
            return revision.Id;
        }

        public int GetCurrentRevision()
        {
            var revision = databaseContext.Revisions.Where(r => r.CommitDate.HasValue)
                .OrderBy(r => r.Id).Reverse().First();
            return revision.Id;
        }

        public int? GetInProgressRevision()
        {
            var revision = databaseContext.Revisions.Where(r => !r.CommitDate.HasValue)
                .OrderBy(r => r.Id).Reverse().FirstOrDefault();
            return revision?.Id;
        }

        public Entities.Revision CreateNewRevision()
        {
            var revision = new Revision();
            databaseContext.Revisions.Add(revision);
            databaseContext.SaveChanges();
            return revision;
        }
    }
}