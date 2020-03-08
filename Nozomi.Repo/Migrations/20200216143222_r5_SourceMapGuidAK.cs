using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r5_SourceMapGuidAK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sources_Guid",
                table: "Sources");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Guid",
                table: "Sources",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Guid",
                table: "Sources");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Guid",
                table: "Sources",
                column: "Guid",
                unique: true);
        }
    }
}
