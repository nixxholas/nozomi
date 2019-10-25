using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r9_CQRS_Base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "Request_AK_Guid",
                table: "Requests");

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

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Sources",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RequestProperties",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RequestComponents",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RcdHistoricItems",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CurrencyTypes",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CurrencySources",
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

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "AnalysedHistoricItems",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "AnalysedComponents",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_WebsocketCommands_Guid",
                table: "WebsocketCommands",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Guid",
                table: "Sources",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Requests_Guid",
                table: "Requests",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RequestProperties_Guid",
                table: "RequestProperties",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RequestComponents_Guid",
                table: "RequestComponents",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RcdHistoricItems_Guid",
                table: "RcdHistoricItems",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrencyTypes_Guid",
                table: "CurrencyTypes",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrencySources_Guid",
                table: "CurrencySources",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrencyPairs_Guid",
                table: "CurrencyPairs",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Currencies_Guid",
                table: "Currencies",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AnalysedHistoricItems_Guid",
                table: "AnalysedHistoricItems",
                column: "Guid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AnalysedComponents_Guid",
                table: "AnalysedComponents",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_WebsocketCommands_Guid",
                table: "WebsocketCommands");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_WebsocketCommandProperties_Guid",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Guid",
                table: "Sources");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Requests_Guid",
                table: "Requests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RequestProperties_Guid",
                table: "RequestProperties");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RequestComponents_Guid",
                table: "RequestComponents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RcdHistoricItems_Guid",
                table: "RcdHistoricItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrencyTypes_Guid",
                table: "CurrencyTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrencySources_Guid",
                table: "CurrencySources");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrencyPairs_Guid",
                table: "CurrencyPairs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Currencies_Guid",
                table: "Currencies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AnalysedHistoricItems_Guid",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AnalysedComponents_Guid",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "AnalysedComponents");

            migrationBuilder.AddUniqueConstraint(
                name: "Request_AK_Guid",
                table: "Requests",
                column: "Guid");
        }
    }
}
