using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r1_InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TypeShortForm = table.Column<string>(maxLength: 12, nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Abbreviation = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    APIDocsURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CurrencyTypeId = table.Column<long>(nullable: false),
                    Abbrv = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CurrencySourceId = table.Column<long>(nullable: false),
                    WalletTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_Sources_CurrencySourceId",
                        column: x => x.CurrencySourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Currencies_CurrencyTypes_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalTable: "CurrencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyPairs",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CurrencyPairType = table.Column<int>(nullable: false),
                    APIUrl = table.Column<string>(nullable: false),
                    DefaultComponent = table.Column<string>(nullable: false),
                    CurrencySourceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyPairs_Sources_CurrencySourceId",
                        column: x => x.CurrencySourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartialCurrencyPairs",
                columns: table => new
                {
                    CurrencyId = table.Column<long>(nullable: false),
                    CurrencyPairId = table.Column<long>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialCurrencyPairs", x => new { x.CurrencyPairId, x.IsMain });
                    table.ForeignKey(
                        name: "FK_PartialCurrencyPairs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartialCurrencyPairs_CurrencyPairs_CurrencyPairId",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RequestType = table.Column<int>(nullable: false),
                    DataPath = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    CurrencyPairId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Request_PK_Id", x => x.Id);
                    table.UniqueConstraint("Request_AK_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Requests_CurrencyPairs_CurrencyPairId",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestComponents",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RequestId = table.Column<long>(nullable: false),
                    QueryComponent = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ComponentType = table.Column<int>(nullable: true),
                    CurrencyPairId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RequestComponent_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestComponents_CurrencyPairs_CurrencyPairId",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestComponents_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type = table.Column<int>(nullable: false),
                    RawPayload = table.Column<string>(nullable: true),
                    RequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RequestLog_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestLogs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestProperties",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RequestPropertyType = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    RequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RequestProperty_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestProperties_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    { 2L, "None", "HAKO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Coinhako" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbrv", "CreatedAt", "CreatedBy", "CurrencySourceId", "CurrencyTypeId", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[,]
                {
                    { 1L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 1L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L },
                    { 2L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 2L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", 1L },
                    { 3L, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 2L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Kyber Network Coin", 4L }
                });

            migrationBuilder.InsertData(
                table: "CurrencyPairs",
                columns: new[] { "Id", "APIUrl", "CreatedAt", "CreatedBy", "CurrencyPairType", "CurrencySourceId", "DefaultComponent", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { 1L, "https://api.ethfinex.com/v2/ticker/tETHUSD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 1L, "0", null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L },
                    { 2L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, 1L, "0", null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L }
                });

            migrationBuilder.InsertData(
                table: "PartialCurrencyPairs",
                columns: new[] { "CurrencyPairId", "IsMain", "CurrencyId" },
                values: new object[,]
                {
                    { 1L, false, 1L },
                    { 1L, true, 2L },
                    { 2L, false, 1L },
                    { 2L, true, 3L }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", null, 0L, "CurrencyPairRequest", new Guid("ee6d8194-780e-45ae-a4a4-72140d7e7576"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", null, 0L, "CurrencyPairRequest", new Guid("149630a1-3ca9-48a5-bd26-4b81cf115090"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L }
                });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value", "ComponentType", "CurrencyPairId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, "CurrencyPairComponent", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "0", 1L, "0", 1, 1L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Discriminator", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value", "ComponentType", "CurrencyPairId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, "CurrencyPairComponent", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "0", 2L, "0", 1, 2L });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CurrencySourceId",
                table: "Currencies",
                column: "CurrencySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CurrencyTypeId",
                table: "Currencies",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_CurrencySourceId",
                table: "CurrencyPairs",
                column: "CurrencySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PartialCurrencyPairs_CurrencyId",
                table: "PartialCurrencyPairs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_CurrencyPairId",
                table: "RequestComponents",
                column: "CurrencyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_RequestId",
                table: "RequestComponents",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_RequestId",
                table: "RequestLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestProperties_RequestId",
                table: "RequestProperties",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyPairId",
                table: "Requests",
                column: "CurrencyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Abbreviation",
                table: "Sources",
                column: "Abbreviation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartialCurrencyPairs");

            migrationBuilder.DropTable(
                name: "RequestComponents");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "RequestProperties");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "CurrencyTypes");

            migrationBuilder.DropTable(
                name: "CurrencyPairs");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
