using System.Linq;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class RevisionRepository : BaseRepository<Revision>, IRevisionRepository
    { 

        public RevisionRepository(SentinelDatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public int GetSafeCurrentRevision()
        {
            var revision = DbSet.Where(r => r.CommitDate.HasValue && r.ConfirmDate.HasValue)
                .OrderBy(r => r.Id).Reverse().First();
            return revision.Id;
        }

        public int GetCurrentRevision()
        {
            var revision = DbSet.Where(r => r.CommitDate.HasValue)
                .OrderBy(r => r.Id).Reverse().First();
            return revision.Id;
        }

        public int? GetInProgressRevision()
        {
            var revision = DbSet.Where(r => !r.CommitDate.HasValue)
                .OrderBy(r => r.Id).Reverse().FirstOrDefault();
            return revision?.Id;
        }

        public Entities.Revision CreateNewRevision()
        {
            var revision = new Revision();
            revision = Create(revision);
            SaveChanges();
            return revision;
        }
    }
}