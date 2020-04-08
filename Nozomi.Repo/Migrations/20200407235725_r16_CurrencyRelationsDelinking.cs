using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r16_CurrencyRelationsDelinking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Currency_AnalysedComponents_Constraint",
                table: "AnalysedComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysedComponents_CurrencyPairs_CurrencyPairId",
                table: "AnalysedComponents");

            migrationBuilder.DropForeignKey(
                name: "CurrencyType_AnalysedComponents_Constraint",
                table: "AnalysedComponents");

            migrationBuilder.DropForeignKey(
                name: "CurrencyType_Currencies_Constraint",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "Source_CurrencyPairs_Constraint",
                table: "CurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "CurrencySource_Currency_Constraint",
                table: "CurrencySources");

            migrationBuilder.DropForeignKey(
                name: "CurrencySource_Source_Constraint",
                table: "CurrencySources");

            migrationBuilder.DropForeignKey(
                name: "Currencies_CurrencyRequests_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "CurrencyType_Request_Constraint",
                table: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "APIDocsURL",
                table: "Sources",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "CurrencyProperty",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MainTicker",
                table: "CurrencyPairs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CounterTicker",
                table: "CurrencyPairs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APIUrl",
                table: "CurrencyPairs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ComponentHistoricItems",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysedComponents_Currencies_CurrencyId",
                table: "AnalysedComponents",
                column: "CurrencyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysedComponents_CurrencyPairs_CurrencyPairId",
                table: "AnalysedComponents",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysedComponents_CurrencyTypes_CurrencyTypeId",
                table: "AnalysedComponents",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_CurrencyTypes_CurrencyTypeId",
                table: "Items",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyPairs_Sources_SourceId",
                table: "CurrencyPairs",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencySources_Currencies_CurrencyId",
                table: "CurrencySources",
                column: "CurrencyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencySources_Sources_SourceId",
                table: "CurrencySources",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Currencies_CurrencyId",
                table: "Requests",
                column: "CurrencyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyTypes_CurrencyTypeId",
                table: "Requests",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysedComponents_Currencies_CurrencyId",
                table: "AnalysedComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysedComponents_CurrencyPairs_CurrencyPairId",
                table: "AnalysedComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysedComponents_CurrencyTypes_CurrencyTypeId",
                table: "AnalysedComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_CurrencyTypes_CurrencyTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyPairs_Sources_SourceId",
                table: "CurrencyPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencySources_Currencies_CurrencyId",
                table: "CurrencySources");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencySources_Sources_SourceId",
                table: "CurrencySources");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Currencies_CurrencyId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyTypes_CurrencyTypeId",
                table: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "APIDocsURL",
                table: "Sources",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "CurrencyProperty",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MainTicker",
                table: "CurrencyPairs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CounterTicker",
                table: "CurrencyPairs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "APIUrl",
                table: "CurrencyPairs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ComponentHistoricItems",
                type: "text",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldDefaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "Currency_AnalysedComponents_Constraint",
                table: "AnalysedComponents",
                column: "CurrencyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysedComponents_CurrencyPairs_CurrencyPairId",
                table: "AnalysedComponents",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencyType_AnalysedComponents_Constraint",
                table: "AnalysedComponents",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "CurrencyType_Currencies_Constraint",
                table: "Items",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Source_CurrencyPairs_Constraint",
                table: "CurrencyPairs",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencySource_Currency_Constraint",
                table: "CurrencySources",
                column: "CurrencyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "CurrencySource_Source_Constraint",
                table: "CurrencySources",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Currencies_CurrencyRequests_Constraint",
                table: "Requests",
                column: "CurrencyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "CurrencyType_Request_Constraint",
                table: "Requests",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
