using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class InterfaceAddressRepository : BaseVersionedRepository<InterfaceAddress>, IInterfaceAddressRepository
    {
        public InterfaceAddressRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext, revisionRepository)
        {

        }
    }
}