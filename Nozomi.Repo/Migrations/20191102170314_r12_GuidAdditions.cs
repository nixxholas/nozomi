using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r12_GuidAdditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Sources",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CurrencyTypes",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Guid",
                table: "Sources",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTypes_Guid",
                table: "CurrencyTypes",
                column: "Guid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sources_Guid",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTypes_Guid",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencyTypes");
        }
    }
}
