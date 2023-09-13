using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNumberOfCopiesFromBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_borrow_books",
                table: "borrow");

            migrationBuilder.DropForeignKey(
                name: "FK_borrow_member",
                table: "borrow");

            migrationBuilder.DropColumn(
                name: "number_of_copies",
                table: "book");

            migrationBuilder.AddForeignKey(
                name: "FK_borrow_books",
                table: "borrow",
                column: "book_id",
                principalTable: "book",
                principalColumn: "book_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_borrow_member",
                table: "borrow",
                column: "member_id",
                principalTable: "member",
                principalColumn: "member_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_borrow_books",
                table: "borrow");

            migrationBuilder.DropForeignKey(
                name: "FK_borrow_member",
                table: "borrow");

            migrationBuilder.AddColumn<int>(
                name: "number_of_copies",
                table: "book",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_borrow_books",
                table: "borrow",
                column: "book_id",
                principalTable: "book",
                principalColumn: "book_id");

            migrationBuilder.AddForeignKey(
                name: "FK_borrow_member",
                table: "borrow",
                column: "member_id",
                principalTable: "member",
                principalColumn: "member_id");
        }
    }
}
