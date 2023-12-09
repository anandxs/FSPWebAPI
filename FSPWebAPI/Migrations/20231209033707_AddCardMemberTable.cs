using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class AddCardMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardMembers_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CardMembers_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardMembers_CardId",
                table: "CardMembers",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardMembers_MemberId",
                table: "CardMembers",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardMembers");
        }
    }
}
