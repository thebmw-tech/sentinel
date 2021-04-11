using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class FirewallTableRepository : BaseVersionedRepository<FirewallTable>, IFirewallTableRepository
    {
        public FirewallTableRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) :
            base(dbContext, revisionRepository)
        {

        }
    }
}