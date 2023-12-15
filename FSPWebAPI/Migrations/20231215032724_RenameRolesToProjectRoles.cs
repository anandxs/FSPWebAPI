using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class RenameRolesToProjectRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Roles_RoleId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Projects_ProjectId",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "ProjectRoles");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_ProjectId",
                table: "ProjectRoles",
                newName: "IX_ProjectRoles_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectRoles",
                table: "ProjectRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_ProjectRoles_RoleId",
                table: "ProjectMembers",
                column: "RoleId",
                principalTable: "ProjectRoles",
                principalColumn: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRoles_Projects_ProjectId",
                table: "ProjectRoles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_ProjectRoles_RoleId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRoles_Projects_ProjectId",
                table: "ProjectRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectRoles",
                table: "ProjectRoles");

            migrationBuilder.RenameTable(
                name: "ProjectRoles",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectRoles_ProjectId",
                table: "Roles",
                newName: "IX_Roles_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Roles_RoleId",
                table: "ProjectMembers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Projects_ProjectId",
                table: "Roles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
