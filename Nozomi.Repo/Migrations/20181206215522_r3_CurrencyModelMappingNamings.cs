using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r3_CurrencyModelMappingNamings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Sources_CurrencySourceId",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_CurrencyTypes_CurrencyTypeId",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyPairs_Sources_CurrencySourceId",
                table: "CurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_PartialCurrencyPairs_Currencies_CurrencyId",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_PartialCurrencyPairs_CurrencyPairs_CurrencyPairId",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sources",
                table: "Sources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartialCurrencyPairs",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyTypes",
                table: "CurrencyTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyPairs",
                table: "CurrencyPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

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

            migrationBuilder.RenameIndex(
                name: "IX_Sources_Abbreviation",
                table: "Sources",
                newName: "Source_Index_Abbreviation");

            migrationBuilder.AddPrimaryKey(
                name: "Source_PK_Id",
                table: "Sources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PartialCurrencyPair_CK_CurrencyPairId_IsMain",
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain" });

            migrationBuilder.AddPrimaryKey(
                name: "CurrencyType_PK_Id",
                table: "CurrencyTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "CurrencyPair_PK_Id",
                table: "CurrencyPairs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "Currency_PK_Id",
                table: "Currencies",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("a26b3970-e215-4efd-8d94-265d168a9c68"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 0, 1L },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("b301e965-87c8-440f-aaf3-769aabc97e22"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 0, 2L },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", 5000, null, 0L, "CurrencyPairRequest", new Guid("80ea92a2-1c34-4832-936e-1c5fc3025e97"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 0, 3L }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "APIDocsURL", "Abbreviation", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[] { 4L, "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html", "ECB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "European Central Bank" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[,]
                {
                    { 6L, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 4L, 1L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Euro", 0L },
                    { 7L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 4L, 1L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L }
                });

            migrationBuilder.InsertData(
                table: "CurrencyPairs",
                columns: new[] { "Id", "APIUrl", "CreatedAt", "CreatedBy", "CurrencyPairType", "CurrencySourceId", "DefaultComponent", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy" },
                values: new object[] { 4L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 4L, "Cube", null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 7, 5, 55, 22, 182, DateTimeKind.Local), new DateTime(2018, 12, 7, 5, 55, 22, 183, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local), new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local), new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local), new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local), new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain", "CurrencyId" },
                values: new object[] { 4L, true, 6L });

            migrationBuilder.InsertData(
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain", "CurrencyId" },
                values: new object[] { 4L, false, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("676ff6e8-6a4e-4605-8b2b-59d39f636c29"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 4L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestComponentDatumId", "RequestId" },
                values: new object[] { 6L, 1, new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local), 0L, null, 0L, true, new DateTime(2018, 12, 7, 5, 55, 22, 184, DateTimeKind.Local), 0L, "gesmes:Envelope/Cube/Cube/Cube/0=>@rate", 0L, 4L });

            migrationBuilder.AddForeignKey(
                name: "Source_Currencies_Constraint",
                table: "Currencies",
                column: "CurrencySourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencyType_Currencies_Constraint",
                table: "Currencies",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Source_CurrencyPairs_Constraint",
                table: "CurrencyPairs",
                column: "CurrencySourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "PartialCurrencyPairs_Currency_Constraint",
                table: "PartialCurrencyPairs",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "PartialCurrencyPairs_CurrencyPair_Constraint",
                table: "PartialCurrencyPairs",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Source_Currencies_Constraint",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "CurrencyType_Currencies_Constraint",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "Source_CurrencyPairs_Constraint",
                table: "CurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "PartialCurrencyPairs_Currency_Constraint",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "PartialCurrencyPairs_CurrencyPair_Constraint",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "Source_PK_Id",
                table: "Sources");

            migrationBuilder.DropPrimaryKey(
                name: "PartialCurrencyPair_CK_CurrencyPairId_IsMain",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropPrimaryKey(
                name: "CurrencyType_PK_Id",
                table: "CurrencyTypes");

            migrationBuilder.DropPrimaryKey(
                name: "CurrencyPair_PK_Id",
                table: "CurrencyPairs");

            migrationBuilder.DropPrimaryKey(
                name: "Currency_PK_Id",
                table: "Currencies");

            migrationBuilder.DeleteData(
                table: "PartialCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "IsMain" },
                keyValues: new object[] { 4L, false });

            migrationBuilder.DeleteData(
                table: "PartialCurrencyPairs",
                keyColumns: new[] { "CurrencyPairId", "IsMain" },
                keyValues: new object[] { 4L, true });

            migrationBuilder.DeleteData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L);

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
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "CurrencyPairs",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.RenameIndex(
                name: "Source_Index_Abbreviation",
                table: "Sources",
                newName: "IX_Sources_Abbreviation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sources",
                table: "Sources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartialCurrencyPairs",
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyTypes",
                table: "CurrencyTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyPairs",
                table: "CurrencyPairs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("8fd9be01-afec-4e5c-8237-d4c16276f5bf"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("b9dfa033-bf21-4fa0-99fa-676d161989ad"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", 5000, null, 0L, "CurrencyPairRequest", new Guid("4a8a2493-62d5-4719-bb94-b18d4a80bfe6"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 0, 3L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 3, 12, 42, 40, 707, DateTimeKind.Local), new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local) });

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Sources_CurrencySourceId",
                table: "Currencies",
                column: "CurrencySourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_CurrencyTypes_CurrencyTypeId",
                table: "Currencies",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyPairs_Sources_CurrencySourceId",
                table: "CurrencyPairs",
                column: "CurrencySourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartialCurrencyPairs_Currencies_CurrencyId",
                table: "PartialCurrencyPairs",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartialCurrencyPairs_CurrencyPairs_CurrencyPairId",
                table: "PartialCurrencyPairs",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
