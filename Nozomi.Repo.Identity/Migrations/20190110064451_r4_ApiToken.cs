using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Identity.Migrations
{
    public partial class r4_ApiToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PlanType",
                table: "UserSubscriptions",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 10, 6, 44, 50, 982, DateTimeKind.Utc).AddTicks(1410),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 10, 6, 44, 50, 982, DateTimeKind.Utc).AddTicks(810),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(4500));

            migrationBuilder.CreateTable(
                name: "ApiTokens",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValue: new Guid("7bd9669c-eaf0-40dc-bb9c-74929b170dc0")),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LastAccessed = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 10, 14, 44, 50, 975, DateTimeKind.Local).AddTicks(2140)),
                    PublicKey = table.Column<string>(nullable: false),
                    ApiKey = table.Column<string>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ApiToken_PK_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ApiTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiTokens_UserId",
                table: "ApiTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiTokens");

            migrationBuilder.AlterColumn<int>(
                name: "PlanType",
                table: "UserSubscriptions",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(7850),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 10, 6, 44, 50, 982, DateTimeKind.Utc).AddTicks(1410));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(4500),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 10, 6, 44, 50, 982, DateTimeKind.Utc).AddTicks(810));
        }
    }
}
