using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Compute.Migrations
{
    public partial class r2_ComputeDelay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Delay",
                table: "Computes",
                nullable: false,
                defaultValue: 5000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delay",
                table: "Computes");
        }
    }
}
