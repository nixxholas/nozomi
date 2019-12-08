using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r9_EntityRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AnalysedComponents");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "WebsocketCommands",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "WebsocketCommands",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "WebsocketCommands",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "WebsocketCommandProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "WebsocketCommandProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "WebsocketCommandProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Sources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Sources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Sources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "RequestProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "RequestProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "RequestProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "RequestComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "RequestComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "RequestComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "RcdHistoricItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "RcdHistoricItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "RcdHistoricItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CurrencyTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "CurrencyTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "CurrencyTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CurrencySources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "CurrencySources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "CurrencySources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CurrencyProperty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "CurrencyProperty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "CurrencyProperty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "CurrencyPairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Currencies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Currencies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Currencies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "AnalysedHistoricItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "AnalysedHistoricItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "AnalysedHistoricItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "AnalysedComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "AnalysedComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "AnalysedComponents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CurrencyProperty");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "AnalysedComponents");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "AnalysedComponents");

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "WebsocketCommands",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "WebsocketCommands",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WebsocketCommands",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "WebsocketCommandProperties",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "WebsocketCommandProperties",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "WebsocketCommandProperties",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Sources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "Sources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Sources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Requests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "Requests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Requests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "RequestProperties",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "RequestProperties",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "RequestProperties",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "RequestComponents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "RequestComponents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "RequestComponents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "RcdHistoricItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "RcdHistoricItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "RcdHistoricItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "CurrencyTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "CurrencyTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "CurrencyTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "CurrencySources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "CurrencySources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "CurrencySources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "CurrencyProperty",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "CurrencyProperty",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "CurrencyProperty",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "CurrencyPairs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "CurrencyPairs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "CurrencyPairs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Currencies",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "Currencies",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "Currencies",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "AnalysedHistoricItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "AnalysedHistoricItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "AnalysedHistoricItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "AnalysedComponents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "AnalysedComponents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                table: "AnalysedComponents",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
