using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Identity.Migrations
{
    public partial class r7_AnalysedComponentDelay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 3, 7, 22, 32, 59, 972, DateTimeKind.Utc).AddTicks(8810),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 18, 8, 17, 56, 815, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 3, 7, 22, 32, 59, 972, DateTimeKind.Utc).AddTicks(8190),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 18, 8, 17, 56, 814, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccessed",
                table: "ApiTokens",
                nullable: false,
                defaultValue: new DateTime(2019, 3, 8, 6, 32, 59, 964, DateTimeKind.Local).AddTicks(8070),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 18, 16, 17, 56, 810, DateTimeKind.Local).AddTicks(110));

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "ApiTokens",
                nullable: false,
                defaultValue: new Guid("b8e9157a-564f-416a-85b7-c2081f1c14ee"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("8a2a6ee9-c841-4aa9-ad94-2f30851ebd27"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 18, 8, 17, 56, 815, DateTimeKind.Utc).AddTicks(70),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 3, 7, 22, 32, 59, 972, DateTimeKind.Utc).AddTicks(8810));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 18, 8, 17, 56, 814, DateTimeKind.Utc).AddTicks(9620),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 3, 7, 22, 32, 59, 972, DateTimeKind.Utc).AddTicks(8190));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccessed",
                table: "ApiTokens",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 18, 16, 17, 56, 810, DateTimeKind.Local).AddTicks(110),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 3, 8, 6, 32, 59, 964, DateTimeKind.Local).AddTicks(8070));

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "ApiTokens",
                nullable: false,
                defaultValue: new Guid("8a2a6ee9-c841-4aa9-ad94-2f30851ebd27"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("b8e9157a-564f-416a-85b7-c2081f1c14ee"));
        }
    }
}
