using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r9_DenominationAndDatumRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcdHistoricItems_RequestComponentData_RequestComponentDatum~",
                table: "RcdHistoricItems");

            migrationBuilder.DropTable(
                name: "RequestComponentData");

            migrationBuilder.RenameColumn(
                name: "RequestComponentDatumId",
                table: "RcdHistoricItems",
                newName: "RequestComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_RcdHistoricItems_RequestComponentDatumId",
                table: "RcdHistoricItems",
                newName: "IX_RcdHistoricItems_RequestComponentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDenominated",
                table: "RequestComponents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "RequestComponents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDenominated",
                table: "AnalysedComponents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_RcdHistoricItems_RequestComponents_RequestComponentId",
                table: "RcdHistoricItems",
                column: "RequestComponentId",
                principalTable: "RequestComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcdHistoricItems_RequestComponents_RequestComponentId",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "IsDenominated",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "IsDenominated",
                table: "AnalysedComponents");

            migrationBuilder.RenameColumn(
                name: "RequestComponentId",
                table: "RcdHistoricItems",
                newName: "RequestComponentDatumId");

            migrationBuilder.RenameIndex(
                name: "IX_RcdHistoricItems_RequestComponentId",
                table: "RcdHistoricItems",
                newName: "IX_RcdHistoricItems_RequestComponentDatumId");

            migrationBuilder.CreateTable(
                name: "RequestComponentData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    RequestComponentId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("RequestComponentDatum_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestComponentData_RequestComponents_RequestComponentId",
                        column: x => x.RequestComponentId,
                        principalTable: "RequestComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponentData_RequestComponentId",
                table: "RequestComponentData",
                column: "RequestComponentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RcdHistoricItems_RequestComponentData_RequestComponentDatum~",
                table: "RcdHistoricItems",
                column: "RequestComponentDatumId",
                principalTable: "RequestComponentData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
