using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LibraryMVC.Migrations
{
    public partial class DateOfBorrowAndReturnColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBorrow",
                table: "BorrowedBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow.AddDays(-1));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfReturn",
                table: "BorrowedBooks",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBorrow",
                table: "BorrowedBooks");

            migrationBuilder.DropColumn(
                name: "DateOfReturn",
                table: "BorrowedBooks");
        }
    }
}
