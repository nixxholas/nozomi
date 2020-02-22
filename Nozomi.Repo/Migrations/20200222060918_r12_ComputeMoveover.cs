using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r12_ComputeMoveover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Computes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedById = table.Column<string>(type: "text", nullable: true),
                    Formula = table.Column<string>(type: "text", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedById = table.Column<string>(type: "text", nullable: true),
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
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    ComputeGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Value = table.Column<string>(type: "text", nullable: true),
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
                    ComputeGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedById = table.Column<string>(type: "text", nullable: true),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedById = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false),
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
                    ChildComputeGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentComputeGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedById = table.Column<string>(type: "text", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedById = table.Column<string>(type: "text", nullable: true),
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
    }
}
