using System;
using Sentinel.Models;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IFirewallTableService
    {
        FirewallTableDTO GetFirewallTableById(int revisionId, Guid firewallTableId);
    }
}