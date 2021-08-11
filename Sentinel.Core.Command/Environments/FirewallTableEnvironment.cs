using System;
using System.Linq;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Entities;
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
            throw new System.NotImplementedException();
        }

        public string[] Setup(IShell shell, string[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception("Missing firewall table name");
            }

            var tableName = args[0];

            var table = firewallTableRepository.Find(t => t.Name == tableName);

            if (table == null)
            {
                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");

                table = new FirewallTable()
                {
                    RevisionId = revisionId,
                    Name = tableName
                };

                firewallTableRepository.Create(table);
                firewallTableRepository.SaveChanges();
            }

            shell.Environment["CONFIG_FWTABLE_ID"] = table.Id;
            shell.Environment["CONFIG_FWTABLE_NAME"] = tableName;

            shell.SYS_SetCommandMode(CommandMode.FirewallTable);

            return args.Skip(1).ToArray();
        }
    }
}