using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r14_CurrencyCurrencyPair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartialCurrencyPairs");

            migrationBuilder.AddColumn<string>(
                name: "CounterCurrency",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainCurrency",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyCurrencyPairs",
                columns: table => new
                {
                    CurrencyId = table.Column<long>(nullable: false),
                    CurrencyPairId = table.Column<long>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CurrencyCurrencyPair_CK_CurrencyPairId_CurrencyId", x => new { x.CurrencyPairId, x.CurrencyId });
                    table.ForeignKey(
                        name: "CurrencyCurrencyPairs_Currency_Constraint",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PartialCurrencyPairs_CurrencyPair_Constraint",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCurrencyPairs_CurrencyId",
                table: "CurrencyCurrencyPairs",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyCurrencyPairs");

            migrationBuilder.DropColumn(
                name: "CounterCurrency",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "MainCurrency",
                table: "CurrencyPairs");

            migrationBuilder.CreateTable(
                name: "PartialCurrencyPairs",
                columns: table => new
                {
                    CurrencyPairId = table.Column<long>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false),
                    CurrencyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PartialCurrencyPair_CK_CurrencyPairId_IsMain", x => new { x.CurrencyPairId, x.IsMain });
                    table.ForeignKey(
                        name: "PartialCurrencyPairs_Currency_Constraint",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PartialCurrencyPairs_CurrencyPair_Constraint",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartialCurrencyPairs_CurrencyId",
                table: "PartialCurrencyPairs",
                column: "CurrencyId");
        }
    }
}
