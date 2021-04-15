using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface ISystemConfigurationRepository : IRepository<SystemConfiguration>
    {
        SystemConfiguration GetSafe();
        SystemConfiguration GetCurrent();
        SystemConfiguration GetInProgress();

        void CopySafeToRevision(int revisionId);
    }
}