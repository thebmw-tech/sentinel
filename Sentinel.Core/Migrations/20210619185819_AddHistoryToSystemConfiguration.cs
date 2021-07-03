using Microsoft.EntityFrameworkCore.Migrations;

namespace Sentinel.Core.Migrations
{
    public partial class AddHistoryToSystemConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "ShellHistoryLength",
                table: "SystemConfigurations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1000u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShellHistoryLength",
                table: "SystemConfigurations");
        }
    }
}
