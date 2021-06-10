using System;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Sentinel.Core.Entities;
using Sentinel.Core.Entities.Maps;
using Sentinel.Core.Helpers;

namespace Sentinel.Core
{
    public class SentinelDatabaseContext : DbContext
    {
        private const string SQLITE = "sqlite";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = SentinelConfiguration.Instance;

            switch (configuration.DatabaseProvider)
            {
                case SQLITE:
                    optionsBuilder.UseSqlite(configuration.DatabaseConnectionString);
                    break;
                default:
                    throw new Exception($"Unsupported database provider {configuration.DatabaseProvider}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(GetType()));
        }
    }
}