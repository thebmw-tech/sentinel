using Sentinel.Core.Services;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Shell.Commands.Shell
{
    [Command(CommandMode.Shell, "configure")]
    public class ConfigurationCommand : ICommand
    {
        private readonly IInterfaceService interfaceService;

        public ConfigurationCommand(IInterfaceService interfaceService)
        {
            this.interfaceService = interfaceService;
        }

        public void Execute(string command)
        {
            throw new System.NotImplementedException();
        }

        public void Help(string command)
        {
            throw new System.NotImplementedException();
        }

        public string Suggest(string command)
        {
            throw new System.NotImplementedException();
        }
    }
}