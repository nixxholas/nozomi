using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_BaseMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Sources",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Requests",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Requests",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RequestProperties",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RequestComponents",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PartialCurrencyPairs",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CurrencyTypes",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CurrencyPairs",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CurrencyPairComponents",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Currencies",
                type: "xid",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type = table.Column<int>(nullable: false),
                    RawPayload = table.Column<string>(nullable: true),
                    RequestId = table.Column<long>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_RequestId",
                table: "RequestLogs",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RequestProperties");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PartialCurrencyPairs");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CurrencyTypes");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CurrencyPairs");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CurrencyPairComponents");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Currencies");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "uuid_generate_v4()");
        }
    }
}
