using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_AnalysedComponentIndicesAndPriceAveraging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_CurrencyId",
                table: "AnalysedComponents");

            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_CurrencyPairId",
                table: "AnalysedComponents");

            migrationBuilder.DropIndex(
                name: "IX_AnalysedComponents_CurrencyTypeId",
                table: "AnalysedComponents");

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
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(4800), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(5240) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6970), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6970) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7380), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7390), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7390) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7390), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7390) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7390), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7390) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7400), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7420), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7420) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7500), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7500), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7500), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7500) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6920), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6920) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 51L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 52L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 53L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 54L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 3000, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 55L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(7540) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 56L,
                columns: new[] { "ComponentType", "CreatedAt", "ModifiedAt" },
                values: new object[] { 10, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 57L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940), new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6940) });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyPairId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "UIFormatting", "Value" },
                values: new object[,]
                {
                    { 61L, 11, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 0L, "$ 0[.]00", null },
                    { 60L, 10, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6960), 0L, "$ 0[.]00", null },
                    { 59L, 11, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 0L, "$ 0[.]00", null },
                    { 58L, 10, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 13, 0, 27, 830, DateTimeKind.Utc).AddTicks(6950), 0L, "$ 0[.]00", null }
                });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(3650), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(3650) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4420), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4430) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4430), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4430) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4430), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4430) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4440) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4520), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4520) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4520), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4520) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4520), new DateTime(2019, 5, 20, 13, 0, 27, 914, DateTimeKind.Utc).AddTicks(4530) });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("faea6e5b-0363-43d0-9b40-c0ae0ecf606b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("2d2ef11d-9ea0-4274-ad11-8890618c4b13"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("6bd6ee0f-f396-4ad7-a6aa-d81c27b84927"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("01ca7d1d-599a-46ce-99b9-8bb0d433ada1"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("52ae43d0-c724-4c83-b2cb-39e8c4f49fe6"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 16L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("6c9d3702-1fbd-404b-ab0e-cb0c7190c168"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 15L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("dc341d6d-687d-4a58-8e82-a5d54eb7a900"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 14L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("5b7451e3-24e7-445f-a030-b86a3443123f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 13L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("7ce3a852-9ae8-4f94-a982-0fe4bc51c23d"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("da923b38-8336-40cd-b6a8-cf5f85903ba9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("5c3050fe-7a6b-48fa-b78c-3d7269aa2c75"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("67adce52-ea8e-4f7d-9c2f-9282e34bfc25"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("63e97b92-e358-4905-a9cf-0d5e9876effd"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("dda1284c-e32c-40e3-829b-8ee620279159"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.alphavantage.co/query", 5000, null, 0L, "CurrencyPairRequest", new Guid("1e4e028f-c2b7-4ecf-a656-577a7d8cf6a7"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("ef581f4d-8d16-485a-8e09-eda821eb2f34"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("57e6a3af-aa20-4964-80fe-8736e39c1c2c"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("22d39750-6aa3-42e1-ae62-5491e000adc9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("2210f3c9-9f4a-452a-ab4f-6c9c0cbc43d6"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("eb6558c6-600b-42b5-876c-8fb5ce6223ea"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(8650), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(8660) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9040), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9040) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9050) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9060) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9140), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9140) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9150) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9180) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9190) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9200), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9200) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9200), new DateTime(2019, 5, 20, 13, 0, 27, 892, DateTimeKind.Utc).AddTicks(9200) });

            migrationBuilder.CreateIndex(
                name: "AnalysedComponent_Index_CurrencyId_ComponentType",
                table: "AnalysedComponents",
                columns: new[] { "CurrencyId", "ComponentType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AnalysedComponent_Index_CurrencyPairId_ComponentType",
                table: "AnalysedComponents",
                columns: new[] { "CurrencyPairId", "ComponentType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AnalysedComponent_Index_CurrencyTypeId_ComponentType",
                table: "AnalysedComponents",
                columns: new[] { "CurrencyTypeId", "ComponentType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "AnalysedComponent_Index_CurrencyId_ComponentType",
                table: "AnalysedComponents");

            migrationBuilder.DropIndex(
                name: "AnalysedComponent_Index_CurrencyPairId_ComponentType",
                table: "AnalysedComponents");

            migrationBuilder.DropIndex(
                name: "AnalysedComponent_Index_CurrencyTypeId_ComponentType",
                table: "AnalysedComponents");

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 60L);

            migrationBuilder.DeleteData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 61L);

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
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(200), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(620) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 500, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2970), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2970) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2390), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2400) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2410), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2410) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 51L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 52L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 53L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 54L,
                columns: new[] { "CreatedAt", "Delay", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530), 1000, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 55L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3120), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3120) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 56L,
                columns: new[] { "ComponentType", "CreatedAt", "ModifiedAt" },
                values: new object[] { 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420) });

            migrationBuilder.UpdateData(
                table: "AnalysedComponents",
                keyColumn: "Id",
                keyValue: 57L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510), new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(7260), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(7260) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8010), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8010) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040) });

            migrationBuilder.UpdateData(
                table: "CurrencySources",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040) });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("4c192fd7-c924-4dad-a1a7-640aa4b583c4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("73b5731a-77ad-4a5c-a111-0d133c89cd1e"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("aa85dcba-a5d5-49c3-a858-d0d6c46e9b2b"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("f9ede5ad-aea5-4154-9395-671b2611a5ac"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("c1f6fc3f-d5f2-4a04-8e9e-66528e3bb568"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 16L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("94702f2f-8527-4ddf-98de-998343fe9784"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 15L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("5b68db33-4f46-4e6b-97ba-ad9393a95168"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 14L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("52088480-0ee9-41e6-8eda-aab4b2d09761"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 13L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("36caf468-b8d1-4d82-98ea-fba94bdf921a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("0214da06-699d-4843-a88f-b3cf6883454f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("8deb3374-962d-4e06-97ac-0ccc39cc7906"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("7883e24f-d941-4cf3-b684-d3d1eb27dac7"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("eba1ac98-9072-4429-b60d-35e9da307d7a"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("97646141-1e27-46c0-9271-5f557f92836f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.alphavantage.co/query", 5000, null, 0L, "CurrencyPairRequest", new Guid("653138a5-3df4-43fa-875f-945e7dffa40e"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("b1aecae0-0235-4878-9e2e-8b44a98b49a2"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("be003106-ba60-46e8-a56d-10ff33a0c4ef"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("c7cb205a-9800-44c5-88c9-050eb7ca3dc0"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("7df93046-cea1-4b37-8005-539c255bbfe9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("bff8f54a-0bc8-4ae6-9a44-9de66a85d662"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1400), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1400) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1770), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1770) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930) });

            migrationBuilder.UpdateData(
                table: "RequestComponents",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930) });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_CurrencyId",
                table: "AnalysedComponents",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_CurrencyPairId",
                table: "AnalysedComponents",
                column: "CurrencyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_CurrencyTypeId",
                table: "AnalysedComponents",
                column: "CurrencyTypeId");
        }
    }
}
