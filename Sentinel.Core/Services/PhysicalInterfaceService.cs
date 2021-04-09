using Sentinel.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Services
{
    public class PhysicalInterfaceService : IPhysicalInterfaceService
    {
        private readonly ICommandExecutionHelper commandExecutionHelper;

        private const string IP_LINK_INTERFACE_NAME_REGEX = @"^[0-9]+: (?<name>[^:]+):.*mtu (?<mtu>[0-9]+).*state (?<state>[A-Z]+).*$";

        private readonly string[] VALID_IFACE_NAME_PREFIXES = new[] { "eth", "wlan" };

        public PhysicalInterfaceService(ICommandExecutionHelper commandExecutionHelper)
        {
            this.commandExecutionHelper = commandExecutionHelper;
        }

        public List<string> GetPhysicalInterfaceNames()
        {
            var output = commandExecutionHelper.Execute("ip", "link"); // TODO use full path to command
            var allInterfaces = GetInterfaceNamesFromCommandOutput(output.Item1);

            var phyInterfaces = FilterInterfaceNamesForPhy(allInterfaces);

            return phyInterfaces;
        }

        private List<string> FilterInterfaceNamesForPhy(List<string> allInterfaces)
        {
            var interfaces = allInterfaces.Where(iface => VALID_IFACE_NAME_PREFIXES.Any(p => iface.StartsWith(p)) && !iface.Contains('@')).ToList();

            return interfaces;
        }

        private List<string> GetInterfaceNamesFromCommandOutput(string commandOutput)
        {
            var nameRegex = new Regex(IP_LINK_INTERFACE_NAME_REGEX, RegexOptions.Multiline);
            var matches = nameRegex.Matches(commandOutput);

            return matches.Select(m => m.Groups["name"].Value).ToList();
        }
    }
}