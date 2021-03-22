namespace Sentinel.Shell
{
    public interface ICommand
    {
        void Execute(string command);
        void Help(string command);
        string Suggest(string command);
    }
}