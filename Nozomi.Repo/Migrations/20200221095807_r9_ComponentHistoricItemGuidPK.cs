using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r9_ComponentHistoricItemGuidPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "RcdHistoricItem_PK_Id",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RcdHistoricItems");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RcdHistoricItems",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddPrimaryKey(
                name: "RcdHistoricItem_PK_Guid",
                table: "RcdHistoricItems",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "RcdHistoricItem_PK_Guid",
                table: "RcdHistoricItems");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RcdHistoricItems");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "RcdHistoricItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "RcdHistoricItem_PK_Id",
                table: "RcdHistoricItems",
                column: "Id");
        }
    }
}
