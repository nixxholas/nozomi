using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r17_HasDataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WebsocketCommands",
                table: "WebsocketCommands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebsocketCommandProperties",
                table: "WebsocketCommandProperties");

            migrationBuilder.AlterColumn<long>(
                name: "Delay",
                table: "WebsocketCommands",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "WebsocketCommandProperties",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "WebsocketCommandProperties",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "WebsocketCommand_PK_Id",
                table: "WebsocketCommands",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "WebsocketCommandProperty_PK_Id",
                table: "WebsocketCommandProperties",
                column: "Id");

            migrationBuilder.InsertData(
                table: "CurrencyTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "TypeShortForm" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "FIAT Cash", "FIAT" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Cryptocurrency", "CRYPTO" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "APIDocsURL", "Abbreviation", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1L, "https://docs.bitfinex.com/docs/introduction", "BFX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitfinex" },
                    { 2L, "None", "HAKO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Coinhako" },
                    { 3L, "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md", "BNA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Binance" },
                    { 4L, "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html", "ECB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "European Central Bank" },
                    { 5L, "https://www.alphavantage.co/documentation/", "AVG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "AlphaVantage" },
                    { 6L, "https://docs.poloniex.com/#public-http-api-methods", "POLO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Poloniex" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 1L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 15L, "USDT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Tether USD", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 14L, "BTS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "BitShares", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 13L, "BCN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bytecoin", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Denominations", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[,]
                {
                    { 12L, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L, 2L, null, 0L, "Sat", 8, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin", 0L },
                    { 11L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 5L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L },
                    { 10L, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 5L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Euro", 0L },
                    { 9L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 4L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L },
                    { 7L, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L, 2L, null, 0L, "Sat", 8, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin", 0L },
                    { 8L, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 4L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Euro", 0L },
                    { 5L, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L, 2L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Kyber Network Crystal", 4L },
                    { 4L, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 2L, null, 0L, null, 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Kyber Network Crystal", 4L },
                    { 3L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 2L, null, 0L, "Wei", 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", 1L },
                    { 2L, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Euro", 0L },
                    { 6L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L, 2L, null, 0L, "Wei", 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", 1L }
                });

            migrationBuilder.InsertData(
                table: "CurrencyPairs",
                columns: new[] { "Id", "APIUrl", "CounterCurrency", "CreatedAt", "CreatedBy", "CurrencyPairType", "CurrencySourceId", "DefaultComponent", "DeletedAt", "DeletedBy", "IsEnabled", "MainCurrency", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { 9L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "b", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 10L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "b", null, 0L, true, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 6L, "https://poloniex.com/public?command=returnTicker", "BTS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 6L, "BTC_BTS/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 3L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 4L, "Cube", null, 0L, true, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 7L, "https://api.bitfinex.com/v1/pubticker/etheur", "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 1L, "0", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 2L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 1L, "0", null, 0L, true, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 4L, "https://www.alphavantage.co/query", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 5L, "Realtime Currency Exchange Rate/5. Exchange Rate", null, 0L, true, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 1L, "https://api.ethfinex.com/v2/ticker/tETHUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 1L, "0", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 5L, "https://poloniex.com/public?command=returnTicker", "BCN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 6L, "BTC_BCN/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 8L, "https://poloniex.com/public?command=returnTicker", "USDT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 6L, "USDT_BTC/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L }
                });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestId", "UIFormatting", "Value" },
                values: new object[,]
                {
                    { 1L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(200), 0L, 3L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(620), 0L, null, "$ 0 a", null },
                    { 2L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2270), 0L, 4L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2270), 0L, null, "$ 0 a", null },
                    { 3L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290), 0L, 12L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290), 0L, null, "$ 0 a", null }
                });

            migrationBuilder.InsertData(
                table: "CurrencyCurrencyPairs",
                columns: new[] { "CurrencyPairId", "CurrencyId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { 9L, 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 3L, 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 3L, 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 8L, 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 10L, 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 10L, 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 5L, 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 8L, 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 9L, 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 4L, 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 7L, 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 7L, 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 6L, 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 2L, 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 2L, 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 6L, 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 1L, 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 1L, 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 5L, 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 4L, 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("ac21c563-62d4-4cad-8d6e-5e5ffbe69b05"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("8e030e66-65b5-4f69-81b5-fc73c970113a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("6bdc79e2-b636-4ad4-89b9-dcd5ea681602"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("1664ed50-f498-48a0-8c93-9183f0e3c36d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.alphavantage.co/query", 5000, null, 0L, "CurrencyPairRequest", new Guid("eea5e0e3-2ddd-4cc4-9707-dde22af8746f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("55e611e5-f944-49a4-85e3-5cb849142650"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("2fe72593-439c-4d8a-b92f-5b18e09ab561"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("b5f20ecc-4a32-4310-84f1-85a6f070a663"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("2380b70a-bbf7-4af5-826a-bfce9bf3cccb"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("b81ad77c-a056-4fc4-afc8-44ec456e562d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("0f986d48-c2b3-43e0-8567-25377cea27b4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("98671b8d-2c1a-4482-9cc2-f959633f24b9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("0b7c8889-6758-4770-b81d-7d8fecd7d4f4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("d1c79acc-1bbd-429b-8577-50c0c93cb89b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestId", "UIFormatting", "Value" },
                values: new object[,]
                {
                    { 9L, 10, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, 6L, "$ 0[.]00", null },
                    { 21L, 10, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 11L, "0[.]00", null },
                    { 20L, 80, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 11L, "0 a", null },
                    { 12L, 80, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 9L, "0 a", null },
                    { 13L, 10, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 9L, "0[.]00", null },
                    { 14L, 11, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 9L, "0[.]00", null },
                    { 15L, 70, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 9L, "0[.]0", null },
                    { 11L, 70, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, 6L, "0[.]0", null },
                    { 10L, 11, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, 6L, "$ 0[.]00", null },
                    { 8L, 80, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, null, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, 6L, "0 a", null },
                    { 22L, 11, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 11L, "0[.]00", null },
                    { 17L, 10, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 10L, "0[.]00", null },
                    { 18L, 11, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 10L, "0[.]00", null },
                    { 16L, 80, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, null, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), 0L, 10L, "0 a", null },
                    { 6L, 11, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, 5L, "$ 0[.]00", null },
                    { 27L, 70, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2810), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2810), 0L, 12L, "0[.]0", null },
                    { 26L, 11, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2740), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2740), 0L, 12L, "$ 0[.]00", null },
                    { 25L, 10, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 12L, "$ 0[.]00", null },
                    { 24L, 80, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 12L, "0 a", null },
                    { 7L, 70, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), 0L, 5L, "0[.]0", null },
                    { 23L, 70, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 11L, "0[.]0", null },
                    { 4L, 80, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290), 0L, null, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290), 0L, 5L, "$ 0 a", null },
                    { 5L, 10, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2700), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2700), 0L, 5L, "$ 0[.]00", null },
                    { 19L, 70, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), 0L, 10L, "0[.]0", null }
                });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 16L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "gesmes:Envelope/Cube/Cube/Cube/0=>@rate", 7L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 4L, 1010, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, "info/difficulty", 3L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 38L, 5, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, "B", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 37L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, "b", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 3L, 1005, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, "info/blocks", 3L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 17L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "['Realtime Currency Exchange Rate']/['5. Exchange Rate']", 8L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 5L, 1000, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, "data/coin/circulatingSupply", 4L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 1L, 1000, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1410), 0L, null, 0L, null, true, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1410), 0L, "result", 1L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 36L, 8, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, "A", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 19L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "BTC_BCN/lowestAsk", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 20L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "BTC_BCN/highestBid", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 21L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "BTC_BTS/baseVolume", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 22L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "BTC_BTS/lowestAsk", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 23L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "BTC_BTS/highestBid", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 18L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "BTC_BCN/baseVolume", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 35L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), 0L, "a", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 31L, 8, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "A", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 33L, 5, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "B", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 2L, 1000, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), 0L, "result", 2L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 6L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "7", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 7L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "2", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 8L, 8, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "3", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 9L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "0", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 10L, 5, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "1", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 11L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "7", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 34L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "v", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 13L, 8, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "3", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 14L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "0", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 15L, 5, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), 0L, "1", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 12L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), 0L, "2", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 25L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "ask", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 32L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "b", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 24L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "volume", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 27L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "USDT_BTC/lowestAsk", 12L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 30L, 1, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "a", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 29L, 12, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "v", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 28L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), 0L, "USDT_BTC/highestBid", 12L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 26L, 2, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), 0L, "bid", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestProperties",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "Key", "ModifiedAt", "ModifiedBy", "RequestId", "RequestPropertyType", "Value" },
                values: new object[,]
                {
                    { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "function", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "CURRENCY_EXCHANGE_RATE" },
                    { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "from_currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "EUR" },
                    { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "to_currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "USD" },
                    { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "contractaddress", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "0xdd974d5c2e2928dea5f71b9825b8b646686bd200" },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "action", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "tokensupply" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "module", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "stats" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "apikey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 32, "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "action", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 32, "ethsupply" },
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "module", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 32, "stats" },
                    { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "apikey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "TV5HJJHNP8094BRO" },
                    { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "apikey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "WebsocketCommand_PK_Id",
                table: "WebsocketCommands");

            migrationBuilder.DropPrimaryKey(
                name: "WebsocketCommandProperty_PK_Id",
                table: "WebsocketCommandProperties");

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 1L, 3L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 2L, 1L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 2L, 4L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 3L, 8L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 3L, 9L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 4L, 10L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 4L, 11L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 5L, 12L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 5L, 13L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 6L, 12L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 6L, 14L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 7L, 2L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 7L, 3L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 8L, 12L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 8L, 15L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 9L, 6L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 9L, 7L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 10L, 5L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 10L, 6L });

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "RequestProperties",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.AlterColumn<long>(
                name: "Delay",
                table: "WebsocketCommands",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "WebsocketCommandProperties",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "WebsocketCommandProperties",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebsocketCommands",
                table: "WebsocketCommands",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebsocketCommandProperties",
                table: "WebsocketCommandProperties",
                column: "Id");
        }
    }
}
