using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r11_BNBKNCETHSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComponents_CurrencyPairs_CurrencyPairId",
                table: "RequestComponents");

            migrationBuilder.DropIndex(
                name: "IX_RequestComponents_CurrencyPairId",
                table: "RequestComponents");

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DropColumn(
                name: "CurrencyPairId",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RequestComponents");

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("1532c321-a581-4d9f-be86-c4f3accab3d0"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("5686ee9c-b6ba-494e-b23d-ffbc81f3d4bb"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "APIDocsURL", "Abbreviation", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[] { 3L, "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md", "BNA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Binance" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[,]
                {
                    { 4L, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L, 2L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Kyber Network Coin", 4L },
                    { 5L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L, 2L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", 1L }
                });

            migrationBuilder.InsertData(
                table: "CurrencyPairs",
                columns: new[] { "Id", "APIUrl", "CreatedAt", "CreatedBy", "CurrencyPairType", "CurrencySourceId", "DefaultComponent", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy" },
                values: new object[] { 3L, "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 3L, "askPrice", null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 14, 18, 30, 37, 67, DateTimeKind.Local), new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local), new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local), new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain", "CurrencyId" },
                values: new object[] { 3L, false, 4L });

            migrationBuilder.InsertData(
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain", "CurrencyId" },
                values: new object[] { 3L, true, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", 5000, null, 0L, "CurrencyPairRequest", new Guid("4f5612ae-d74e-484e-bcab-e0698d6e8220"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId" },
                values: new object[] { 4L, 1, new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local), 0L, null, 0L, true, new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local), 0L, "askPrice", 3L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId" },
                values: new object[] { 5L, 2, new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local), 0L, null, 0L, true, new DateTime(2018, 10, 14, 18, 30, 37, 70, DateTimeKind.Local), 0L, "bidPrice", 3L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PartialCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "IsMain" },
                keyValues: new object[] { 3L, false });

            migrationBuilder.DeleteData(
                table: "PartialCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "IsMain" },
                keyValues: new object[] { 3L, true });

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.AddColumn<long>(
                name: "CurrencyPairId",
                table: "RequestComponents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RequestComponents",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("766ef46b-bac3-4c5c-84cb-c62dac85423f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("053037db-7fc0-4863-aa94-bfcb72b7b9a6"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 11, 0, 24, 30, 715, DateTimeKind.Local), new DateTime(2018, 10, 11, 0, 24, 30, 717, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 11, 0, 24, 30, 718, DateTimeKind.Local), new DateTime(2018, 10, 11, 0, 24, 30, 718, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 11, 0, 24, 30, 718, DateTimeKind.Local), new DateTime(2018, 10, 11, 0, 24, 30, 718, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_CurrencyPairId",
                table: "RequestComponents",
                column: "CurrencyPairId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComponents_CurrencyPairs_CurrencyPairId",
                table: "RequestComponents",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
