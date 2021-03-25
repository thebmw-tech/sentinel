using Sentinel.Shell.Enums;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Interfaces
{
    public interface ICommand
    {
        CommandReturn Execute(ShellContext context, string command);
        void Help(string command);
        string Suggest(string command);
    }
}