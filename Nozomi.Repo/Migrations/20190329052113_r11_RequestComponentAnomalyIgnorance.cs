using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r11_RequestComponentAnomalyIgnorance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDenominated",
                table: "RequestComponents",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<bool>(
                name: "AnomalyIgnorance",
                table: "RequestComponents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnomalyIgnorance",
                table: "RequestComponents");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDenominated",
                table: "RequestComponents",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }
    }
}
