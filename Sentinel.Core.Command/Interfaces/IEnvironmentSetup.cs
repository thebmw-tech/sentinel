namespace Sentinel.Core.Command.Interfaces
{
    public interface IEnvironmentSetup
    {
        string[] Setup(IShell shell, string[] args);
        void Cleanup(IShell shell);
    }
}