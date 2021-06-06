namespace Sentinel.Core.Generators.Interfaces
{
    public interface IConfigurationGenerator<T> : IGenerator where T : IConfigurationGenerator<T>
    {
        
    }
}