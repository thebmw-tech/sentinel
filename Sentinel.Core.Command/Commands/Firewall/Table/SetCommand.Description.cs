﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.Core.Command.Commands.Firewall.Table
{
    public partial class SetCommand
    {
        [SubCommand("description", "Sets the description")]
        public class SetDescription : BaseCommand
        {
            private readonly IFirewallTableRepository firewallTableRepository;

            public SetDescription(IShell shell, IFirewallTableRepository firewallTableRepository) : base(shell)
            {
                this.firewallTableRepository = firewallTableRepository;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                if (args.Length != 1)
                {
                    error.WriteLine("Missing/Extra Argument(s)");
                    return 1;
                }

                var revisionId = shell.GetEnvironment<int>("CONFIG_REVISION_ID");
                var tableId = shell.GetEnvironment<Guid>("CONFIG_FIREWALL_TABLE_ID");

                firewallTableRepository.Modify(t => t.Id == tableId && t.RevisionId == revisionId, firewallTable =>
                {
                    firewallTable.Description = args[0];
                });
                

                firewallTableRepository.SaveChanges();

                return 0;
            }

            public override string Suggest(string[] args)
            {
                throw new NotImplementedException();
            }
        }
    }
}
