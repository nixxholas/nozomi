using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r4_CPRCRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.AlterColumn<int>(
                name: "ComponentType",
                table: "RequestComponents",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("b476e03b-deb2-4ee3-8481-2fcf80b17b76"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("f086af5a-1e63-4a49-88a8-cb92251b43e3"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId" },
                values: new object[] { 1L, 1, new DateTime(2018, 10, 11, 0, 17, 24, 50, DateTimeKind.Local), 0L, null, 0L, "RequestComponent", true, new DateTime(2018, 10, 11, 0, 17, 24, 53, DateTimeKind.Local), 0L, "1", 1L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId" },
                values: new object[] { 2L, 2, new DateTime(2018, 10, 11, 0, 17, 24, 53, DateTimeKind.Local), 0L, null, 0L, "RequestComponent", true, new DateTime(2018, 10, 11, 0, 17, 24, 53, DateTimeKind.Local), 0L, "0", 1L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId" },
                values: new object[] { 3L, 1, new DateTime(2018, 10, 11, 0, 17, 24, 53, DateTimeKind.Local), 0L, null, 0L, "RequestComponent", true, new DateTime(2018, 10, 11, 0, 17, 24, 53, DateTimeKind.Local), 0L, "0", 2L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.AlterColumn<int>(
                name: "ComponentType",
                table: "RequestComponents",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("1afc1fad-bf6c-4d02-9d13-3f187f52f419"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("8e84085d-5e80-477f-9d9d-0751f044124d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "ComponentType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(2018, 10, 7, 17, 20, 38, 583, DateTimeKind.Local), 0L, null, 0L, "CurrencyPairComponent", true, new DateTime(2018, 10, 7, 17, 20, 38, 585, DateTimeKind.Local), 0L, "1", 1L, 1, 1L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "ComponentType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(2018, 10, 7, 17, 20, 38, 585, DateTimeKind.Local), 0L, null, 0L, "CurrencyPairComponent", true, new DateTime(2018, 10, 7, 17, 20, 38, 585, DateTimeKind.Local), 0L, "0", 1L, 2, 1L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "ComponentType", "CurrencyPairId" },
                values: new object[] { 3L, new DateTime(2018, 10, 7, 17, 20, 38, 585, DateTimeKind.Local), 0L, null, 0L, "CurrencyPairComponent", true, new DateTime(2018, 10, 7, 17, 20, 38, 585, DateTimeKind.Local), 0L, "0", 2L, 1, 2L });
        }
    }
}
