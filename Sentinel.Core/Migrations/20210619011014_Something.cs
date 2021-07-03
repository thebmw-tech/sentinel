using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sentinel.Core.Migrations
{
    public partial class Something : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPv4DefaultGateway",
                table: "SystemConfigurations");

            migrationBuilder.DropColumn(
                name: "IPv6DefaultGateway",
                table: "SystemConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "DHCPHostname",
                table: "SystemConfigurations",
                type: "TEXT",
                maxLength: 63,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vlaninterfaces",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    InterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ParentInterfaceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VlanId = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vlaninterfaces", x => new { x.RevisionId, x.InterfaceName });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vlaninterfaces");

            migrationBuilder.DropColumn(
                name: "DHCPHostname",
                table: "SystemConfigurations");

            migrationBuilder.AddColumn<Guid>(
                name: "IPv4DefaultGateway",
                table: "SystemConfigurations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IPv6DefaultGateway",
                table: "SystemConfigurations",
                type: "TEXT",
                nullable: true);
        }
    }
}
