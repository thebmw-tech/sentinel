using System.Collections.Generic;
using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IRouteRepository
    {
        List<Route> GetSafeRoutes();
        List<Route> GetCurrentRoutes();
        List<Route> GetInProgressRoutes();
    }
}