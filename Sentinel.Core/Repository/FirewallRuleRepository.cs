using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class FirewallRuleRepository : BaseRepository<FirewallRule>, IFirewallRuleRepository
    {
        public FirewallRuleRepository(SentinelDatabaseContext dbContext) : base(dbContext)
        {

        }
    }
}