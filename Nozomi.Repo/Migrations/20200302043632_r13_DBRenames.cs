using Microsoft.EntityFrameworkCore.Migrations;

namespace Nozomi.Repo.Migrations
{
    public partial class r13_DBRenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RcdHistoricItems_RequestComponents_RequestComponentId",
                table: "RcdHistoricItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestComponents_ComponentTypes_ComponentTypeId",
                table: "RequestComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestComponents_Requests_RequestId",
                table: "RequestComponents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RequestComponents_Guid",
                table: "RequestComponents");

            migrationBuilder.RenameTable(
                name: "RequestComponents",
                newName: "Components");

            migrationBuilder.RenameTable(
                name: "RcdHistoricItems",
                newName: "ComponentHistoricItems");

            migrationBuilder.RenameIndex(
                name: "IX_RequestComponents_RequestId",
                table: "Components",
                newName: "IX_Components_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestComponents_ComponentTypeId",
                table: "Components",
                newName: "IX_Components_ComponentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RcdHistoricItems_RequestComponentId",
                table: "ComponentHistoricItems",
                newName: "IX_ComponentHistoricItems_RequestComponentId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Components_Guid",
                table: "Components",
                column: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentHistoricItems_Components_RequestComponentId",
                table: "ComponentHistoricItems",
                column: "RequestComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_ComponentTypes_ComponentTypeId",
                table: "Components",
                column: "ComponentTypeId",
                principalTable: "ComponentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Requests_RequestId",
                table: "Components",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentHistoricItems_Components_RequestComponentId",
                table: "ComponentHistoricItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_ComponentTypes_ComponentTypeId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_Requests_RequestId",
                table: "Components");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Components_Guid",
                table: "Components");

            migrationBuilder.RenameTable(
                name: "Components",
                newName: "RequestComponents");

            migrationBuilder.RenameTable(
                name: "ComponentHistoricItems",
                newName: "RcdHistoricItems");

            migrationBuilder.RenameIndex(
                name: "IX_Components_RequestId",
                table: "RequestComponents",
                newName: "IX_RequestComponents_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Components_ComponentTypeId",
                table: "RequestComponents",
                newName: "IX_RequestComponents_ComponentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentHistoricItems_RequestComponentId",
                table: "RcdHistoricItems",
                newName: "IX_RcdHistoricItems_RequestComponentId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RequestComponents_Guid",
                table: "RequestComponents",
                column: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_RcdHistoricItems_RequestComponents_RequestComponentId",
                table: "RcdHistoricItems",
                column: "RequestComponentId",
                principalTable: "RequestComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComponents_ComponentTypes_ComponentTypeId",
                table: "RequestComponents",
                column: "ComponentTypeId",
                principalTable: "ComponentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComponents_Requests_RequestId",
                table: "RequestComponents",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
