using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SentinelDatabaseContext databaseContext) : base(databaseContext)
        {
            
        }

        public User CreateUser(string userName, string password)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = userName,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }
    }
}