using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r3_RequestPropertyGuidHotfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestProperties_Guid",
                table: "RequestProperties");

            migrationBuilder.AddUniqueConstraint(
                name: "RequestProperty_AK_Guid",
                table: "RequestProperties",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "RequestProperty_AK_Guid",
                table: "RequestProperties");

            migrationBuilder.CreateIndex(
                name: "IX_RequestProperties_Guid",
                table: "RequestProperties",
                column: "Guid",
                unique: true);
        }
    }
}
