using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sentinel.Core.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DestinationNatRules",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InboundInterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IPVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Protocol = table.Column<int>(type: "INTEGER", nullable: false),
                    InvertSourceMatch = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    SourceAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    SourceSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    SourcePortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    SourcePortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true),
                    InvertDestinationMatch = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DestinationAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    DestinationSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    DestinationPortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    DestinationPortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true),
                    Log = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TranslationAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    TranslationSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    TranslationPortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    TranslationPortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationNatRules", x => new { x.RevisionId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "FirewallRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirewallTableId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Action = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IPVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Protocol = table.Column<int>(type: "INTEGER", nullable: false),
                    InvertSourceMatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    SourceAddress = table.Column<string>(type: "TEXT", nullable: true),
                    SourceSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    SourcePortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    SourcePortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true),
                    InvertDestinationMatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    DestinationAddress = table.Column<string>(type: "TEXT", nullable: true),
                    DestinationSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    DestinationPortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    DestinationPortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true),
                    Log = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirewallRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FirewallTables",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultAction = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 28, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DefaultLog = table.Column<bool>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirewallTables", x => new { x.RevisionId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "InterfaceAddresses",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    InterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AddressConfigurationType = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Address = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    SubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterfaceAddresses", x => new { x.RevisionId, x.InterfaceName, x.AddressConfigurationType, x.Address });
                });

            migrationBuilder.CreateTable(
                name: "Interfaces",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    InterfaceType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    SpoofedMAC = table.Column<long>(type: "INTEGER", nullable: true),
                    InboundFirewallTableId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OutboundFirewallTableId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LocalFirewallTableId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interfaces", x => new { x.RevisionId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Revisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    CommitDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConfirmDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Locked = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HasChanges = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    SubnetMask = table.Column<byte>(type: "INTEGER", nullable: false),
                    RouteType = table.Column<int>(type: "INTEGER", nullable: false),
                    InterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    NextHopAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => new { x.RevisionId, x.Address, x.SubnetMask });
                });

            migrationBuilder.CreateTable(
                name: "SourceNatRules",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OutboundInterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IPVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Protocol = table.Column<int>(type: "INTEGER", nullable: false),
                    InvertSourceMatch = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    SourceAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    SourceSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    SourcePortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    SourcePortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true),
                    InvertDestinationMatch = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DestinationAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    DestinationSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    DestinationPortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    DestinationPortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true),
                    Log = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TranslationAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    TranslationSubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    TranslationPortRangeStart = table.Column<ushort>(type: "INTEGER", nullable: true),
                    TranslationPortRangeEnd = table.Column<ushort>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceNatRules", x => new { x.RevisionId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigurations",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", maxLength: 63, nullable: false),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    IPv4DefaultGateway = table.Column<Guid>(type: "TEXT", nullable: true),
                    IPv6DefaultGateway = table.Column<Guid>(type: "TEXT", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigurations", x => x.RevisionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinationNatRules_Order",
                table: "DestinationNatRules",
                column: "Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FirewallRules_FirewallTableId_Order",
                table: "FirewallRules",
                columns: new[] { "FirewallTableId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_CommitDate",
                table: "Revisions",
                column: "CommitDate");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_ConfirmDate",
                table: "Revisions",
                column: "ConfirmDate");

            migrationBuilder.CreateIndex(
                name: "IX_SourceNatRules_Order",
                table: "SourceNatRules",
                column: "Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestinationNatRules");

            migrationBuilder.DropTable(
                name: "FirewallRules");

            migrationBuilder.DropTable(
                name: "FirewallTables");

            migrationBuilder.DropTable(
                name: "InterfaceAddresses");

            migrationBuilder.DropTable(
                name: "Interfaces");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "SourceNatRules");

            migrationBuilder.DropTable(
                name: "SystemConfigurations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
