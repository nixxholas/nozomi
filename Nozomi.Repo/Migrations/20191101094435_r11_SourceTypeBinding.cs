using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r11_SourceTypeBinding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_SourceTypes_Guid",
                table: "SourceTypes");

            migrationBuilder.AddColumn<long>(
                name: "SourceTypeId",
                table: "Sources",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.InsertData(
                table: "SourceTypes",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "Guid", "IsEnabled", "ModifiedAt", "ModifiedById", "Name" },
                values: new object[] { 1L, "UNK", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new Guid("05b6457d-059c-458c-8774-0811e4d59ea8"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Unknown" });

            migrationBuilder.CreateIndex(
                name: "IX_SourceTypes_Guid",
                table: "SourceTypes",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceTypeId",
                table: "Sources",
                column: "SourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_SourceTypes_SourceTypeId",
                table: "Sources",
                column: "SourceTypeId",
                principalTable: "SourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SourceTypes_SourceTypeId",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_SourceTypes_Guid",
                table: "SourceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Sources_SourceTypeId",
                table: "Sources");

            migrationBuilder.DeleteData(
                table: "SourceTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "SourceTypeId",
                table: "Sources");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SourceTypes_Guid",
                table: "SourceTypes",
                column: "Guid");
        }
    }
}
