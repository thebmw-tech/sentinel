using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class FirewallRuleRepository : BaseVersionedRepository<FirewallRule>, IFirewallRuleRepository
    {
        public FirewallRuleRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext, revisionRepository)
        {

        }
    }
}