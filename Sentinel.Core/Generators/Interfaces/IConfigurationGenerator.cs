namespace Sentinel.Core.Generators.Interfaces
{
    public interface IConfigurationGenerator<T> where T : IConfigurationGenerator<T>
    {
        public void Generate();
        public void Apply();
    }
}