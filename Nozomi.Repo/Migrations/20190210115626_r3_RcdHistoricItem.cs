using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r3_RcdHistoricItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "WebsocketCommandProperties",
                nullable: true);

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
                    Value = table.Column<string>(nullable: true, defaultValue: ""),
                    RequestComponentDatumId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RcdHistoricItem_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RcdHistoricItems_RequestComponentData_RequestComponentDatum~",
                        column: x => x.RequestComponentDatumId,
                        principalTable: "RequestComponentData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RcdHistoricItems_RequestComponentDatumId",
                table: "RcdHistoricItems",
                column: "RequestComponentDatumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "WebsocketCommandProperties");
        }
    }
}
