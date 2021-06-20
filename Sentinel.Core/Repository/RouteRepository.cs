using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class RouteRepository : BaseVersionedRepository<Route>, IRouteRepository
    {
        public RouteRepository(SentinelDatabaseContext dbContext, IRevisionRepository revisionRepository) : base(
            dbContext, revisionRepository)
        {

        }
    }
}