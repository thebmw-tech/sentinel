using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sentinel.Core.Migrations
{
    public partial class Initial_Interface_Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gateways",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GatewayType = table.Column<int>(type: "INTEGER", nullable: false),
                    InterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IPAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    IPVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateways", x => new { x.RevisionId, x.Id });
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
                    IPv4ConfigurationType = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    IPv4Address = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    IPv4SubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    IPv4GatewayId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IPv6ConfigurationType = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    IPv6Address = table.Column<string>(type: "TEXT", maxLength: 39, nullable: true),
                    IPv6SubnetMask = table.Column<byte>(type: "INTEGER", nullable: true),
                    IPv6GatewayId = table.Column<Guid>(type: "TEXT", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CommitDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConfirmDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    GatewayId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => new { x.RevisionId, x.Address, x.SubnetMask });
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigurations",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", maxLength: 63, nullable: false),
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
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gateways_InterfaceName",
                table: "Gateways",
                column: "InterfaceName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gateways");

            migrationBuilder.DropTable(
                name: "Interfaces");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "SystemConfigurations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
