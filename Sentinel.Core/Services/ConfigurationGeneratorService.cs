using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Sentinel.Core.Entities;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Interfaces;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core.Services
{
    public class ConfigurationGeneratorService : IConfigurationGeneratorService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ConfigurationGeneratorService> logger;

        private List<IGenerator> generators;

        public ConfigurationGeneratorService(IServiceProvider serviceProvider,
            ILogger<ConfigurationGeneratorService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            LoadGenerators();
        }

        private void LoadGenerators()
        {
            var generatorTypes = Assembly.GetAssembly(GetType()).GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IGenerator)));

            var enabledGeneratorTypeNames = SentinelConfiguration.Instance.EnabledGenerators;

            var enabledGeneratorTypes =
                generatorTypes.Where(t => enabledGeneratorTypeNames.Any(n => t.Name.StartsWith(n)));

            var generatorGeneric = typeof(IConfigurationGenerator<>);

            enabledGeneratorTypes = enabledGeneratorTypes.Select(t => generatorGeneric.MakeGenericType(t));

            generators = enabledGeneratorTypes.Select(t => (IGenerator)serviceProvider.GetService(t)).Where(g => g != null).ToList();
        }

        public void Apply()
        {
            generators.ForEach(g => g.Apply());
        }

        public void Generate()
        {
           generators.ForEach(g => g.Generate());
        }

        public void GenerateAndApply()
        {
            Generate();
            Apply();
        }
    }
}