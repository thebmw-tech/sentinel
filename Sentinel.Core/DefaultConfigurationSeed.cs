using System;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core.Entities;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core
{
    public class DefaultConfigurationSeed
    {
        public static void Seed(ServiceProvider services)
        {
            var revisionRepo = services.GetService<IRevisionRepository>();
            var revision = revisionRepo.CreateNewRevision();
            var userRepo = services.GetService<IUserRepository>();
            var adminUser = userRepo.CreateUser("admin", "admin");
        }
    }
}