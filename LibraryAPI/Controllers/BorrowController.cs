using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/borrows")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;
        public BorrowController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all Borrow records.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<List<Borrow>>> GetBorrows()
        {
            // Get data from database
            var borrowsDomain = await _context.Borrows.ToListAsync();

            // Map domain to DTO
            var borrowsDto = new List<BorrowDto>();
            foreach (Borrow borrowDomain in borrowsDomain)
            {
                borrowsDto.Add(new BorrowDto()
                {
                    BorrowId = borrowDomain.BorrowId,
                    BookId = borrowDomain.BookId,
                    MemberId = borrowDomain.MemberId,
                    BorrowDate = borrowDomain.BorrowDate,
                    DueReturnDate = borrowDomain.DueReturnDate,
                    ActualReturnDate = borrowDomain.ActualReturnDate,

                });
            }

            // Return DTO
            return Ok(borrowsDto);
        }


        /// <summary>
        /// Handles return operation of a book
        /// </summary>
        /// <param name="borrowId"></param>
        /// <returns></returns>
        [HttpPut("return/{borrowId}")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            // Get the Borrow record from the database
            var borrow = await _context.Borrows.FirstOrDefaultAsync(borrowObject => borrowObject.BorrowId == borrowId);

            if (borrow == null)
            {
                return NotFound(new { Message = "No borrow record with that ID", Id = borrowId });
            }

            // Check if book is already returned
            if (borrow.ActualReturnDate.HasValue)
            {
                return BadRequest("Book has already been returned");
            }

            borrow.ActualReturnDate = DateTime.Now;

            _context.Borrows.Update(borrow);
            _context.SaveChanges();

            var book = await _context.Books.FirstOrDefaultAsync(bookObject => bookObject.BookId == borrow.BookId);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            book.AvailabilityStatus = "Available";

            await _context.SaveChangesAsync();

            return Ok("Book returned successfully");
        }
    }
}
