namespace Sentinel.Core.Command.Interfaces
{
    public interface IFilteredCommand
    {
        bool ShouldShow(IShell shell);
    }
}