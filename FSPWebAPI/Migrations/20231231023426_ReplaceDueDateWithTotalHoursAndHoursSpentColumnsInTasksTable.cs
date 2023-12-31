using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSPWebAPI.Migrations
{
    public partial class ReplaceDueDateWithTotalHoursAndHoursSpentColumnsInTasksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Tasks");

            migrationBuilder.AddColumn<float>(
                name: "HoursSpent",
                table: "Tasks",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalHours",
                table: "Tasks",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursSpent",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);
        }
    }
}
