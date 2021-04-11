using Microsoft.Extensions.Logging;
using Sentinel.Core.Generators.Interfaces;

namespace Sentinel.Core.Generators.Dummy
{
    public class DummyConfigurationGenerator<T> : IConfigurationGenerator<T>
    {
        private readonly ILogger<DummyConfigurationGenerator<T>> logger;

        public DummyConfigurationGenerator(ILogger<DummyConfigurationGenerator<T>> logger)
        {
            this.logger = logger;
        }

        public bool Apply()
        {
            logger.LogInformation($"Using dummy config apply for {nameof(T)}.");
            return true;
        }

        public void Generate()
        {
            logger.LogInformation($"Using dummy config generator for {nameof(T)}.");
        }
    }
}