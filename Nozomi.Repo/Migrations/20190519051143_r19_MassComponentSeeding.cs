using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r19_MassComponentSeeding : Migration
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
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(1180), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(1620) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330) });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestId", "UIFormatting", "Value" },
                values: new object[,]
                {
                    { 55L, 1, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, null, 2L, 3000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, null, "$ 0 a", null },
                    { 54L, 21, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3340), 0L, 12L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3340), 0L, null, "$ 0 a", null },
                    { 52L, 6, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), 0L, 12L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), 0L, null, "$ 0 a", null },
                    { 51L, 21, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), 0L, 4L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), 0L, null, "$ 0 a", null },
                    { 53L, 20, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), 0L, 12L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3340), 0L, null, "$ 0 a", null },
                    { 49L, 6, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, 4L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, null, "$ 0 a", null },
                    { 48L, 21, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, 3L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, null, "$ 0 a", null },
                    { 47L, 20, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, 3L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, null, "$ 0 a", null },
                    { 46L, 6, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3300), 0L, 3L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3300), 0L, null, "$ 0 a", null },
                    { 50L, 20, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3320), 0L, 4L, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3330), 0L, null, "$ 0 a", null }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("7d93ce48-9a2f-4925-8052-0a52bdf1305e"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 13L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("53a5e431-c4c1-45f1-8464-996e050554d4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("96e148b2-fc80-4706-93fb-0aa1dd441a6d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("9155fc83-d942-4355-b95c-304831a2bf95"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("099139b2-6dc0-48e2-8622-4b5b969d118f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("944771b4-ff5d-463a-bee5-16a9ada4f237"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 16L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("6fdf6c0d-c724-48bb-8a9f-e6b8152018e0"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 15L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("6e045a60-e8d6-45e3-badf-0836fa1d74d8"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 14L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("11561bef-45a1-46ca-bc5c-22febb559496"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.alphavantage.co/query", 5000, null, 0L, "CurrencyPairRequest", new Guid("8f1a664e-8548-4a6e-9461-ef27ae6f3e72"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("b55bce5c-089a-41fb-a4d5-bd4c2cab571f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("9e75f923-8fd7-4dee-a8b4-d14fc1327e0a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("fc5cd0b8-d8a3-4789-a701-242d49d252fb"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("5eefd5ab-c7ab-4225-b0fe-8b7b8ecf73d7"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("f10e5d15-6335-4761-95f8-4f2cc6c3e365"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("f8931765-b6c0-453d-a3d3-fb46484b6811"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("e0e78af1-f35d-427b-b671-d2f70fa0dcd2"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("6b859ba6-7181-452d-b602-783fd56fc83d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("63163df8-8629-4015-af42-4bb797c1056e"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("1fa3018c-a29b-4773-8ccc-372beb4f1783"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3340), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3340) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3750), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3750) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3760), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3760) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3770), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3770) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3770), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3770) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3770), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3770) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestId", "UIFormatting", "Value" },
                values: new object[,]
                {
                    { 33L, 70, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3990), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3990), 0L, 16L, "0[.]0", null },
                    { 42L, 70, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, 19L, "0[.]0", null },
                    { 39L, 70, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, 18L, "0[.]0", null },
                    { 38L, 11, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, 18L, "$ 0[.]00", null },
                    { 37L, 10, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4000), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4000), 0L, 18L, "$ 0[.]00", null },
                    { 28L, 10, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, 15L, "$ 0[.]00", null },
                    { 43L, 10, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, 20L, "$ 0[.]00", null },
                    { 36L, 70, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4000), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4000), 0L, 17L, "0[.]0", null },
                    { 35L, 11, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4000), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4000), 0L, 17L, "$ 0[.]00", null },
                    { 29L, 11, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, 15L, "$ 0[.]00", null },
                    { 30L, 70, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, 15L, "0[.]0", null },
                    { 31L, 10, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3820), 0L, 16L, "$ 0[.]00", null },
                    { 34L, 10, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3990), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3990), 0L, 17L, "$ 0[.]00", null },
                    { 45L, 70, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, 20L, "0[.]0", null },
                    { 44L, 11, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4020), 0L, 20L, "$ 0[.]00", null },
                    { 32L, 11, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3990), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(3990), 0L, 16L, "$ 0[.]00", null },
                    { 40L, 10, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, 19L, "$ 0[.]00", null },
                    { 41L, 11, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, null, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 19, 5, 11, 42, 435, DateTimeKind.Utc).AddTicks(4010), 0L, 19L, "$ 0[.]00", null }
                });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3380), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3380) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3770), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3770) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3780), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3810), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3820) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3830), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3830) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3910), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3910), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3940), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3940) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3940), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3940) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3940), new DateTime(2019, 5, 19, 5, 11, 42, 504, DateTimeKind.Utc).AddTicks(3940) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 55L);

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
                values: new object[] { 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("2a563052-8528-4289-bcbd-a92aab7d2ff4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 16L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("c8cd8722-b7b0-4e0c-b83c-daac7bc9ddeb"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 15L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("6b0efe60-fe8c-459c-98c8-e76d661d7fd2"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 14L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("7c4b446b-2e9a-4adb-b421-8ef20ab01c12"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 13L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("b20ebe25-ec90-4105-8b4a-a0fda713c325"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("3e836a82-b279-41c1-9e55-a75a91617e3f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

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
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("efa20d38-3395-432d-a7b7-f58ce4e493d8"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("3944b529-caca-4ca2-ab6f-aca86f553872"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

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

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2590) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2600), new DateTime(2019, 5, 18, 20, 9, 5, 885, DateTimeKind.Utc).AddTicks(2600) });
        }
    }
}
