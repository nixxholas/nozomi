using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r1_InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    TypeShortForm = table.Column<string>(maxLength: 12, nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CurrencyType_PK_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    APIDocsURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Source_PK_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    CurrencyTypeId = table.Column<long>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Denominations = table.Column<int>(nullable: false, defaultValue: 0),
                    DenominationName = table.Column<string>(nullable: true),
                    WalletTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Currency_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "CurrencyType_Currencies_Constraint",
                        column: x => x.CurrencyTypeId,
                        principalTable: "CurrencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyPairs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    CurrencyPairType = table.Column<int>(nullable: false),
                    APIUrl = table.Column<string>(nullable: false),
                    DefaultComponent = table.Column<string>(nullable: false),
                    SourceId = table.Column<long>(nullable: false),
                    MainCurrencyAbbrv = table.Column<string>(nullable: false),
                    CounterCurrencyAbbrv = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CurrencyPair_PK_Id", x => x.Id);
                    table.UniqueConstraint("CurrencyPair_AK_MainCurrency_CounterCurrency_Source", x => new { x.MainCurrencyAbbrv, x.CounterCurrencyAbbrv, x.SourceId });
                    table.ForeignKey(
                        name: "Source_CurrencyPairs_Constraint",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyProperty",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyProperty_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencySources",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    CurrencyId = table.Column<long>(nullable: false),
                    SourceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CurrencySource_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "CurrencySource_Currency_Constraint",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "CurrencySource_Source_Constraint",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnalysedComponents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    ComponentType = table.Column<int>(nullable: false, defaultValue: 0),
                    Value = table.Column<string>(nullable: true),
                    IsDenominated = table.Column<bool>(nullable: false),
                    Delay = table.Column<int>(nullable: false, defaultValue: 86400000),
                    UIFormatting = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<long>(nullable: true),
                    CurrencyPairId = table.Column<long>(nullable: true),
                    CurrencyTypeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AnalysedComponent_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "Currency_AnalysedComponents_Constraint",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalysedComponents_CurrencyPairs_CurrencyPairId",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "CurrencyType_AnalysedComponents_Constraint",
                        column: x => x.CurrencyTypeId,
                        principalTable: "CurrencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    RequestType = table.Column<int>(nullable: false),
                    ResponseType = table.Column<int>(nullable: false, defaultValue: 1),
                    DataPath = table.Column<string>(nullable: true),
                    Delay = table.Column<int>(nullable: false, defaultValue: 0),
                    FailureDelay = table.Column<long>(nullable: false, defaultValue: 3600000L),
                    Discriminator = table.Column<string>(nullable: false),
                    CurrencyPairId = table.Column<long>(nullable: true),
                    CurrencyId = table.Column<long>(nullable: true),
                    WebsocketRequest_CurrencyPairId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Request_PK_Id", x => x.Id);
                    table.UniqueConstraint("Request_AK_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "CurrencyPair_CurrencyPairRequest_Constraint",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "CurrencyRequest_Currency_Constraint",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_CurrencyPairs_WebsocketRequest_CurrencyPairId",
                        column: x => x.WebsocketRequest_CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysedHistoricItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    AnalysedComponentId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    HistoricDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AnalysedHistoricItem_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysedHistoricItems_AnalysedComponents_AnalysedComponentId",
                        column: x => x.AnalysedComponentId,
                        principalTable: "AnalysedComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestComponents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    ComponentType = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    QueryComponent = table.Column<string>(nullable: true),
                    IsDenominated = table.Column<bool>(nullable: false, defaultValue: false),
                    AnomalyIgnorance = table.Column<bool>(nullable: false, defaultValue: false),
                    Value = table.Column<string>(nullable: true),
                    RequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RequestComponent_PK_Id", x => x.Id);
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "WebsocketCommands",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    CommandType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Delay = table.Column<long>(nullable: false, defaultValue: 0L),
                    WebsocketRequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("WebsocketCommand_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommands_Requests_WebsocketRequestId",
                        column: x => x.WebsocketRequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RcdHistoricItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    HistoricDateTime = table.Column<DateTime>(nullable: false),
                    Value = table.Column<string>(nullable: true, defaultValue: ""),
                    RequestComponentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RcdHistoricItem_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RcdHistoricItems_RequestComponents_RequestComponentId",
                        column: x => x.RequestComponentId,
                        principalTable: "RequestComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsocketCommandProperties",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    CommandPropertyType = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: false),
                    WebsocketCommandId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("WebsocketCommandProperty_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommandProperties_WebsocketCommands_WebsocketComma~",
                        column: x => x.WebsocketCommandId,
                        principalTable: "WebsocketCommands",
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
                    { 2L, "None", "HAKO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Coinhako" },
                    { 3L, "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md", "BNA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Binance" },
                    { 4L, "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html", "ECB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "European Central Bank" },
                    { 5L, "https://www.alphavantage.co/documentation/", "AVG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "AlphaVantage" },
                    { 6L, "https://docs.poloniex.com/#public-http-api-methods", "POLO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Poloniex" }
                });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyPairId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "UIFormatting", "Value" },
                values: new object[] { 55L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3120), 0L, null, null, 2L, 3000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3120), 0L, "$ 0 a", null });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 1L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 10L, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Litecoin", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[] { 6L, "BCN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bytecoin", 0L });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Denominations", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "WalletTypeId" },
                values: new object[,]
                {
                    { 5L, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, "Sat", 8, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin", 0L },
                    { 7L, "BTS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "BitShares", 0L },
                    { 3L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, "Wei", 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", 1L },
                    { 9L, "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Singapore Dollar", 0L },
                    { 8L, "USDT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Tether USD", 0L },
                    { 2L, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, 0, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Euro", 0L },
                    { 4L, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Kyber Network Crystal", 4L }
                });

            migrationBuilder.InsertData(
                table: "CurrencyPairs",
                columns: new[] { "Id", "APIUrl", "CounterCurrencyAbbrv", "CreatedAt", "CreatedBy", "CurrencyPairType", "DefaultComponent", "DeletedAt", "DeletedBy", "IsEnabled", "MainCurrencyAbbrv", "ModifiedAt", "ModifiedBy", "SourceId" },
                values: new object[,]
                {
                    { 14L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 5L, "https://poloniex.com/public?command=returnTicker", "BCN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "BTC_BCN/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L },
                    { 4L, "https://www.alphavantage.co/query", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, "Realtime Currency Exchange Rate/5. Exchange Rate", null, 0L, true, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 5L },
                    { 3L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, "Cube", null, 0L, true, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 4L },
                    { 16L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 15L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 13L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 7L, "https://api.bitfinex.com/v1/pubticker/etheur", "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, "0", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L },
                    { 11L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 10L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "b", null, 0L, true, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 9L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "b", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 6L, "https://poloniex.com/public?command=returnTicker", "BTS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "BTC_BTS/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L },
                    { 2L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, "0", null, 0L, true, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L },
                    { 1L, "https://api.ethfinex.com/v2/ticker/tETHUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, "0", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L },
                    { 12L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 8L, "https://poloniex.com/public?command=returnTicker", "USDT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "USDT_BTC/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L }
                });

            migrationBuilder.InsertData(
                table: "AnalysedComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "CurrencyId", "CurrencyPairId", "CurrencyTypeId", "Delay", "DeletedAt", "DeletedBy", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "UIFormatting", "Value" },
                values: new object[,]
                {
                    { 5L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2970), 0L, null, 1L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2970), 0L, "$ 0[.]00", null },
                    { 53L, 20, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, 5L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, "$ 0 a", null },
                    { 54L, 21, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530), 0L, 5L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530), 0L, "$ 0 a", null },
                    { 24L, 80, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 8L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "0 a", null },
                    { 33L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, null, 12L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, "0[.]0", null },
                    { 32L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, null, 12L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, "$ 0[.]00", null },
                    { 31L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, null, 12L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, "$ 0[.]00", null },
                    { 30L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, null, 11L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, "0[.]0", null },
                    { 29L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 11L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "$ 0[.]00", null },
                    { 4L, 80, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530), 0L, null, 1L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2530), 0L, "$ 0 a", null },
                    { 45L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 16L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "0[.]0", null },
                    { 6L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), 0L, null, 1L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), 0L, "$ 0[.]00", null },
                    { 52L, 6, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, 5L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, "$ 0 a", null },
                    { 7L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), 0L, null, 1L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), 0L, "0[.]0", null },
                    { 8L, 80, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), 0L, null, 2L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2980), 0L, "0 a", null },
                    { 9L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 2L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, "$ 0[.]00", null },
                    { 10L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 2L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, "$ 0[.]00", null },
                    { 11L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 2L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, "0[.]0", null },
                    { 27L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 8L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "0[.]0", null },
                    { 20L, 80, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 7L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0 a", null },
                    { 21L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 7L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0[.]00", null },
                    { 22L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 7L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "0[.]00", null },
                    { 23L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 7L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "0[.]0", null },
                    { 18L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 6L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0[.]00", null },
                    { 17L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 6L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0[.]00", null },
                    { 19L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 6L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0[.]0", null },
                    { 3L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, 5L, null, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, "$ 0 a", null },
                    { 15L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 5L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0[.]0", null },
                    { 34L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, null, 13L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3020), 0L, "$ 0[.]00", null },
                    { 44L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 16L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "$ 0[.]00", null },
                    { 43L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 16L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "$ 0[.]00", null },
                    { 12L, 80, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 5L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, "0 a", null },
                    { 42L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 15L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "0[.]0", null },
                    { 41L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 15L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "$ 0[.]00", null },
                    { 40L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 15L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "$ 0[.]00", null },
                    { 13L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 5L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, "0[.]00", null },
                    { 39L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, null, 14L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3110), 0L, "0[.]0", null },
                    { 38L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, null, 14L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, "$ 0[.]00", null },
                    { 37L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, null, 14L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, "$ 0[.]00", null },
                    { 1L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(200), 0L, 3L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(620), 0L, "$ 0 a", null },
                    { 46L, 6, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2390), 0L, 3L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2400), 0L, "$ 0 a", null },
                    { 47L, 20, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2410), 0L, 3L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2410), 0L, "$ 0 a", null },
                    { 48L, 21, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420), 0L, 3L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420), 0L, "$ 0 a", null },
                    { 56L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2420), 0L, "$ 0[.]00", null },
                    { 57L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510), 0L, "$ 0[.]00", null },
                    { 26L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 8L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "$ 0[.]00", null },
                    { 14L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, null, 5L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2990), 0L, "0[.]00", null },
                    { 36L, 70, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, null, 13L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, "0[.]0", null },
                    { 2L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510), 0L, 4L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2510), 0L, "$ 0 a", null },
                    { 49L, 6, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, 4L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, "$ 0 a", null },
                    { 50L, 20, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, 4L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, "$ 0 a", null },
                    { 51L, 21, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, 4L, null, null, 1000, null, 0L, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(2520), 0L, "$ 0 a", null },
                    { 25L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 8L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "$ 0[.]00", null },
                    { 35L, 11, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, null, 13L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3100), 0L, "$ 0[.]00", null },
                    { 16L, 80, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, null, 6L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3000), 0L, "0 a", null },
                    { 28L, 10, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, null, 11L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 9, 14, 41, 0, DateTimeKind.Utc).AddTicks(3010), 0L, "$ 0[.]00", null }
                });

            migrationBuilder.InsertData(
                table: "CurrencySources",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CurrencyId", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "SourceId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(7260), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(7260), 0L, 1L },
                    { 14L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 7L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 6L },
                    { 9L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 4L },
                    { 11L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 5L },
                    { 19L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 2L },
                    { 2L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8010), 0L, 2L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8010), 0L, 1L },
                    { 8L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 2L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 4L },
                    { 10L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 2L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 5L },
                    { 15L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 8L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 6L },
                    { 17L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 9L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 2L },
                    { 3L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 3L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 1L },
                    { 6L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 3L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 3L },
                    { 4L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 4L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 1L },
                    { 5L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 4L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8020), 0L, 3L },
                    { 7L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 5L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 3L },
                    { 12L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 5L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 6L },
                    { 16L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 5L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 2L },
                    { 13L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 6L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8030), 0L, 6L },
                    { 18L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 6L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 2L },
                    { 20L, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 10L, null, 0L, true, new DateTime(2019, 5, 20, 9, 14, 41, 82, DateTimeKind.Utc).AddTicks(8040), 0L, 2L }
                });

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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("be003106-ba60-46e8-a56d-10ff33a0c4ef"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

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
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("7df93046-cea1-4b37-8005-539c255bbfe9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("bff8f54a-0bc8-4ae6-9a44-9de66a85d662"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("c7cb205a-9800-44c5-88c9-050eb7ca3dc0"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("7883e24f-d941-4cf3-b684-d3d1eb27dac7"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("0214da06-699d-4843-a88f-b3cf6883454f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

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
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("b1aecae0-0235-4878-9e2e-8b44a98b49a2"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("8deb3374-962d-4e06-97ac-0ccc39cc7906"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 1L, 1000, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1400), 0L, null, 0L, null, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1400), 0L, "result", 1L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 27L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "USDT_BTC/lowestAsk", 12L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 35L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "a", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 36L, 8, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "A", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 37L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "b", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 38L, 5, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "B", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 39L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "data/buy_price", 15L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 40L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "data/sell_price", 15L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 41L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/buy_price", 16L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 42L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/sell_price", 16L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 43L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/buy_price", 17L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 44L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/sell_price", 17L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 45L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/buy_price", 18L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 46L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/sell_price", 18L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 47L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1920), 0L, "data/buy_price", 19L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 48L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), 0L, "data/sell_price", 19L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 49L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), 0L, "data/buy_price", 20L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 50L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1930), 0L, "data/sell_price", 20L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 16L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "gesmes:Envelope/Cube/Cube/Cube/0=>@rate", 7L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 17L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "['Realtime Currency Exchange Rate']/['5. Exchange Rate']", 8L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 18L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "BTC_BCN/baseVolume", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 19L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, "BTC_BCN/lowestAsk", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 20L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, "BTC_BCN/highestBid", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 21L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, "BTC_BTS/baseVolume", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 22L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1810), 0L, "BTC_BTS/lowestAsk", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 23L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), 0L, "BTC_BTS/highestBid", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 33L, 5, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "B", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 32L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "b", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 34L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1910), 0L, "v", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 30L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "a", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 2L, 1000, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1770), 0L, null, 0L, null, true, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1770), 0L, "result", 2L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 3L, 1005, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), 0L, "info/blocks", 3L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 4L, 1010, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), 0L, "info/difficulty", 3L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 31L, 8, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "A", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 6L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, "7", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 7L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, "2", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 8L, 8, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, "3", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 9L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, "0", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 10L, 5, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, "1", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 5L, 1000, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1780), 0L, "data/coin/circulatingSupply", 4L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 12L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "2", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 29L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "v", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 11L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1790), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "7", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 26L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "bid", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 25L, 1, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), 0L, "ask", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 28L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1900), 0L, "USDT_BTC/highestBid", 12L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 15L, 5, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "1", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 14L, 2, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "0", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 13L, 8, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1800), 0L, "3", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 24L, 12, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 9, 14, 41, 62, DateTimeKind.Utc).AddTicks(1890), 0L, "volume", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestProperties",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "Key", "ModifiedAt", "ModifiedBy", "RequestId", "RequestPropertyType", "Value" },
                values: new object[,]
                {
                    { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "apikey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224" },
                    { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "contractaddress", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "0xdd974d5c2e2928dea5f71b9825b8b646686bd200" },
                    { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "apikey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "TV5HJJHNP8094BRO" },
                    { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "from_currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "EUR" },
                    { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "to_currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "USD" },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "action", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "tokensupply" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "module", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, 32, "stats" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "apikey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 32, "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "action", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 32, "ethsupply" },
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "module", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, 32, "stats" },
                    { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, true, "function", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 8L, 300, "CURRENCY_EXCHANGE_RATE" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedHistoricItems_AnalysedComponentId",
                table: "AnalysedHistoricItems",
                column: "AnalysedComponentId");

            migrationBuilder.CreateIndex(
                name: "Currency_Index_Abbreviation",
                table: "Currencies",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CurrencyTypeId",
                table: "Currencies",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_SourceId",
                table: "CurrencyPairs",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyProperty_CurrencyId",
                table: "CurrencyProperty",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencySources_SourceId",
                table: "CurrencySources",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "CurrencySource_CK_CurrencyId_SourceId",
                table: "CurrencySources",
                columns: new[] { "CurrencyId", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RcdHistoricItems_RequestComponentId",
                table: "RcdHistoricItems",
                column: "RequestComponentId");

            migrationBuilder.CreateIndex(
                name: "RequestComponent_AK_RequestId_ComponentType",
                table: "RequestComponents",
                columns: new[] { "RequestId", "ComponentType" },
                unique: true);

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
                name: "IX_Requests_CurrencyId",
                table: "Requests",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WebsocketRequest_CurrencyPairId",
                table: "Requests",
                column: "WebsocketRequest_CurrencyPairId");

            migrationBuilder.CreateIndex(
                name: "Source_Index_Abbreviation",
                table: "Sources",
                column: "Abbreviation");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommandProperties_WebsocketCommandId",
                table: "WebsocketCommandProperties",
                column: "WebsocketCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommands_WebsocketRequestId",
                table: "WebsocketCommands",
                column: "WebsocketRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysedHistoricItems");

            migrationBuilder.DropTable(
                name: "CurrencyProperty");

            migrationBuilder.DropTable(
                name: "CurrencySources");

            migrationBuilder.DropTable(
                name: "RcdHistoricItems");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "RequestProperties");

            migrationBuilder.DropTable(
                name: "WebsocketCommandProperties");

            migrationBuilder.DropTable(
                name: "AnalysedComponents");

            migrationBuilder.DropTable(
                name: "RequestComponents");

            migrationBuilder.DropTable(
                name: "WebsocketCommands");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "CurrencyPairs");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "CurrencyTypes");
        }
    }
}
