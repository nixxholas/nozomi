using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_WebsocketRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WebsocketRequest_CurrencyPairId",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "RequestComponents",
                nullable: true);

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
                    Delay = table.Column<long>(nullable: false),
                    WebsocketRequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsocketCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommands_Requests_WebsocketRequestId",
                        column: x => x.WebsocketRequestId,
                        principalTable: "Requests",
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
                    Value = table.Column<string>(nullable: true),
                    WebsocketCommandId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsocketCommandProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommandProperties_WebsocketCommands_WebsocketComma~",
                        column: x => x.WebsocketCommandId,
                        principalTable: "WebsocketCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WebsocketRequest_CurrencyPairId",
                table: "Requests",
                column: "WebsocketRequest_CurrencyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommandProperties_WebsocketCommandId",
                table: "WebsocketCommandProperties",
                column: "WebsocketCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommands_WebsocketRequestId",
                table: "WebsocketCommands",
                column: "WebsocketRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyPairs_WebsocketRequest_CurrencyPairId",
                table: "Requests",
                column: "WebsocketRequest_CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyPairs_WebsocketRequest_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "WebsocketCommandProperties");

            migrationBuilder.DropTable(
                name: "WebsocketCommands");

            migrationBuilder.DropIndex(
                name: "IX_Requests_WebsocketRequest_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WebsocketRequest_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "RequestComponents");
        }
    }
}
