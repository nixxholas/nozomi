using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_RequestComponentsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestComponents_RequestId",
                table: "RequestComponents");

            migrationBuilder.DropPrimaryKey(
                name: "RequestComponenDatum_PK_Id",
                table: "RequestComponentData");

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

            migrationBuilder.AddPrimaryKey(
                name: "RequestComponentDatum_PK_Id",
                table: "RequestComponentData",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("84128eaf-9c03-4e26-b5ed-e4c7c9f7192b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("6faf778a-3b43-486b-b7db-9317248f780b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", 5000, null, 0L, "CurrencyPairRequest", new Guid("7941ff17-c10a-4aa1-881b-7392e59335b9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 11, 2, 12, 57, 43, 779, DateTimeKind.Local), new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local), new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local), new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local), new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local), new DateTime(2018, 11, 2, 12, 57, 43, 799, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "RequestComponent_AK_RequestId_ComponentType",
                table: "RequestComponents",
                columns: new[] { "RequestId", "ComponentType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RequestComponent_AK_RequestId_ComponentType",
                table: "RequestComponents");

            migrationBuilder.DropPrimaryKey(
                name: "RequestComponentDatum_PK_Id",
                table: "RequestComponentData");

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

            migrationBuilder.AddPrimaryKey(
                name: "RequestComponenDatum_PK_Id",
                table: "RequestComponentData",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("7314aaab-0e64-4dbe-a2d2-271f4e68e5ae"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("40861286-11f4-443c-a215-05da268c5f8a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", 5000, null, 0L, "CurrencyPairRequest", new Guid("5285f92f-cf72-495c-a193-f33b046c3a0f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 21, 13, 31, 46, 640, DateTimeKind.Local), new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local), new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local), new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local), new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local), new DateTime(2018, 10, 21, 13, 31, 46, 642, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_RequestId",
                table: "RequestComponents",
                column: "RequestId");
        }
    }
}
