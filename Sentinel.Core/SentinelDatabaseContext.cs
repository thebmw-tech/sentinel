﻿using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Sentinel.Core.Entities;
using Sentinel.Core.Entities.Maps;

namespace Sentinel.Core
{
    public class SentinelDatabaseContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InterfaceMap());
            modelBuilder.ApplyConfiguration(new GatewayMap());
            modelBuilder.ApplyConfiguration(new RouteMap());
            modelBuilder.ApplyConfiguration(new SystemConfigurationMap());

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Revision> Revisions { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<Interface> Interfaces { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Route> Routes { get; set; }
    }
}