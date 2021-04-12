using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Entities;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Interfaces;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Services
{
    public class ConfigurationGeneratorService : IConfigurationGeneratorService
    {
        private readonly IConfigurationGenerator<IConfigurationEntity>[] configurationGenerators;

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ConfigurationGeneratorService> logger;

        public ConfigurationGeneratorService(IServiceProvider serviceProvider,
            ILogger<ConfigurationGeneratorService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            var configGenBase = typeof(IConfigurationGenerator<>);

            configurationGenerators = configGenBase.Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(IConfigurationEntity)) && !t.IsAbstract).Select(t => configGenBase.MakeGenericType(t))
                .Select(t => (IConfigurationGenerator<IConfigurationEntity>)serviceProvider.GetService(t)).ToArray();
        }

        public void Apply()
        {
            foreach (var configurationGenerator in configurationGenerators)
            {
                configurationGenerator.Apply();
            }
        }

        public void Generate()
        {
            foreach (var configurationGenerator in configurationGenerators)
            {
                configurationGenerator.Generate();
            }
        }

        public void GenerateAndApply()
        {
            Generate();
            Apply();
        }
    }
}