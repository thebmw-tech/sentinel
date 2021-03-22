using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Sentinel.Core.Entities;
using Sentinel.Core.Generators;
using Sentinel.Core.Generators.Interface;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Repository;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core
{
    public static class ServiceConfiguration
    {
        public static ServiceCollection UseSentinelDi(this ServiceCollection services)
        {
            services.AddDbContext<SentinelDatabaseContext>();

            // Setup Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            // Setup Abstractions & Helpers
            services.AddTransient<IFileSystem, FileSystem>(); // TODO verify this should be transient

            // Setup Repositories
            services.AddTransient<IRevisionRepository, RevisionRepository>();
            services.AddTransient<IInterfaceRepository, InterfaceRepository>();

            // Setup Services
            services.AddTransient<IInterfaceService, InterfaceService>();

            // Setup Generators -- This will eventualy be based on some configuration file options.
            services.AddTransient<IConfigurationGenerator<Interface>, NetplanInterfaceConfigurationGenerator>();


            return services;
        }
    }
}
