﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Sentinel.Core.Entities;
using Sentinel.Core.Generators;
using Sentinel.Core.Generators.Dummy;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Generators.IPTables;
using Sentinel.Core.Generators.Netplan;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Core.Validation;
using Sentinel.Core.Validation.Entities;

namespace Sentinel.Core
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection RegisterSentinelCore(this IServiceCollection services)
        {
            services.AddDbContext<SentinelDatabaseContext>();

            // Setup Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            // Setup Abstractions & Helpers
            services.AddTransient<IFileSystem, FileSystem>(); // TODO verify this should be transient
            services.AddTransient<ICommandExecutionHelper, CommandExecutionHelper>();
            services.AddTransient<Validator>();

            // Setup Repositories
            services.AddTransient<IRevisionRepository, RevisionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IInterfaceRepository, InterfaceRepository>();
            services.AddTransient<ISystemConfigurationRepository, SystemConfigurationRepository>();
            services.AddTransient<IFirewallRuleRepository, FirewallRuleRepository>();

            // Setup Services
            services.AddTransient<IConfigurationGeneratorService, ConfigurationGeneratorService>();
            services.AddTransient<IInterfaceService, InterfaceService>();

            // Add Platform Dependent Services
#if DEBUG
            services.AddTransient<IKernelInterfaceService, KernelInterfaceServiceMock>();
#else
             services.AddTransient<IKernelInterfaceService, KernelInterfaceService>();
#endif

            // Setup Generators -- This will eventualy be based on some configuration file options.
            services.AddTransient<IConfigurationGenerator<DestinationNatRule>, DummyConfigurationGenerator<DestinationNatRule>>();
            services.AddTransient<IConfigurationGenerator<FirewallRule>, IPTablesPersistentConfigurationGenerator>();
            services.AddTransient<IConfigurationGenerator<FirewallTable>, DummyConfigurationGenerator<FirewallTable>>();
            services.AddTransient<IConfigurationGenerator<Gateway>, DummyConfigurationGenerator<Gateway>>();
            services.AddTransient<IConfigurationGenerator<Interface>, NetplanInterfaceConfigurationGenerator>();
            services.AddTransient<IConfigurationGenerator<Route>, DummyConfigurationGenerator<Route>>();
            services.AddTransient<IConfigurationGenerator<SourceNatRule>, DummyConfigurationGenerator<SourceNatRule>>();



            // Setup Validators
            services.AddTransient<BaseValidator<Interface>, InterfaceValidator>();


            return services;
        }
    }
}
