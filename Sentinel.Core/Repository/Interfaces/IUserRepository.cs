using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User CreateUser(string userName, string password);
    }
}