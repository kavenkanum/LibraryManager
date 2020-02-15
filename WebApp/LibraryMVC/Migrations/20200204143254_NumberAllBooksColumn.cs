using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryMVC.Migrations
{
    public partial class NumberAllBooksColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberAllBooks",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberAllBooks",
                table: "Books");
        }
    }
}
