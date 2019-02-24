using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r4_AnalyticalModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    RequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AnalysedComponent_PK_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysedComponents_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Value = table.Column<string>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedComponents_RequestId",
                table: "AnalysedComponents",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysedHistoricItems_AnalysedComponentId",
                table: "AnalysedHistoricItems",
                column: "AnalysedComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysedHistoricItems");

            migrationBuilder.DropTable(
                name: "AnalysedComponents");
        }
    }
}
