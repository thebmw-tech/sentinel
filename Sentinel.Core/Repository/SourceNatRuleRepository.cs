using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class SourceNatRuleRepository : BaseVersionedRepository<SourceNatRule>, ISourceNatRuleRepository
    {
        public SourceNatRuleRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext, revisionRepository)
        {

        }
    }
}