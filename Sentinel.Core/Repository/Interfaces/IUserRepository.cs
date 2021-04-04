using Sentinel.Core.Entities;

namespace Sentinel.Core.Repository.Interfaces
{
    public interface IUserRepository
    {
        User CreateUser(string userName, string password);
    }
}