using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r15_RemainingGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "WebsocketCommands",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "WebsocketCommandProperties",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "SourceTypes",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RequestProperties",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CurrencyProperty",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CurrencyPairs",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Currencies",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommands_Guid",
                table: "WebsocketCommands",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestProperties_Guid",
                table: "RequestProperties",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_Guid",
                table: "CurrencyPairs",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Guid",
                table: "Currencies",
                column: "Guid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebsocketCommands_Guid",
                table: "WebsocketCommands");

            migrationBuilder.DropIndex(
                name: "IX_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropIndex(
                name: "IX_RequestProperties_Guid",
                table: "RequestProperties");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyPairs_Guid",
                table: "CurrencyPairs");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Guid",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Currencies");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "SourceTypes",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "uuid_generate_v4()");
        }
    }
}
