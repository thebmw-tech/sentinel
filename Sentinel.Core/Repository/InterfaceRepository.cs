using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class InterfaceRepository : BaseVersionedRepository<Interface>, IInterfaceRepository
    {
        public InterfaceRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext, revisionRepository)
        {

        }
    }
}