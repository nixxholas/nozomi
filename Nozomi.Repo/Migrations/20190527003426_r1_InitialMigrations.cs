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
                    LogoPath = table.Column<string>(nullable: false, defaultValue: "assets/svg/icons/question.svg"),
                    Abbreviation = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Denominations = table.Column<int>(nullable: false, defaultValue: 0),
                    DenominationName = table.Column<string>(nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
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
                values: new object[] { 55L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, null, 2L, 3000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1L, "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "United States Dollar", "USD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2027L, "CNX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Cryptonex", "CNX" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2349L, "XIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Mixin", "XIN" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1776L, "MCO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Crypto.com", "MCO" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1759L, "SNT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Status", "SNT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2299L, "ELF", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "aelf", "ELF" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1455L, "GNT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Golem", "GNT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1925L, "WTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Waltonchain", "WTC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3724L, "SOLVE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "SOLVE", "SOLVE" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2087L, "KCS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "KuCoin Shares", "KCS" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3116L, "INB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Insight Chain", "INB" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1886L, "DENT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Dent", "DENT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 291L, "MAID", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "MaidSafeCoin", "MAID" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3144L, "THR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "ThoreCoin", "THR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1343L, "STRAT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Stratis", "STRAT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2416L, "THETA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "THETA", "THETA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3224L, "QBIT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Qubitica", "QBIT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1866L, "BTM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bytom", "BTM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1230L, "STEEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Steem", "STEEM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1521L, "KMD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Komodo", "KMD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3437L, "ABBC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "ABBC Coin", "ABBC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1042L, "SC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Siacoin", "SC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2405L, "IOST", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "IOST", "IOST" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1700L, "AE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Aeternity", "AE" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3822L, "TFUEL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Theta Fuel", "TFUEL" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1320L, "ARDR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ardor", "ARDR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3607L, "VEST", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "VestChain", "VEST" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2308L, "DAI", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Dai", "DAI" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1414L, "XZC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Zcoin", "XZC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2090L, "LA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "LATOKEN", "LA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3890L, "MATIC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Matic Network", "MATIC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1789L, "PPT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Populous", "PPT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2092L, "NULS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "NULS", "NULS" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2135L, "R", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Revain", "R" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1807L, "SAN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Santiment Network Token", "SAN" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2588L, "LOOM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Loom Network", "LOOM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1934L, "LRC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Loopring", "LRC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2492L, "ELA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Elastos", "ELA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1703L, "ETP", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Metaverse ETP", "ETP" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2502L, "HT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Huobi Token", "HT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2062L, "AION", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Aion", "AION" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1966L, "MANA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Decentraland", "MANA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1750L, "GXC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "GXChain", "GXC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 213L, "MONA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "MonaCoin", "MONA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 460L, "CLAM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Clams", "CLAM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1229L, "DGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "DigixDAO", "DGD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1698L, "ZEN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Horizen", "ZEN" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1586L, "ARK", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ark", "ARK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2457L, "TRUE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "TrueChain", "TRUE" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2300L, "WAX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "WAX", "WAX" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2900L, "PAI", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Project Pai", "PAI" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1087L, "FCT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Factom", "FCT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3835L, "ORBS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Orbs", "ORBS" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2469L, "ZIL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Zilliqa", "ZIL" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2130L, "ENJ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Enjin Coin", "ENJ" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3330L, "PAX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Paxos Standard Token", "PAX" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 25L, "XEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "NEM", "XEM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 24L, "ETC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum Classic", "ETC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 23L, "ATOM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Cosmos", "ATOM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 22L, "XTZ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Tezos", "XTZ" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2603L, "NPXS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Pundi X", "NPXS" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 20L, "DASH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Dash", "DASH" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 19L, "XMR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Monero", "XMR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 18L, "BSV", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin SV", "BSV" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 17L, "TRX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "TRON", "TRX" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 16L, "ADA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Cardano", "ADA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 15L, "XLM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Stellar", "XLM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 26L, "NEO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "NEO", "NEO" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 14L, "BNB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Binance Coin", "BNB" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 12L, "BCH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin Cash", "BCH" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 11L, "XRP", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "XRP", "XRP" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 10L, "LTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Litecoin", "LTC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 8L, "USDT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Tether", "USDT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 7L, "BTS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "BitShares", "BTS" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 6L, "BCN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bytecoin", "BCN" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Denominations", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 5L, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, "Sat", 8, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin", "BTC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Denominations", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 4L, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Kyber Network Crystal", "KNC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Denominations", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3L, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, "Wei", 18, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ethereum", "ETH" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 9L, "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Singapore Dollar", "SGD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2L, "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Euro", "EUR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 13L, "EOS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "EOS", "EOS" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 27L, "MKR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Maker", "MKR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 21L, "MIOTA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "IOTA", "MIOTA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 29L, "ZEC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Zcash", "ZEC" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 693L, "XVG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Verge", "XVG" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2099L, "ICX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "ICON", "ICX" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 109L, "DGB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "DigiByte", "DGB" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2577L, "RVN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ravencoin", "RVN" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1896L, "ZRX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "0x", "ZRX" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 28L, "ONT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Ontology", "ONT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2222L, "BCD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin Diamond", "BCD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1104L, "REP", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Augur", "REP" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1567L, "NANO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Nano", "NANO" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2563L, "TUSD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "TrueUSD", "TUSD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2682L, "HOT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Holo", "HOT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3718L, "BTT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "BitTorrent", "BTT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1214L, "LSK", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Lisk", "LSK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1168L, "DCR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Decred", "DCR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 30L, "BAT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Basic Attention Token", "BAT" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 31L, "CRO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Crypto.com Chain", "CRO" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1274L, "WAVES", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Waves", "WAVES" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3077L, "VET", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "VeChain", "VET" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1975L, "LINK", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Chainlink", "LINK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2083L, "BTG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Bitcoin Gold", "BTG" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 74L, "DOGE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Dogecoin", "DOGE" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 2874L, "AOA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Aurora", "AOA" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1808L, "OMG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "OmiseGO", "OMG" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 1684L, "QTUM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "Qtum", "QTUM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Abbreviation", "CreatedAt", "CreatedBy", "CurrencyTypeId", "DeletedAt", "DeletedBy", "DenominationName", "Description", "IsEnabled", "ModifiedAt", "ModifiedBy", "Name", "Slug" },
                values: new object[] { 3408L, "USDC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2L, null, 0L, null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "USD Coin", "USDC" });

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
                    { 6L, "https://poloniex.com/public?command=returnTicker", "BTS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "BTC_BTS/lowestAsk", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 6L },
                    { 11L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", "SGD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "data/buy_price", null, 0L, true, "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 10L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "b", null, 0L, true, "KNC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 9L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", "BTC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 2, "b", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 7L, "https://api.bitfinex.com/v1/pubticker/etheur", "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1, "0", null, 0L, true, "ETH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 1L },
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
                    { 10L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 2L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 54L, 21, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 60L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 61L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 40L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 15L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 39L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 14L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 38L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 14L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 37L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 14L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 36L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 13L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 35L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 13L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 34L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 13L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 4L, 80, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 1L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 5L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 1L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 53L, 20, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 6L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 1L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 33L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 12L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 8L, 80, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 2L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0 a", null },
                    { 9L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 2L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 27L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 8L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 11L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 2L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 32L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 12L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 20L, 80, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 7L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0 a", null },
                    { 21L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 7L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]00", null },
                    { 22L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 7L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]00", null },
                    { 23L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 7L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 31L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 12L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 30L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 11L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 7L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 1L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 52L, 6, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 3L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, null, 3000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 41L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 15L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 26L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 8L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 25L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 8L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 24L, 80, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 8L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0 a", null },
                    { 19L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 6L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 18L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 6L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]00", null },
                    { 17L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 6L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]00", null },
                    { 16L, 80, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 6L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0 a", null },
                    { 15L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 5L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 14L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 5L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]00", null },
                    { 1L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 46L, 6, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 47L, 20, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 48L, 21, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 56L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 57L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 13L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 5L, null, 500, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]00", null },
                    { 12L, 80, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 5L, null, 1000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0 a", null },
                    { 45L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 16L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 2L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 49L, 6, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 50L, 20, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 51L, 21, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0 a", null },
                    { 58L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 59L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, null, 3000, null, 0L, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 44L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 16L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 43L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 16L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 42L, 70, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 15L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0[.]0", null },
                    { 29L, 11, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 11L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null },
                    { 28L, 10, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 11L, null, 10000, null, 0L, false, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "$ 0[.]00", null }
                });

            migrationBuilder.InsertData(
                table: "CurrencySources",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CurrencyId", "DeletedAt", "DeletedBy", "IsEnabled", "ModifiedAt", "ModifiedBy", "SourceId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L },
                    { 13L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 6L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 6L },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 4L, null, 0L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 3L },
                    { 6L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L },
                    { 7L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L },
                    { 3L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 3L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L },
                    { 12L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 6L },
                    { 16L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L },
                    { 17L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 9L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L },
                    { 10L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L },
                    { 18L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 6L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L },
                    { 4L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L },
                    { 14L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 7L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 6L },
                    { 15L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 8L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 6L },
                    { 20L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 10L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L },
                    { 2L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L },
                    { 19L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L },
                    { 11L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 5L },
                    { 9L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L },
                    { 8L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, null, 0L, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 4L }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("d13fc276-8077-49d2-ba38-998c58895df9"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("419db9ee-0510-47d1-8b14-620e2c86dcb4"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("b729acf9-a83c-4e76-8af8-a2ac7efc28c2"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 6L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("6f9d8fe7-71f4-42b8-ac31-526f559549a3"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 9L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.etherscan.io/api", 5000, null, 0L, "CurrencyRequest", new Guid("b7b9642e-357a-451c-9741-bf5a7fcb0ad1"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000, null, 0L, "CurrencyPairRequest", new Guid("1d8ba5ea-9d3a-4b02-b2d8-84ccd0851e69"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2, 3L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 20L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("92121fbb-8f01-45de-bfab-fe17aeac7174"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 16L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "WebsocketRequest_CurrencyPairId" },
                values: new object[] { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "wss://stream.binance.com:9443/stream?streams=!ticker@arr", null, 0L, "WebsocketRequest", new Guid("dc33dc82-26e5-4eef-af44-78e1efce2d1f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 50, 1, 10L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 19L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/LTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("58bf3728-1887-4460-bf61-6b898be360f3"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 15L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://insight.bitpay.com/api/status?q=getBlockCount", 90000, null, 0L, "CurrencyRequest", new Guid("31ceeb18-1d89-43d2-b215-0488d9417c67"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 18L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("ceb4e033-ebbb-45d9-9312-951f09228c30"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 14L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyId" },
                values: new object[] { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.coinranking.com/v1/public/coin/1?base=USD", 90000, null, 0L, "CurrencyRequest", new Guid("7f10715f-b5cc-4e52-9fa8-011311a5a2ca"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 5L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 17L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/ETHSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("49be3d33-d7b8-47aa-abf0-ee8765100b21"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 13L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 16L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCUSD", 10000, null, 0L, "CurrencyPairRequest", new Guid("fd199860-f699-4414-ba14-fdae9e856b5e"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 12L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tETHUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("096e9def-1c0f-4d1c-aa7b-273499f2cbda"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "CurrencyPairId" },
                values: new object[] { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.ethfinex.com/v2/ticker/tKNCUSD", 5000, null, 0L, "CurrencyPairRequest", new Guid("534ccff8-b6ff-4cce-961b-8458ef0ca5af"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 2L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://api.bitfinex.com/v1/pubticker/etheur", 2000, null, 0L, "CurrencyPairRequest", new Guid("ee593665-c6c5-454a-8831-b7e28265a1c8"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 7L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 15L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.coinhako.com/api/v1/price/currency/BTCSGD", 10000, null, 0L, "CurrencyPairRequest", new Guid("c162e683-cceb-4a03-aa24-f095b4d9db1f"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 11L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://www.alphavantage.co/query", 5000, null, 0L, "CurrencyPairRequest", new Guid("48ad7cb2-b2b7-41be-8540-64136b72883c"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 4L });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DataPath", "Delay", "DeletedAt", "DeletedBy", "Discriminator", "Guid", "IsEnabled", "ModifiedAt", "ModifiedBy", "RequestType", "ResponseType", "CurrencyPairId" },
                values: new object[] { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "https://poloniex.com/public?command=returnTicker", 5000, null, 0L, "CurrencyPairRequest", new Guid("e47e6062-e727-41ed-a0c1-750b1a792dd7"), true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0, 1, 8L });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 1L, 1000, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "result", 1L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 27L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "USDT_BTC/lowestAsk", 12L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 35L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "a", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 36L, 8, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "A", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 37L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "b", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 38L, 5, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "B", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 39L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/buy_price", 15L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 40L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/sell_price", 15L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 41L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/buy_price", 16L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 42L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/sell_price", 16L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 43L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/buy_price", 17L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 44L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/sell_price", 17L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 45L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/buy_price", 18L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 46L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/sell_price", 18L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 47L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/buy_price", 19L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 48L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/sell_price", 19L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 49L, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "data/buy_price", 20L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 50L, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, null, 0L, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "data/sell_price", 20L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 16L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "gesmes:Envelope/Cube/Cube/Cube/0=>@rate", 7L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 17L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "['Realtime Currency Exchange Rate']/['5. Exchange Rate']", 8L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 18L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "BTC_BCN/baseVolume", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 19L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "BTC_BCN/lowestAsk", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 20L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "BTC_BCN/highestBid", 9L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 21L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "BTC_BTS/baseVolume", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 22L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "BTC_BTS/lowestAsk", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 23L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "BTC_BTS/highestBid", 10L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 33L, 5, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "B", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 32L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "b", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 34L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>KNCETH", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "v", 14L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 30L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "a", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsDenominated", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 2L, 1000, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "result", 2L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 3L, 1005, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "info/blocks", 3L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 4L, 1010, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "info/difficulty", 3L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 31L, 8, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "A", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 6L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "7", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 7L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "2", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 8L, 8, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "3", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 9L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 10L, 5, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "1", 5L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 5L, 1000, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "data/coin/circulatingSupply", 4L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 12L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "2", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 29L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, "data/s=>ETHBTC", true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "v", 13L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 11L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "7", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 26L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "bid", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 25L, 1, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "ask", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 28L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "USDT_BTC/highestBid", 12L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 15L, 5, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "1", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 14L, 2, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "0", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 13L, 8, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "3", 6L, null });

            migrationBuilder.InsertData(
                table: "RequestComponents",
                columns: new[] { "Id", "ComponentType", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "IsEnabled", "ModifiedAt", "ModifiedBy", "QueryComponent", "RequestId", "Value" },
                values: new object[] { 24L, 12, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, null, true, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, "volume", 11L, null });

            migrationBuilder.InsertData(
                table: "RequestProperties",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "IsEnabled", "Key", "ModifiedAt", "ModifiedBy", "RequestId", "RequestPropertyType", "Value" },
                values: new object[,]
                {
                    { 7L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "apikey", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, 32, "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224" },
                    { 6L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "contractaddress", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, 32, "0xdd974d5c2e2928dea5f71b9825b8b646686bd200" },
                    { 8L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "apikey", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 8L, 300, "TV5HJJHNP8094BRO" },
                    { 10L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "from_currency", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 8L, 300, "EUR" },
                    { 11L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "to_currency", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 8L, 300, "USD" },
                    { 5L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "action", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, 32, "tokensupply" },
                    { 4L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "module", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 2L, 32, "stats" },
                    { 3L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "apikey", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, 32, "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224" },
                    { 2L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "action", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, 32, "ethsupply" },
                    { 1L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "module", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 1L, 32, "stats" },
                    { 9L, new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, null, 0L, true, "function", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 8L, 300, "CURRENCY_EXCHANGE_RATE" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedHistoricItems_AnalysedComponentId",
                table: "AnalysedHistoricItems",
                column: "AnalysedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CurrencyTypeId",
                table: "Currencies",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "Currency_Index_Slug",
                table: "Currencies",
                column: "Slug",
                unique: true);

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
