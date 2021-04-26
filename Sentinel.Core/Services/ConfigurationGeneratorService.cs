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
   

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ConfigurationGeneratorService> logger;

        public ConfigurationGeneratorService(IServiceProvider serviceProvider,
            ILogger<ConfigurationGeneratorService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            
        }

        public void Apply()
        {
            
        }

        public void Generate()
        {
           
        }

        public void GenerateAndApply()
        {
            Generate();
            Apply();
        }
    }
}