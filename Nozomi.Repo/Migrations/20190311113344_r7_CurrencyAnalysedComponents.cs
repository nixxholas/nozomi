using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r7_CurrencyAnalysedComponents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RequestId",
                table: "AnalysedComponents",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "AnalysedComponents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Delay",
                table: "AnalysedComponents",
                nullable: false,
                defaultValue: 86400000);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_CurrencyId",
                table: "AnalysedComponents",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "Currency_AnalysedComponents_Constraint",
                table: "AnalysedComponents",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Currency_AnalysedComponents_Constraint",
                table: "AnalysedComponents");

            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_CurrencyId",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "Delay",
                table: "AnalysedComponents");

            migrationBuilder.AlterColumn<long>(
                name: "RequestId",
                table: "AnalysedComponents",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
