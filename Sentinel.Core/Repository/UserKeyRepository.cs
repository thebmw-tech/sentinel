using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class UserKeyRepository : BaseRepository<UserKey>, IUserKeyRepository
    {
        public UserKeyRepository(SentinelDatabaseContext databaseContext) : base(databaseContext)
        {

        }
    }
}