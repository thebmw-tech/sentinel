using System.Linq;
using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IVersionedRepository<T> : IRepository<T> where T : BaseVersionedEntity<T>
    {
        IQueryable<T> GetCurrent();
        IQueryable<T> GetInProgress();
        IQueryable<T> GetSafe(); // Brandon Warner (2021-04-10): Do we need this?
        public IQueryable<T> GetForRevision(int revisionId);
    }
}