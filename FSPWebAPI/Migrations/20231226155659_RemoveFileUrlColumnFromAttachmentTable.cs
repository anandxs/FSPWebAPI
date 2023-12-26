using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class RemoveFileUrlColumnFromAttachmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Attachments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Attachments",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");
        }
    }
}
