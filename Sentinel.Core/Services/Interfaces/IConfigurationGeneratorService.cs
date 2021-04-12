namespace Sentinel.Core.Services.Interfaces
{
    public interface IConfigurationGeneratorService
    {
        void Apply();
        void Generate();
        void GenerateAndApply();
    }
}