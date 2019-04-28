using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r13_Currency_AbbrvSourceUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Currency_Index_Abbrv_CurrencySourceId",
                table: "Currencies",
                columns: new[] { "Abbrv", "CurrencySourceId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Currency_Index_Abbrv_CurrencySourceId",
                table: "Currencies");
        }
    }
}
