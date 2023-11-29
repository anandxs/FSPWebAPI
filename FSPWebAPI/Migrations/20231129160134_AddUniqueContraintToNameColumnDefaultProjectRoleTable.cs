using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class AddUniqueContraintToNameColumnDefaultProjectRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DefaultProjectRoles_Name",
                table: "DefaultProjectRoles",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DefaultProjectRoles_Name",
                table: "DefaultProjectRoles");
        }
    }
}
