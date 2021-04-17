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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaseFile = HelperFunctions.FindExistingFile(new[]
            {
                "/etc/sentinel/conf.db",
                "~/.config/sentinel/conf.db",
                "C:\\projects\\sentinel\\conf.db",
            }, "/etc/sentinel/conf.db");

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(databaseFile));

            optionsBuilder.UseSqlite($"Data Source={databaseFile}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var mapTypes = Assembly.GetAssembly(GetType()).GetTypes()
            //    .Where(t => t.IsSubclassOf(typeof(BaseEntityMap<>))).ToList();

            //foreach (var mapType in mapTypes)
            //{
            //    var entityType = (Type)mapType.GetMethod("GetEntityType").Invoke(null, new object[]{});

            //    var applyMethod = typeof(ModelBuilder).GetMethod("ApplyConfiguration");
            //    applyMethod = applyMethod.MakeGenericMethod(entityType);

            //    var mapInstance = Activator.CreateInstance(mapType);
            //    applyMethod.Invoke(modelBuilder, new object?[] { mapInstance });
            //}



            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(GetType()));

            /*
            modelBuilder.ApplyConfiguration(new FirewallRuleMap());
            modelBuilder.ApplyConfiguration(new GatewayMap());
            modelBuilder.ApplyConfiguration(new InterfaceMap());
            modelBuilder.ApplyConfiguration(new RevisionMap());
            modelBuilder.ApplyConfiguration(new RouteMap());
            modelBuilder.ApplyConfiguration(new SystemConfigurationMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            */

        }


        public DbSet<User> Users { get; set; }
    }
}