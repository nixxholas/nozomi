using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r6_DbWideGuidAK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebsocketCommands_Guid",
                table: "WebsocketCommands");

            migrationBuilder.DropIndex(
                name: "IX_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTypes_Guid",
                table: "CurrencyTypes");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyPairs_Guid",
                table: "CurrencyPairs");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Guid",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_Guid",
                table: "AnalysedComponents");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_WebsocketCommands_Guid",
                table: "WebsocketCommands",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RequestComponents_Guid",
                table: "RequestComponents",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrencyTypes_Guid",
                table: "CurrencyTypes",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrencyPairs_Guid",
                table: "CurrencyPairs",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Currencies_Guid",
                table: "Currencies",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AnalysedComponents_Guid",
                table: "AnalysedComponents",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_WebsocketCommands_Guid",
                table: "WebsocketCommands");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RequestComponents_Guid",
                table: "RequestComponents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrencyTypes_Guid",
                table: "CurrencyTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrencyPairs_Guid",
                table: "CurrencyPairs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Currencies_Guid",
                table: "Currencies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AnalysedComponents_Guid",
                table: "AnalysedComponents");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommands_Guid",
                table: "WebsocketCommands",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTypes_Guid",
                table: "CurrencyTypes",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_Guid",
                table: "CurrencyPairs",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Guid",
                table: "Currencies",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_Guid",
                table: "AnalysedComponents",
                column: "Guid",
                unique: true);
        }
    }
}
