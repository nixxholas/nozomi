using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r1_InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    RequestType = table.Column<int>(nullable: false),
                    ResponseType = table.Column<int>(nullable: false, defaultValue: 1),
                    DataPath = table.Column<string>(nullable: true),
                    Delay = table.Column<int>(nullable: false, defaultValue: 0),
                    FailureDelay = table.Column<long>(nullable: false, defaultValue: 3600000L),
                    CurrencyId = table.Column<long>(nullable: true),
                    CurrencyPairId = table.Column<long>(nullable: true),
                    CurrencyTypeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Request_PK_Id", x => x.Id);
                    table.UniqueConstraint("Request_AK_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "Currencies_CurrencyRequests_Constraint",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "CurrencyPair_CurrencyPairRequest_Constraint",
                        column: x => x.CurrencyPairId,
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "CurrencyType_Request_Constraint",
                        column: x => x.CurrencyTypeId,
                        principalTable: "CurrencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnalysedHistoricItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    RequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("WebsocketCommand_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommands_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RcdHistoricItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "IX_Requests_CurrencyId",
                table: "Requests",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyPairId",
                table: "Requests",
                column: "CurrencyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyTypeId",
                table: "Requests",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "Source_Index_Abbreviation",
                table: "Sources",
                column: "Abbreviation");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommandProperties_WebsocketCommandId",
                table: "WebsocketCommandProperties",
                column: "WebsocketCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommands_RequestId",
                table: "WebsocketCommands",
                column: "RequestId");
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
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "CurrencyPairs");

            migrationBuilder.DropTable(
                name: "CurrencyTypes");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
