using System;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SentinelDatabaseContext databaseContext;

        public UserRepository(SentinelDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public User CreateUser(string userName, string password)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = userName,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };
            databaseContext.Users.Add(user);
            databaseContext.SaveChanges();
            return user;
        }
    }
}