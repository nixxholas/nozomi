using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_NpgsqlIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateSequence(
                name: "Id",
                startValue: 1000L);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WebsocketCommands",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WebsocketCommandProperties",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Sources",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Requests",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Requests",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RequestProperties",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RequestLogs",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RequestComponents",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RcdHistoricItems",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencyTypes",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencySources",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencyProperty",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencyPairs",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Currencies",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AnalysedHistoricItems",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AnalysedComponents",
                nullable: false,
                defaultValueSql: "nextval('\"Id\"')",
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "Id");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WebsocketCommands",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WebsocketCommandProperties",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Sources",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RequestProperties",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RequestLogs",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RequestComponents",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RcdHistoricItems",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencyTypes",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencySources",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencyProperty",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CurrencyPairs",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Currencies",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AnalysedHistoricItems",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AnalysedComponents",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "nextval('\"Id\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        }
    }
}
