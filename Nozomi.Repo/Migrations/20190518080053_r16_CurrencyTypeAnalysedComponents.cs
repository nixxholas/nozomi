using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r16_CurrencyTypeAnalysedComponents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrencyTypeId",
                table: "AnalysedComponents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_CurrencyTypeId",
                table: "AnalysedComponents",
                column: "CurrencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "CurrencyType_AnalysedComponents_Constraint",
                table: "AnalysedComponents",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "CurrencyType_AnalysedComponents_Constraint",
                table: "AnalysedComponents");

            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_CurrencyTypeId",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                table: "AnalysedComponents");
        }
    }
}
