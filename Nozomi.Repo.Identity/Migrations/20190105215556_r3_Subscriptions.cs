using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Identity.Migrations
{
    public partial class r3_Subscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevKey_Users_UserId",
                table: "DevKey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevKey",
                table: "DevKey");

            migrationBuilder.RenameTable(
                name: "DevKey",
                newName: "DevKeys");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DevKeys",
                newName: "UserSubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_DevKey_UserId",
                table: "DevKeys",
                newName: "IX_DevKeys_UserSubscriptionId");

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSourceId",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(7850),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "DevKeys",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKeys",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(4500),
                oldClrType: typeof(DateTime));

            migrationBuilder.AddPrimaryKey(
                name: "DevKey_PK_Id",
                table: "DevKeys",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    SubscriptionId = table.Column<string>(nullable: true),
                    PlanType = table.Column<int>(nullable: false, defaultValue: 1),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserSubscription_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UserSubscription_Index_UserId_DeletedAt",
                table: "UserSubscriptions",
                columns: new[] { "UserId", "DeletedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserSubscription_Index_UserId_SubscriptionId",
                table: "UserSubscriptions",
                columns: new[] { "UserId", "SubscriptionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DevKeys_UserSubscriptions_UserSubscriptionId",
                table: "DevKeys",
                column: "UserSubscriptionId",
                principalTable: "UserSubscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevKeys_UserSubscriptions_UserSubscriptionId",
                table: "DevKeys");

            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "DevKey_PK_Id",
                table: "DevKeys");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StripeSourceId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "DevKeys",
                newName: "DevKey");

            migrationBuilder.RenameColumn(
                name: "UserSubscriptionId",
                table: "DevKey",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DevKeys_UserSubscriptionId",
                table: "DevKey",
                newName: "IX_DevKey_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "DevKey",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "DevKey",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevKey",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 5, 21, 55, 56, 681, DateTimeKind.Utc).AddTicks(4500));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevKey",
                table: "DevKey",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DevKey_Users_UserId",
                table: "DevKey",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
