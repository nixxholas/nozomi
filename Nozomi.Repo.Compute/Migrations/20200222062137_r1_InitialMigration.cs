using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Compute.Migrations
{
    public partial class r1_InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Computes",
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
                    Key = table.Column<string>(nullable: true),
                    Formula = table.Column<string>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Compute_PK_Guid", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ComputeExpressions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false, defaultValue: 1),
                    Expression = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ComputeGuid = table.Column<Guid>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ComputeExpression_PK_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ComputeExpressions_Computes_ComputeGuid",
                        column: x => x.ComputeGuid,
                        principalTable: "Computes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComputeValues",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ComputeGuid = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ComputeValue_CK_ComputeGuid_CreatedAt", x => new { x.ComputeGuid, x.CreatedAt });
                    table.UniqueConstraint("ComputeValue_AK_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ComputeValues_Computes_ComputeGuid",
                        column: x => x.ComputeGuid,
                        principalTable: "Computes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubComputes",
                columns: table => new
                {
                    ParentComputeGuid = table.Column<Guid>(nullable: false),
                    ChildComputeGuid = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SubCompute_CK_ChildComputeGuid_ParentComputeGuid", x => new { x.ChildComputeGuid, x.ParentComputeGuid });
                    table.ForeignKey(
                        name: "FK_SubComputes_Computes_ChildComputeGuid",
                        column: x => x.ChildComputeGuid,
                        principalTable: "Computes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubComputes_Computes_ParentComputeGuid",
                        column: x => x.ParentComputeGuid,
                        principalTable: "Computes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComputeExpressions_ComputeGuid",
                table: "ComputeExpressions",
                column: "ComputeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_SubComputes_ParentComputeGuid",
                table: "SubComputes",
                column: "ParentComputeGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputeExpressions");

            migrationBuilder.DropTable(
                name: "ComputeValues");

            migrationBuilder.DropTable(
                name: "SubComputes");

            migrationBuilder.DropTable(
                name: "Computes");
        }
    }
}
