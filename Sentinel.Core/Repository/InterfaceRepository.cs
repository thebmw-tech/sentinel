using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class InterfaceRepository : BaseRepository<Interface>, IInterfaceRepository
    {
        public InterfaceRepository(SentinelDatabaseContext dbContext) : base(dbContext)
        {

        }
    }
}