using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r2_RequestRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "CurrencyRequest_Currency_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_CurrencyPairs_WebsocketRequest_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsocketCommands_Requests_WebsocketRequestId",
                table: "WebsocketCommands");

            migrationBuilder.DropIndex(
                name: "IX_Requests_WebsocketRequest_CurrencyPairId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WebsocketRequest_CurrencyPairId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "WebsocketRequestId",
                table: "WebsocketCommands",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_WebsocketCommands_WebsocketRequestId",
                table: "WebsocketCommands",
                newName: "IX_WebsocketCommands_RequestId");

            migrationBuilder.AddColumn<long>(
                name: "CurrencyTypeId",
                table: "Requests",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CurrencyPairId",
                value: 9L);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CurrencyPairId",
                value: 10L);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrencyTypeId",
                table: "Requests",
                column: "CurrencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "Currencies_CurrencyRequests_Constraint",
                table: "Requests",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "CurrencyType_Request_Constraint",
                table: "Requests",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsocketCommands_Requests_RequestId",
                table: "WebsocketCommands",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Currencies_CurrencyRequests_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "CurrencyType_Request_Constraint",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsocketCommands_Requests_RequestId",
                table: "WebsocketCommands");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CurrencyTypeId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "WebsocketCommands",
                newName: "WebsocketRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_WebsocketCommands_RequestId",
                table: "WebsocketCommands",
                newName: "IX_WebsocketCommands_WebsocketRequestId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Requests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "WebsocketRequest_CurrencyPairId",
                table: "Requests",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 13L,
                column: "WebsocketRequest_CurrencyPairId",
                value: 9L);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 14L,
                column: "WebsocketRequest_CurrencyPairId",
                value: 10L);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WebsocketRequest_CurrencyPairId",
                table: "Requests",
                column: "WebsocketRequest_CurrencyPairId");

            migrationBuilder.AddForeignKey(
                name: "CurrencyPair_CurrencyPairRequest_Constraint",
                table: "Requests",
                column: "CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "CurrencyRequest_Currency_Constraint",
                table: "Requests",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_CurrencyPairs_WebsocketRequest_CurrencyPairId",
                table: "Requests",
                column: "WebsocketRequest_CurrencyPairId",
                principalTable: "CurrencyPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsocketCommands_Requests_WebsocketRequestId",
                table: "WebsocketCommands",
                column: "WebsocketRequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
