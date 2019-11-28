using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r14_AnalysedComponentGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "AnalysedComponents",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_Guid",
                table: "AnalysedComponents",
                column: "Guid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_Guid",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "AnalysedComponents");
        }
    }
}
