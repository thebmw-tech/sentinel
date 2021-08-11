using System;
using System.Linq;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Environments
{
    public class FirewallTableEnvironment : IEnvironmentSetup
    {
        private readonly IFirewallTableRepository firewallTableRepository;

        public FirewallTableEnvironment(IFirewallTableRepository firewallTableRepository)
        {
            this.firewallTableRepository = firewallTableRepository;
        }

        public void Cleanup(IShell shell)
        {
            var keysToDelete = shell.Environment.Keys.Where(s => s.StartsWith("CONFIG_FIREWALL_TABLE"));
            foreach (var key in keysToDelete)
            {
                shell.Environment.Remove(key);
            }

            shell.SYS_SetCommandMode(CommandMode.Configuration);
        }

        public string GetPrompt(IShell shell, string hostname)
        {
            var revision = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);
            var table = shell.GetEnvironment<string>("CONFIG_FIREWALL_TABLE_NAME");

            return $"{hostname}(config{{{revision:X}}}-firewall{{{table}}})#";
        }

        public string[] Setup(IShell shell, string[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception("Missing table name");
            }

            var revisionId = shell.GetEnvironment<int>(SentinelCommandEnvironment.REVISON_ID);

            var tableName = args[0];

            var firewallTable = firewallTableRepository.GetForRevision(revisionId)
                .FirstOrDefault(ft => ft.Name == tableName);

            if (firewallTable == null)
            {
                firewallTable = new FirewallTable()
                {
                    RevisionId = revisionId,
                    Id = Guid.NewGuid(),
                    Name = tableName,
                    Enabled = true,
                    DefaultAction = FirewallAction.Pass,
                    DefaultLog = false
                };
                firewallTableRepository.Create(firewallTable);
                firewallTableRepository.SaveChanges();
            }

            shell.Environment["CONFIG_FIREWALL_TABLE_ID"] = firewallTable.Id;
            shell.Environment["CONFIG_FIREWALL_TABLE_NAME"] = tableName;

            shell.SYS_SetCommandMode(CommandMode.FirewallTable);
            return args;
        }
    }
}