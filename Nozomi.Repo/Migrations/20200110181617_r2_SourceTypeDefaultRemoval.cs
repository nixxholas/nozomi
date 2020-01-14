using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_SourceTypeDefaultRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SourceTypeGuid",
                table: "Sources",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("05b6457d-059c-458c-8774-0811e4d59ea8"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SourceTypeGuid",
                table: "Sources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("05b6457d-059c-458c-8774-0811e4d59ea8"),
                oldClrType: typeof(Guid));
        }
    }
}
