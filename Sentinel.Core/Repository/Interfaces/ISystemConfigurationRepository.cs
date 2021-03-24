using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface ISystemConfigurationRepository
    {
        SystemConfiguration GetSafeConfiguration();
        SystemConfiguration GetCurrentConfiguration();
        SystemConfiguration GetInProgressConfiguration();
    }
}