using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class VlanInterfaceRepository : BaseVersionedRepository<VlanInterface>, IVlanInterfaceRepository
    {
        public VlanInterfaceRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(dbContext, revisionRepository)
        {
        }
    }
}