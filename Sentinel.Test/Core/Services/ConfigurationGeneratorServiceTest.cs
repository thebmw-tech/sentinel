using System;
using System.Collections.Generic;
using Moq;
using Sentinel.Core.Entities;
using Sentinel.Core.Generators.Interfaces;
using Sentinel.Core.Interfaces;
using Sentinel.Core.Services;
using Sentinel.Test.Helpers;
using Xunit;

namespace Sentinel.Test.Core.Services
{
    public class ConfigurationGeneratorServiceTest
    {
        private readonly MockTestDIHelper<ConfigurationGeneratorService> diHelper;

        private readonly Mock<IServiceProvider> serviceProviderMock;
        private readonly Mock<IConfigurationGenerator<IConfigurationEntity>> configurationGeneratorMock;

        private readonly ConfigurationGeneratorService service;

        public ConfigurationGeneratorServiceTest()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            configurationGeneratorMock = new Mock<IConfigurationGenerator<IConfigurationEntity>>();

            serviceProviderMock.Setup(s => s.GetService(It.IsAny<Type>())).Returns(configurationGeneratorMock.Object);

            diHelper = new MockTestDIHelper<ConfigurationGeneratorService>(new Dictionary<Type, object>()
            {
                { typeof(IServiceProvider), serviceProviderMock.Object }
            });

            service = diHelper.GetInstance();
        }

        [Fact]
        public void TestGenerate()
        {
            configurationGeneratorMock.Setup(s => s.Generate()).Verifiable();

            service.Generate();

            configurationGeneratorMock.Verify(v => v.Generate(), Times.Exactly(8));
        }

        [Fact]
        public void TestApply()
        {
            configurationGeneratorMock.Setup(s => s.Apply()).Verifiable();

            service.Apply();

            configurationGeneratorMock.Verify(v => v.Apply(), Times.Exactly(8));
        }
    }
}