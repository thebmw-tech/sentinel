using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Entities;
using Sentinel.Core.Environments;
using Sentinel.Core.Factories;
using Sentinel.Core.Generators;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Repository;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection RegisterSentinelCoreCommand(this IServiceCollection services)
        {
            services.AddTransient<CommandInterpreter>();
            services.AddTransient<EnvironmentSetupFactory>();
            services.AddTransient(typeof(SubCommandInterpreter<>));

            services.AddScoped<ShellEnvironment>();
            services.AddScoped<ConfigurationEnvironment>();
            services.AddScoped<InterfaceEnvironment>();
            services.AddScoped<FirewallTableEnvironment>();
            

            return services;
        }
    }
}
