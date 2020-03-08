using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r10_DynamicComponentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestComponents_Guid",
                table: "RequestComponents");

            migrationBuilder.DropIndex(
                name: "RequestComponent_AK_RequestId_ComponentType",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "ComponentType",
                table: "RequestComponents");

            migrationBuilder.AddColumn<long>(
                name: "ComponentTypeId",
                table: "RequestComponents",
                nullable: false,
                defaultValue: 666L);

            migrationBuilder.CreateTable(
                name: "ComponentTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ComponentType_PK_Id", x => x.Id);
                    table.UniqueConstraint("ComponentType_AK_Slug", x => x.Slug);
                });

            migrationBuilder.InsertData(
                table: "ComponentTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "Description", "IsEnabled", "ModifiedAt", "ModifiedById", "Name", "Slug" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Ask", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ask", "Ask" },
                    { 2002L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Money Supply (M3)", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "MoneySupplyM3", "MoneySupplyM3" },
                    { 2001L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Money Supply (M2)", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "MoneySupplyM2", "MoneySupplyM2" },
                    { 2000L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Money Supply (M1)", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "MoneySupplyM1", "MoneySupplyM1" },
                    { 1010L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "The current mining difficulty of this asset.", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Difficulty", "Difficulty" },
                    { 1005L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "The current block count of this crypto.", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "BlockCount", "BlockCount" },
                    { 1000L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "The current circulating supply of this asset.", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "CirculatingSupply", "CirculatingSupply" },
                    { 666L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Unknown", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Unknown", "Unknown" },
                    { 211L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Total supply amount for loans", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "TotalLoanSupply", "TotalLoanSupply" },
                    { 210L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Borrowing collateral factor", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "CollateralFactor", "CollateralFactor" },
                    { 209L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Supply reserve amount", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "SupplyReserve", "SupplyReserve" },
                    { 201L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Borrow % interest", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "BorrowRate", "BorrowRate" },
                    { 200L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Supply % interest", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "SupplyRate", "SupplyRate" },
                    { 101L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Price of the last successfully closed order", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "LastPrice", "LastPrice" },
                    { 100L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Order", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Order", "Order" },
                    { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Daily low", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Low", "Low" },
                    { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Daily high", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "High", "High" },
                    { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Daily volume", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DailyVolume", "DailyVolume" },
                    { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Daily price change expressed in percentage terms", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DailyChangePerc", "DailyChangePerc" },
                    { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Daily price change", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DailyChange", "DailyChange" },
                    { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Sum of the 25 lowest ask sizes", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "AskSize", "AskSize" },
                    { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Ask period covered in days", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "AskPeriod", "AskPeriod" },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Sum of the 25 highest bid sizes", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "BidSize", "BidSize" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Bid period covered in days", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "BidPeriod", "BidPeriod" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Flash Return Rate", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "FRR", "FRR" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Bid", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bid", "Bid" },
                    { 2003L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Money Supply (M4)", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "MoneySupplyM4", "MoneySupplyM4" },
                    { 2040L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Quasi-money consists of highly liquid assets which are not cash but can easily be converted into cash", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "QuasiMoneySupply", "QuasiMoneySupply" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_ComponentTypeId",
                table: "RequestComponents",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_RequestId",
                table: "RequestComponents",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComponents_ComponentTypes_ComponentTypeId",
                table: "RequestComponents",
                column: "ComponentTypeId",
                principalTable: "ComponentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComponents_ComponentTypes_ComponentTypeId",
                table: "RequestComponents");

            migrationBuilder.DropTable(
                name: "ComponentTypes");

            migrationBuilder.DropIndex(
                name: "IX_RequestComponents_ComponentTypeId",
                table: "RequestComponents");

            migrationBuilder.DropIndex(
                name: "IX_RequestComponents_RequestId",
                table: "RequestComponents");

            migrationBuilder.DropColumn(
                name: "ComponentTypeId",
                table: "RequestComponents");

            migrationBuilder.AddColumn<int>(
                name: "ComponentType",
                table: "RequestComponents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestComponents_Guid",
                table: "RequestComponents",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RequestComponent_AK_RequestId_ComponentType",
                table: "RequestComponents",
                columns: new[] { "RequestId", "ComponentType" },
                unique: true);
        }
    }
}
