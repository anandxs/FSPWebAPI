using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class DropProjectRolesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_ProjectRoles_ProjectRoleId",
                table: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectRoleId",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ProjectRoleId",
                table: "ProjectMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectRoleId",
                table: "ProjectMembers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_ProjectRoles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectRoleId",
                table: "ProjectMembers",
                column: "ProjectRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_ProjectId",
                table: "ProjectRoles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_ProjectRoles_ProjectRoleId",
                table: "ProjectMembers",
                column: "ProjectRoleId",
                principalTable: "ProjectRoles",
                principalColumn: "RoleId");
        }
    }
}
