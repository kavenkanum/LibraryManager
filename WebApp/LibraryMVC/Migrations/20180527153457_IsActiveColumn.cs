using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LibraryMVC.Migrations
{
    public partial class IsActiveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Books_BorrowedBooksID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BorrowedBooksID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BorrowedBooksID",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "BorrowedBooksID",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BorrowedBooksID",
                table: "Users",
                column: "BorrowedBooksID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Books_BorrowedBooksID",
                table: "Users",
                column: "BorrowedBooksID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
