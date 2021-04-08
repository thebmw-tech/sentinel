using System;
using System.Collections.Generic;
using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IGatewayRepository
    {
        List<Gateway> GetSafeGateways();
        List<Gateway> GetCurrentGateways();
        List<Gateway> GetInProgressGateways();

        Gateway GetCurrentGatewayById(Guid gatewayId);
    }
}