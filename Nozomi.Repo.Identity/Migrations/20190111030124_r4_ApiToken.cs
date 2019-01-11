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
                defaultValue: new DateTime(2019, 1, 11, 3, 1, 24, 586, DateTimeKind.Utc).AddTicks(2560),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 11, 3, 1, 24, 586, DateTimeKind.Utc).AddTicks(1980),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(4500));

            migrationBuilder.CreateTable(
                name: "ApiTokens",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValue: new Guid("347e2220-70eb-4201-9da4-f7f071d20547")),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LastAccessed = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 11, 11, 1, 24, 580, DateTimeKind.Local).AddTicks(2710)),
                    Secret = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
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
                oldDefaultValue: new DateTime(2019, 1, 11, 3, 1, 24, 586, DateTimeKind.Utc).AddTicks(2560));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(4500),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 11, 3, 1, 24, 586, DateTimeKind.Utc).AddTicks(1980));
        }
    }
}
