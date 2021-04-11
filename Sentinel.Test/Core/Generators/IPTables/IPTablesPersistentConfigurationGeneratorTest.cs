using NLog.Time;
using Sentinel.Core.Generators.IPTables;
using Sentinel.Test.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Test.Core.Generators.IPTables
{
    public class IPTablesPersistentConfigurationGeneratorTest
    {
        private readonly TestMockDIHelper<IPTablesPersistentConfigurationGenerator> diHelper;

        private readonly ITestOutputHelper outputHelper;

        private readonly IPTablesPersistentConfigurationGenerator generator;

        public IPTablesPersistentConfigurationGeneratorTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            diHelper = new TestMockDIHelper<IPTablesPersistentConfigurationGenerator>();

            generator = diHelper.GetInstance();
        }

        [Fact]
        public void TestGenerate()
        {

        }
    }
}