using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core.Command.Attributes;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Helpers;

namespace Sentinel.Core.Command.Commands.Shell
{
    public partial class DatabaseCommand
    {
        [SubCommand("migration", "Run database migrations")]
        public class DatabaseMigrationCommand : BaseCommand
        {
            private readonly SubCommandInterpreter<DatabaseMigrationCommand> subCommandInterpreter;

            public DatabaseMigrationCommand(IShell shell,
                SubCommandInterpreter<DatabaseMigrationCommand> subCommandInterpreter) : base(shell)
            {
                this.subCommandInterpreter = subCommandInterpreter;
            }

            public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
            {
                return subCommandInterpreter.Execute(shell, args, input, output, error);
            }

            public override string Suggest(string[] args)
            {
                return subCommandInterpreter.Suggest(shell, args);
            }

            [SubCommand("apply", "Apply Database Migrations")]
            public class DatabaseMigrationApplyCommand : BaseCommand
            {
                public DatabaseMigrationApplyCommand(IShell shell) : base(shell)
                {

                }

                public override int Main(string[] args, TextReader input, TextWriter output, TextWriter error)
                {
                    HelperFunctions.MigrateDatabase(output);
                    return 0;
                }

                public override string Suggest(string[] args)
                {
                    return "";
                }
            }
        }
    }
}