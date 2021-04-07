using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Sentinel.Core.Entities;
using Sentinel.Core.Entities.Maps;
using Sentinel.Core.Helpers;

namespace Sentinel.Core
{
    public class SentinelDatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaseFile = HelperFunctions.FindExistingFile(new[]
            {
                "/etc/sentinel/conf.db",
                "~/.config/sentinel/conf.db",
                "C:\\projects\\sentinel\\conf.db",
            }, "/etc/sentinel/conf.db");

            optionsBuilder.UseSqlite($"Data Source={databaseFile}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GatewayMap());
            modelBuilder.ApplyConfiguration(new InterfaceMap());
            modelBuilder.ApplyConfiguration(new RevisionMap());
            modelBuilder.ApplyConfiguration(new RouteMap());
            modelBuilder.ApplyConfiguration(new SystemConfigurationMap());
            modelBuilder.ApplyConfiguration(new UserMap());

        }

        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Interface> Interfaces { get; set; }
        public DbSet<Revision> Revisions { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}