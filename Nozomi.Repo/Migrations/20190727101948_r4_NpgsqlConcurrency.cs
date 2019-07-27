using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r4_NpgsqlConcurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "WebsocketCommands",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "WebsocketCommandProperties",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Sources",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Requests",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RequestProperties",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RequestComponents",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RcdHistoricItems",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CurrencyTypes",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CurrencySources",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CurrencyPairs",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Currencies",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AnalysedHistoricItems",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AnalysedComponents",
                type: "xid",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "WebsocketCommands");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "WebsocketCommandProperties");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CurrencySources");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AnalysedHistoricItems");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AnalysedComponents");
        }
    }
}
