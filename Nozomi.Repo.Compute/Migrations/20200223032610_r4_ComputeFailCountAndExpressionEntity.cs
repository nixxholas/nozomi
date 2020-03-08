using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Compute.Migrations
{
    public partial class r4_ComputeFailCountAndExpressionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFailing",
                table: "Computes");

            migrationBuilder.AddColumn<int>(
                name: "FailCount",
                table: "Computes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ComputeExpressions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ComputeExpressions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ComputeExpressions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "ComputeExpressions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ComputeExpressions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ComputeExpressions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "ComputeExpressions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailCount",
                table: "Computes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ComputeExpressions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ComputeExpressions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ComputeExpressions");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "ComputeExpressions");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ComputeExpressions");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ComputeExpressions");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ComputeExpressions");

            migrationBuilder.AddColumn<bool>(
                name: "IsFailing",
                table: "Computes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
