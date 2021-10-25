using Microsoft.EntityFrameworkCore.Migrations;

namespace LementPro.Server.SvcTemplate.Repository.Migrations
{
    public partial class StatusTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "blockWork",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "blockWork",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
