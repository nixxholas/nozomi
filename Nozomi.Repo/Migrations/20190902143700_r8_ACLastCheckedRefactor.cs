using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r8_ACLastCheckedRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChecked",
                table: "AnalysedComponents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChecked",
                table: "AnalysedComponents",
                nullable: true);
        }
    }
}
