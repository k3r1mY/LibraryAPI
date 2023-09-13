using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/fines")]
    [ApiController]
    public class FineController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;
        public FineController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        // GET

        /// <summary>
        /// Returns all Fines.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Fine>>> GetFines()
        {
            // Get data from database
            var finesDomain = await _context.Fines.ToListAsync();

            // Map domain to DTO
            var finesDto = new List<FineDto>();
            foreach (Fine finedomain in finesDomain)
            {
                finesDto.Add(new FineDto()
                {
                    FineId = finedomain.FineId,
                    MemberId = finedomain.MemberId,
                    Amount = finedomain.Amount,
                    DatePaid = finedomain.DatePaid,
                });
            }

            // Return DTO
            return Ok(finesDto);
        }
    }
}
