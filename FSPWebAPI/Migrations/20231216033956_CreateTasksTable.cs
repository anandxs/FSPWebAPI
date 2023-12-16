using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class CreateTasksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssigneeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "StageId");
                    table.ForeignKey(
                        name: "FK_Tasks_TaskTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeId",
                table: "Tasks",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StageId",
                table: "Tasks",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TypeId",
                table: "Tasks",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
