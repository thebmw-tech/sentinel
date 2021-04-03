using Sentinel.Core.Command.Enums;

namespace Sentinel.Core.Command.Interfaces
{
    public interface ICommand
    {
        CommandReturn Execute(string command);
        void Help(string command);
        string Suggest(string command);
    }
}