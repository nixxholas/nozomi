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
                name: "CurrencyPairId1",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WebsocketCommand",
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
                    table.PrimaryKey("PK_WebsocketCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommand_Requests_WebsocketRequestId",
                        column: x => x.WebsocketRequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsocketCommandProperty",
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
                    table.PrimaryKey("PK_WebsocketCommandProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsocketCommandProperty_WebsocketCommand_WebsocketCommandId",
                        column: x => x.WebsocketCommandId,
                        principalTable: "WebsocketCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyPairId1",
                table: "Requests",
                column: "CurrencyPairId1");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommand_WebsocketRequestId",
                table: "WebsocketCommand",
                column: "WebsocketRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsocketCommandProperty_WebsocketCommandId",
                table: "WebsocketCommandProperty",
                column: "WebsocketCommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId1",
                table: "Requests",
                column: "CurrencyPairId1",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyPairs_CurrencyPairId1",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "WebsocketCommandProperty");

            migrationBuilder.DropTable(
                name: "WebsocketCommand");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CurrencyPairId1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrencyPairId1",
                table: "Requests");
        }
    }
}
