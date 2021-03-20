namespace Sentinel.Core.Generators.Interfaces
{
    public interface IConfigurationGenerator<T>
    {
        public void Generate();
        public bool Apply();
    }
}