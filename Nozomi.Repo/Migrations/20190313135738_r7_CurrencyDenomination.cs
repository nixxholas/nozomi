using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r7_CurrencyDenomination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DenominationName",
                table: "Currencies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Denominations",
                table: "Currencies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DenominationName",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Denominations",
                table: "Currencies");
        }
    }
}
