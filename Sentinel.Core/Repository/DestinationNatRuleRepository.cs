using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class DestinationNatRuleRepository : BaseVersionedRepository<DestinationNatRule>, IDestinationNatRuleRepository
    {
        public DestinationNatRuleRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext, revisionRepository)
        {

        }
    }
}