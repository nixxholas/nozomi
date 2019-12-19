using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r16_CurrencyPairRenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "CurrencyPair_AK_MainCurrency_CounterCurrency_Source",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "CounterCurrencyAbbrv",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "MainCurrencyAbbrv",
                table: "CurrencyPairs");

            migrationBuilder.AddColumn<string>(
                name: "CounterTicker",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainTicker",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_MainTicker_CounterTicker_SourceId",
                table: "CurrencyPairs",
                columns: new[] { "MainTicker", "CounterTicker", "SourceId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CurrencyPairs_MainTicker_CounterTicker_SourceId",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "CounterTicker",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "MainTicker",
                table: "CurrencyPairs");

            migrationBuilder.AddColumn<string>(
                name: "CounterCurrencyAbbrv",
                table: "CurrencyPairs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCurrencyAbbrv",
                table: "CurrencyPairs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "CurrencyPair_AK_MainCurrency_CounterCurrency_Source",
                table: "CurrencyPairs",
                columns: new[] { "MainCurrencyAbbrv", "CounterCurrencyAbbrv", "SourceId" });
        }
    }
}
