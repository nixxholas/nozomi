using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r7_CurrencyPairApiUrlOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "APIUrl",
                table: "ItemPairs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "APIUrl",
                table: "ItemPairs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
