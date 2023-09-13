using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    nationality = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__author__86516BCF4899A81A", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genre_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__genre__18428D4218DB6F72", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    member_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    membership_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__member__B29B8534FB8C71AD", x => x.member_id);
                });

            migrationBuilder.CreateTable(
                name: "publisher",
                columns: table => new
                {
                    publisher_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__publishe__3263F29D0530DA37", x => x.publisher_id);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    staff_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    position = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__staff__1963DD9C3A56BDE3", x => x.staff_id);
                });

            migrationBuilder.CreateTable(
                name: "fine",
                columns: table => new
                {
                    fine_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    member_id = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    payment_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    date_paid = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__fine__F3C688D109268F49", x => x.fine_id);
                    table.ForeignKey(
                        name: "FK_fine_member",
                        column: x => x.member_id,
                        principalTable: "member",
                        principalColumn: "member_id");
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isbn = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    author_id = table.Column<int>(type: "int", nullable: true),
                    publisher_id = table.Column<int>(type: "int", nullable: true),
                    publish_year = table.Column<int>(type: "int", nullable: true),
                    genre_id = table.Column<int>(type: "int", nullable: true),
                    availability_status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    number_of_copies = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__books__490D1AE1C6700560", x => x.book_id);
                    table.ForeignKey(
                        name: "FK_books_author",
                        column: x => x.author_id,
                        principalTable: "author",
                        principalColumn: "author_id");
                    table.ForeignKey(
                        name: "FK_books_genre",
                        column: x => x.genre_id,
                        principalTable: "genre",
                        principalColumn: "genre_id");
                    table.ForeignKey(
                        name: "FK_books_publisher",
                        column: x => x.publisher_id,
                        principalTable: "publisher",
                        principalColumn: "publisher_id");
                });

            migrationBuilder.CreateTable(
                name: "borrow",
                columns: table => new
                {
                    borrow_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    member_id = table.Column<int>(type: "int", nullable: true),
                    borrow_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    due_return_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    actual_return_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__borrow__262B57A0A00BE494", x => x.borrow_id);
                    table.ForeignKey(
                        name: "FK_borrow_books",
                        column: x => x.book_id,
                        principalTable: "book",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK_borrow_member",
                        column: x => x.member_id,
                        principalTable: "member",
                        principalColumn: "member_id");
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    reservation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    member_id = table.Column<int>(type: "int", nullable: true),
                    reservation_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    pickup_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reservat__31384C29BBF48FE4", x => x.reservation_id);
                    table.ForeignKey(
                        name: "FK_reservation_books",
                        column: x => x.book_id,
                        principalTable: "book",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK_reservation_member",
                        column: x => x.member_id,
                        principalTable: "member",
                        principalColumn: "member_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_book_author_id",
                table: "book",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_genre_id",
                table: "book",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_publisher_id",
                table: "book",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_borrow_book_id",
                table: "borrow",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_borrow_member_id",
                table: "borrow",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_fine_member_id",
                table: "fine",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_book_id",
                table: "reservation",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_member_id",
                table: "reservation",
                column: "member_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "borrow");

            migrationBuilder.DropTable(
                name: "fine");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "publisher");
        }
    }
}
