using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityandSoftDeleteToToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ToDos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ToDos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ToDos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ToDos");
        }
    }
}
