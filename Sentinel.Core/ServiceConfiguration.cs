using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Sentinel.Core.Entities;
using Sentinel.Core.Generators;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Generators.IPTables;
using Sentinel.Core.Generators.Netplan;
using Sentinel.Core.Generators.NetworkD;
using Sentinel.Core.Helpers;
using Sentinel.Core.Repository;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Core.Validation;
using Sentinel.Core.Validation.Entities;
using Hangfire;
using Hangfire.Storage.SQLite;

namespace Sentinel.Core
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection RegisterSentinelCore(this IServiceCollection services)
        {
            services.AddSingleton(SentinelConfiguration.Instance);

            services.AddDbContext<SentinelDatabaseContext>();

            // Setup Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            // Setup Hangfire
            services.AddHangfire(configuration =>
            {
                configuration.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseColouredConsoleLogProvider();

                if (SentinelConfiguration.Instance.HangfireDatabaseProvider == "sqlite")
                {
                    configuration.UseSQLiteStorage(SentinelConfiguration.Instance.HangfireDatabaseConnectionString);
                }
            });

            // Setup Mapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Setup Abstractions & Helpers
            services.AddTransient<IFileSystem, FileSystem>(); // TODO verify this should be transient
            services.AddTransient<ICommandExecutionHelper, CommandExecutionHelper>();
            services.AddTransient<Validator>();

            // Setup Repositories
            services.AddTransient<IDestinationNatRuleRepository, DestinationNatRuleRepository>();
            services.AddTransient<IFirewallRuleRepository, FirewallRuleRepository>();
            services.AddTransient<IFirewallTableRepository, FirewallTableRepository>();
            services.AddTransient<IInterfaceAddressRepository, InterfaceAddressRepository>();
            services.AddTransient<IInterfaceRepository, InterfaceRepository>();
            services.AddTransient<IRevisionRepository, RevisionRepository>();
            services.AddTransient<ISourceNatRuleRepository, SourceNatRuleRepository>();
            services.AddTransient<ISystemConfigurationRepository, SystemConfigurationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IVlanInterfaceRepository, VlanInterfaceRepository>();

            // Setup Services
            services.AddTransient<IConfigurationGeneratorService, ConfigurationGeneratorService>();
            services.AddTransient<IFirewallTableService, FirewallTableService>();
            services.AddTransient<IInterfaceService, InterfaceService>();
            services.AddTransient<IRevisionService, RevisionService>();

            // Add Platform Dependent Services
#if DEBUG
            services.AddTransient<IKernelInterfaceService, KernelInterfaceServiceMock>();
#else
             services.AddTransient<IKernelInterfaceService, KernelInterfaceService>();
#endif

            // Register All Generators Even Ones That Aren't Used
            services.AddTransient<IConfigurationGenerator<IPTablesPersistentConfigurationGenerator>, IPTablesPersistentConfigurationGenerator>();;
            services.AddTransient<IConfigurationGenerator<NetplanInterfaceConfigurationGenerator>, NetplanInterfaceConfigurationGenerator>();
            services.AddTransient<IConfigurationGenerator<NetworkDConfigurationGenerator>, NetworkDConfigurationGenerator>();



            // Setup Validators
            services.AddTransient<BaseValidator<Interface>, InterfaceValidator>();


            return services;
        }
    }
}
