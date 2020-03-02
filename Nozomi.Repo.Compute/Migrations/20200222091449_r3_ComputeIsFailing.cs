using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Compute.Migrations
{
    public partial class r3_ComputeIsFailing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFailing",
                table: "Computes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFailing",
                table: "Computes");
        }
    }
}
