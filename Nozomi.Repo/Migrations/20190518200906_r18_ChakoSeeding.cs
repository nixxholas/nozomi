using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r18_ChakoSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(850), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(1290) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(2970), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(2970) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 20L, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Litecoin", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 16L, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 18L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 17L, "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Singapore Dollar", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 19L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L });

            migrationBuilder.InsertData(
                table: "CurrencyPairs",
                columns: new[] { "Id", "APIUrl", "CounterCurrency", "CreatedAt", "CreatedBy", "CurrencyPairType", "CurrencySourceId", "DefaultComponent", "DeletedAt", "DeletedBy", "IsEnabled", "MainCurrency", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { 11L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "data/buy_price", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 12L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "data/buy_price", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 13L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "data/buy_price", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 14L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "data/buy_price", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 15L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "data/buy_price", null, 0L, true, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 16L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, 3L, "data/buy_price", null, 0L, true, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("681d252c-cbec-4ed4-beea-96333f292448"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("d8cd2866-daf9-4760-a628-9a713ae80c63"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("aaff1449-0180-4529-9e20-d77b1076350b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("b2da09ea-15a8-46df-9afa-3d60c925599a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("77ded891-e7e0-45ac-a44d-ad8c5dc084fc"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("f54853df-6915-4983-8c03-eaa9b32585a1"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("3944b529-caca-4ca2-ab6f-aca86f553872"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("1fb3fd38-3d81-4fe7-b237-1f5a73fee775"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.alphavantage.co/query", 5000, null, 0L, "CurrencyPairRequest", new Guid("34e3efac-e430-44f3-9940-21f17a07d2ec"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("7c8cc950-28e6-4937-9333-603e6176b5f3"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("90e24972-de5c-4b8b-8c47-5820a6d098ae"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("c5073eac-beb6-4cad-90c7-97119c730bf7"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("efa20d38-3395-432d-a7b7-f58ce4e493d8"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("de988e6e-5a9c-401f-97af-769ca5633e82"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3480), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3480) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3490), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3490) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3490), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3500), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3510), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3520), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530), new DateTime(2019, 5, 18, 20, 9, 5, 817, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.InsertData(
                table: "CurrencyCurrencyPairs",
                columns: new[] { "CurrencyPairId", "CurrencyId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { 11L, 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 14L, 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 15L, 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 13L, 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 16L, 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 13L, 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 12L, 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 12L, 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 14L, 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 11L, 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 16L, 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 15L, 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L }
                });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2060), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2440), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2440) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2440), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2440) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2460) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2480), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2480) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2560) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2570) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("b20ebe25-ec90-4105-8b4a-a0fda713c325"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("6b0efe60-fe8c-459c-98c8-e76d661d7fd2"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 14L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("c8cd8722-b7b0-4e0c-b83c-daac7bc9ddeb"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 15L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("2a563052-8528-4289-bcbd-a92aab7d2ff4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 16L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("3e836a82-b279-41c1-9e55-a75a91617e3f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("7c4b446b-2e9a-4adb-b421-8ef20ab01c12"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 13L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 39L, 2, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, "data/buy_price", 15L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 40L, 1, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, "data/sell_price", 15L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 41L, 2, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, "data/buy_price", 16L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 42L, 1, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), 0L, "data/sell_price", 16L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 43L, 2, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/buy_price", 17L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 44L, 1, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/sell_price", 17L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 45L, 2, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/buy_price", 18L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 46L, 1, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/sell_price", 18L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 47L, 2, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/buy_price", 19L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 48L, 1, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/sell_price", 19L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 49L, 2, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), 0L, "data/buy_price", 20L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 50L, 1, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2600), 0L, null, 0L, null, true, new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2600), 0L, "data/sell_price", 20L, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 11L, 16L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 11L, 17L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 12L, 16L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 12L, 19L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 13L, 17L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 13L, 18L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 14L, 18L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 14L, 19L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 15L, 17L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 15L, 20L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 16L, 19L });

            migrationBuilder.DeleteData(
                table: "CurrencyCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "CurrencyId" },
                keyValues: new object[] { 16L, 20L });

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 50L);

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
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(200), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(620) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2270), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2270) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290) });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("8e030e66-65b5-4f69-81b5-fc73c970113a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("0b7c8889-6758-4770-b81d-7d8fecd7d4f4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("d1c79acc-1bbd-429b-8577-50c0c93cb89b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("b5f20ecc-4a32-4310-84f1-85a6f070a663"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("ac21c563-62d4-4cad-8d6e-5e5ffbe69b05"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("6bdc79e2-b636-4ad4-89b9-dcd5ea681602"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("2380b70a-bbf7-4af5-826a-bfce9bf3cccb"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("b81ad77c-a056-4fc4-afc8-44ec456e562d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("2fe72593-439c-4d8a-b92f-5b18e09ab561"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("1664ed50-f498-48a0-8c93-9183f0e3c36d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2290) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2700), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2700) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2740), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2740) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2810), new DateTime(2019, 5, 18, 19, 27, 38, 86, DateTimeKind.Utc).AddTicks(2810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1410), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1410) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1840) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850), new DateTime(2019, 5, 18, 19, 27, 38, 152, DateTimeKind.Utc).AddTicks(1850) });
        }
    }
}
