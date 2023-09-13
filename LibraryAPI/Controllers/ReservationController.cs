using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;

        public ReservationController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Reservations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetReservations()
        {
            // Get data from database
            var reservationsDomain = await _context.Reservations.ToListAsync();

            // Map domain to DTO
            var reservationDto = new List<ReservationDto>();
            foreach (Reservation reservationDomain in reservationsDomain)
            {
                reservationDto.Add(new ReservationDto()
                {
                    ReservationId = reservationDomain.ReservationId,
                    BookId = reservationDomain.BookId,
                    MemberId = reservationDomain.MemberId,
                    ReservationDate = reservationDomain.ReservationDate,
                    PickupDate = reservationDomain.PickupDate,

                });
            }

            // Return DTO
            return Ok(reservationDto);
        }

    }
}
