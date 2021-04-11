using System;
using System.Collections.Generic;
using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IGatewayRepository : IVersionedRepository<Gateway>
    {
        Gateway GetCurrentGatewayById(Guid gatewayId);
    }
}