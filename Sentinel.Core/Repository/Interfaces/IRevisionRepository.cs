using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IRevisionRepository : IRepository<Revision>
    {
        Revision CreateNewRevision();
        int GetSafeRevisionId();
        int GetCurrentRevisionId();
        int? GetInProgressRevisionId();
    }
}