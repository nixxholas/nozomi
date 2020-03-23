using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Auth.Migrations
{
    public partial class r4_ApiKeyRevealed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Revealed",
                table: "ApiKeys",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Revealed",
                table: "ApiKeys");
        }
    }
}
