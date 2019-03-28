using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r6_CurrencyRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyId",
                table: "Requests",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "CurrencyRequest_Currency_Constraint",
                table: "Requests",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "CurrencyRequest_Currency_Constraint",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CurrencyId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Requests");
        }
    }
}
