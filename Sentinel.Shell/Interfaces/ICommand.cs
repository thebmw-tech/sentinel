using Sentinel.Shell.Enums;

namespace Sentinel.Shell.Interfaces
{
    public interface ICommand
    {
        CommandReturn Execute(string command);
        void Help(string command);
        string Suggest(string command);
    }
}