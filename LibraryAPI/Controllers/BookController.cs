using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;

        public BookController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Search books based on Genre, title and author or just display all books.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="authorName"></param>
        /// <param name="genreName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? title = "", [FromQuery] string? authorName = "", [FromQuery] string? genreName = "")
        {
            int skip = ((pageNumber - 1) * pageSize) ?? 0;
            skip = Math.Max(0, skip); // Ensure skip is non-negative



            string lowerCaseTitle = title?.ToLower();
            string lowerCaseAuthor = authorName?.ToLower();
            string lowerCaseGenre = genreName?.ToLower();

            // Query the database based on the provided search criteria
            IQueryable<Book> query = _context.Books.Include(b => b.Author).Include(b => b.Genre);

            // If no ID is provided, continue with other search criteria
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(book => book.Title.ToLower().Contains(lowerCaseTitle));
                var matchingBooks = await query.ToListAsync();
                if (!matchingBooks.Any())
                {
                    return NotFound("No book with that title found.");
                }
            }

            if (!string.IsNullOrEmpty(authorName))
            {
                query = query.Where(book => book.Author.FirstName.ToLower().Contains(lowerCaseAuthor) || book.Author.LastName.ToLower().Contains(lowerCaseAuthor));

                if (!await query.AnyAsync())
                {
                    return NotFound("No book by that author found.");
                }
            }

            if (!string.IsNullOrEmpty(genreName))
            {
                query = query.Where(book => book.Genre.GenreName.ToLower().Contains(lowerCaseGenre));

                if (!await query.AnyAsync())
                {
                    return NotFound("No book with that genre found.");
                }
            }

            query = query.Skip(skip).Take(pageSize ?? 10);

            List<Book> booksDomain = await query.ToListAsync();

            // Map domain to DTO
            List<BookDto> booksDto = booksDomain.Select(bookDomain => new BookDto
            {
                BookId = bookDomain.BookId,
                Isbn = bookDomain.Isbn,
                Title = bookDomain.Title,
                AuthorName = bookDomain.Author.FirstName + " " + bookDomain.Author.LastName,
                AuthorId = bookDomain.AuthorId,
                PublisherId = bookDomain.PublisherId,
                GenreName = bookDomain.Genre.GenreName,
                PublishYear = bookDomain.PublishYear,
                GenreId = bookDomain.GenreId,
                AvailabilityStatus = bookDomain.AvailabilityStatus,
            }).ToList();

            // Count the total number of books in query (without pagination)
            int totalBooks = await _context.Books.CountAsync();

            // Calculate number of pages
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize.Value);

            // Determine if there's a next page
            bool hasNextPage = (pageNumber < totalPages);

            // Determine if there's a previous page
            bool hasPrevPage = (pageNumber > 1);

            var paginationDetails = new
            {
                Page = pageNumber,
                Size = pageSize,
                HasNextPage = hasNextPage,
                HasPrevPage = hasPrevPage,
                TotalPages = totalPages,
            };

            var response = new
            {
                Books = booksDto,
                Pagination = paginationDetails,
            };

            // Return DTO
            return Ok(response);
        }


        /// <summary>
        /// Get a book by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            // Find book with given id in database
            var bookDomain = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(book => book.BookId == id);

            if (bookDomain == null)
            {
                return NotFound("Book of that ID was not found.");
            }

            // Map domain to DTO
            BookDto bookDto = new BookDto
            {
                BookId = bookDomain.BookId,
                Isbn = bookDomain.Isbn,
                Title = bookDomain.Title,
                AuthorName = bookDomain.Author.FirstName + " " + bookDomain.Author.LastName,
                AuthorId = bookDomain.AuthorId,
                PublisherId = bookDomain.PublisherId,
                GenreName = bookDomain.Genre.GenreName,
                PublishYear = bookDomain.PublishYear,
                GenreId = bookDomain.GenreId,
                AvailabilityStatus = bookDomain.AvailabilityStatus,
            };

            return Ok(bookDto);
        }


        /// <summary>
        /// Posts a new book to the database while incrementing the id automatically
        /// </summary>
        /// <param name="addBookRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequestDto addBookRequestDto)
        {
            // Check if the provided AuthorId exists in the Authors table
            bool authorExists = await _context.Authors.AnyAsync(authorObject => authorObject.AuthorId == addBookRequestDto.AuthorId);
            if (!authorExists)
            {
                ModelState.AddModelError("AuthorId", "Invalid AuthorId. Author with the specified ID does not exist.");
                return BadRequest(ModelState);
            }

            // Check if the provided GenreId exists in the Genres table
            bool genreExists = await _context.Genres.AnyAsync(genreObject => genreObject.GenreId == addBookRequestDto.GenreId);
            if (!genreExists)
            {
                ModelState.AddModelError("GenreId", "Invalid GenreId. Genre with the specified ID does not exist.");
                return BadRequest(ModelState);
            }

            // Check if the provided PublisherId exists in the Publishers table (if applicable)
            if (addBookRequestDto.PublisherId.HasValue)
            {
                bool publisherExists = await _context.Publishers.AnyAsync(publisherObject => publisherObject.PublisherId == addBookRequestDto.PublisherId);
                if (!publisherExists)
                {
                    ModelState.AddModelError("PublisherId", "Invalid PublisherId. Publisher with the specified ID does not exist.");
                    return BadRequest(ModelState);
                }
            }

            // Map DTO to domain model
            var bookDomainModel = new Book
            {
                Isbn = addBookRequestDto.Isbn,
                Title = addBookRequestDto.Title,
                AuthorId = addBookRequestDto.AuthorId,
                PublisherId = addBookRequestDto.PublisherId,
                PublishYear = addBookRequestDto.PublishYear,
                GenreId = addBookRequestDto.GenreId,
                AvailabilityStatus = "Available",
            };

            // Use domain model to create book object
            _context.Books.Add(bookDomainModel);
            await _context.SaveChangesAsync();

            // Map domain model back to dto
            BookDto bookDto = new BookDto
            {
                Isbn = bookDomainModel.Isbn,
                BookId = bookDomainModel.BookId,
                Title = bookDomainModel.Title,
                AuthorId = bookDomainModel.AuthorId,
                PublisherId = bookDomainModel.PublisherId,
                PublishYear = bookDomainModel.PublishYear,
                GenreId = bookDomainModel.GenreId,
                AvailabilityStatus = "Available",
            };

            return CreatedAtAction(nameof(GetBookById), new { id = bookDto.BookId }, bookDto);
        }



        /// <summary>
        /// Handles checkout operation of a book
        /// </summary>
        /// <param name="addBorrowRequestDto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/checkout")]
        public async Task<IActionResult> CheckoutBook([FromBody] AddBorrowRequestDto addBorrowRequestDto, [FromRoute] int id)
        {
            // Check if the provided MemberId exists in the Members table
            bool memberExists = await _context.Members.AnyAsync(memberObject => memberObject.MemberId == addBorrowRequestDto.MemberId);
            if (!memberExists)
            {
                ModelState.AddModelError("MemberId", "Invalid MemberId. Member with the specified ID does not exist.");
                return BadRequest(ModelState);
            }

            // Check if the provided BookId exists in the Books table
            bool bookExists = await _context.Books.AnyAsync(bookObject => bookObject.BookId == id);
            if (!bookExists)
            {
                ModelState.AddModelError("BookId", "Invalid BookId. Book with the specified ID does not exist.");
                return BadRequest(ModelState);
            }

            // Check if the book is available for checkout
            var book = await _context.Books.FirstOrDefaultAsync(bookObject => bookObject.BookId == id);
            if (book.AvailabilityStatus.ToLower() != "available")
            {
                ModelState.AddModelError("BookId", "Book is not available for checkout.");
                return BadRequest(ModelState);
            } else
            {
                return BadRequest(ModelState);
            }

            // Create a new borrow record
            var borrow = new Borrow
            {
                BookId = id,
                MemberId = addBorrowRequestDto.MemberId,
                BorrowDate = DateTime.Now,
                DueReturnDate = DateTime.Now.AddDays(30), // Assuming a borrow duration of 30 days
                ActualReturnDate = null // The book is not yet returned
            };

            _context.Borrows.Add(borrow);

            // Update book availability status
            book.AvailabilityStatus = "Borrowed";

            await _context.SaveChangesAsync();

            // Return a 201 Created response with the newly created borrow record
            BorrowDto borrowDto = new BorrowDto
            {
                BorrowId = borrow.BorrowId,
                BookId = borrow.BookId,
                MemberId = borrow.MemberId,
                BorrowDate = borrow.BorrowDate,
                DueReturnDate = borrow.DueReturnDate,
                ActualReturnDate = borrow.ActualReturnDate
            };


            return Ok(borrowDto);
        }



        /// <summary>
        /// Updates a book id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addBookRequestDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, AddBookRequestDto addBookRequestDto)
        {
            Book existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null)
            {
                return NotFound("No book with that id");
            }

            Author existingAuthor = await _context.Authors.FindAsync(addBookRequestDto.AuthorId);
            Genre existingGenre = await _context.Genres.FindAsync(addBookRequestDto.GenreId);

            if (existingAuthor == null || existingGenre == null)
            {
                // Handle the case where the specified AuthorId or GenreId does not exist in the database
                return BadRequest("Invalid AuthorId or GenreId");
            }

            // Update the Author and Genre properties of the existingBook
            existingBook.Author = existingAuthor;
            existingBook.Genre = existingGenre;

            existingBook.Isbn = addBookRequestDto.Isbn;
            existingBook.Title = addBookRequestDto.Title;
            existingBook.AuthorId = addBookRequestDto.AuthorId;
            existingBook.GenreId = addBookRequestDto.GenreId;
            existingBook.PublisherId = addBookRequestDto.PublisherId;
            existingBook.PublishYear = addBookRequestDto.PublishYear;

            await _context.SaveChangesAsync();

            // Map to DTO

            BookDto bookDto = new BookDto
            {
                Isbn = existingBook.Isbn,
                Title = existingBook.Title,
                AuthorId = existingBook.AuthorId,
                GenreId = existingBook.GenreId,
                PublisherId = existingBook.PublisherId,
                PublishYear = existingBook.PublishYear,
                AvailabilityStatus = existingBook.AvailabilityStatus,
                AuthorName = existingBook.Author.FirstName + " " + existingBook.Author.LastName,
                GenreName = existingBook.Genre.GenreName,

            };

            return Ok(bookDto);

        }

        /// <summary>
        /// Delete a book using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // Find the book with the given id in the database
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                // If the book with the given id is not found, return NotFound
                return NotFound("Book of that id was not found.");
            }

            // Remove the book from the database context
            _context.Books.Remove(book);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return a 204 No Content response to indicate successful deletion
            return NoContent();
        }



        /// <summary>
        /// A test case for exceptions
        /// </summary>
        /// <returns></returns>
        [HttpGet("test-exception")]
        public IActionResult TestException()
        {
            try
            {
                // Simulate an exception by throwing a custom exception
                throw new Exception("This is a test exception.");
            }
            catch (Exception ex)
            {
                // Catch the exception and handle it (optional)
                // You can log the exception or perform additional actions if needed.

                // Re-throw the exception to trigger the global exception handling
                throw;
            }

            // The code above will throw a custom exception, and the global exception handler should catch it.
            // The custom middleware you implemented in the previous steps should handle the exception.

            // Since an exception is thrown above, the following code won't be reached.
            return Ok("This won't be returned.");
        }



    }
}
