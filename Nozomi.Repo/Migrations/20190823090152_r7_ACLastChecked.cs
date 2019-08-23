using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r7_ACLastChecked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChecked",
                table: "AnalysedComponents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChecked",
                table: "AnalysedComponents");
        }
    }
}
