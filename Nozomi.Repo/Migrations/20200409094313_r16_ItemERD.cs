using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r16_ItemERD : Migration
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
                table: "Currencies");

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

            migrationBuilder.AddColumn<Guid>(
                name: "ItemGuid",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemPairGuid",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemTypeGuid",
                table: "Requests",
                nullable: true);

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
                name: "Value",
                table: "ComponentHistoricItems",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.CreateTable(
                name: "ItemPairs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    SourceId = table.Column<long>(nullable: false),
                    MainTicker = table.Column<string>(nullable: false),
                    CounterTicker = table.Column<string>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPairs_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(maxLength: 12, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Guid);
                    table.UniqueConstraint("AK_ItemTypes_Slug", x => x.Slug);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    ItemTypeGuid = table.Column<Guid>(nullable: false),
                    LogoPath = table.Column<string>(nullable: false, defaultValue: "assets/svg/icons/question.svg"),
                    Abbreviation = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Denominations = table.Column<int>(nullable: false, defaultValue: 0),
                    DenominationName = table.Column<string>(nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeGuid",
                        column: x => x.ItemTypeGuid,
                        principalTable: "ItemTypes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemProperties",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ItemGuid = table.Column<Guid>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemProperties", x => x.Guid);
                    table.UniqueConstraint("AK_ItemProperties_Guid_Name", x => new { x.Guid, x.Name });
                    table.ForeignKey(
                        name: "FK_ItemProperties_Items_ItemGuid",
                        column: x => x.ItemGuid,
                        principalTable: "Items",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSources",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    ItemGuid = table.Column<Guid>(nullable: false),
                    SourceId = table.Column<long>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSources_Items_ItemGuid",
                        column: x => x.ItemGuid,
                        principalTable: "Items",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSources_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ItemGuid",
                table: "Requests",
                column: "ItemGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ItemPairGuid",
                table: "Requests",
                column: "ItemPairGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ItemTypeGuid",
                table: "Requests",
                column: "ItemTypeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPairs_SourceId",
                table: "ItemPairs",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPairs_MainTicker_CounterTicker_SourceId",
                table: "ItemPairs",
                columns: new[] { "MainTicker", "CounterTicker", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemProperties_ItemGuid",
                table: "ItemProperties",
                column: "ItemGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeGuid",
                table: "Items",
                column: "ItemTypeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Slug",
                table: "Items",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSources_SourceId",
                table: "ItemSources",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSources_ItemGuid_SourceId",
                table: "ItemSources",
                columns: new[] { "ItemGuid", "SourceId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysedComponents_Currencies_CurrencyId",
                table: "AnalysedComponents",
                column: "CurrencyId",
                principalTable: "Currencies",
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
                table: "Currencies",
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
                principalTable: "Currencies",
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
                principalTable: "Currencies",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Items_ItemGuid",
                table: "Requests",
                column: "ItemGuid",
                principalTable: "Items",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ItemPairs_ItemPairGuid",
                table: "Requests",
                column: "ItemPairGuid",
                principalTable: "ItemPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ItemTypes_ItemTypeGuid",
                table: "Requests",
                column: "ItemTypeGuid",
                principalTable: "ItemTypes",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
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
                table: "Currencies");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Items_ItemGuid",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ItemPairs_ItemPairGuid",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ItemTypes_ItemTypeGuid",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "ItemPairs");

            migrationBuilder.DropTable(
                name: "ItemProperties");

            migrationBuilder.DropTable(
                name: "ItemSources");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ItemGuid",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ItemPairGuid",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ItemTypeGuid",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemGuid",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemPairGuid",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemTypeGuid",
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
                principalTable: "Currencies",
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
                table: "Currencies",
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
                principalTable: "Currencies",
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
                principalTable: "Currencies",
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
