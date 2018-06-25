using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_CurrencyPairBinding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrencyPairId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyPairId",
                table: "Requests",
                column: "CurrencyPairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrencyPairId",
                table: "Requests");
        }
    }
}
