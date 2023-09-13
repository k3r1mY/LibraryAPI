using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;
        public StaffController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all members of staff.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Staff>>> GetStaff()
        {
            var staffDomain = await _context.Staff.ToListAsync();

            var staffDto = new List<StaffDto>();
            foreach (Staff staffObj in staffDomain)
            {
                staffDto.Add(new StaffDto
                {
                    StaffId = staffObj.StaffId,
                    Name = staffObj.Name,
                    Position = staffObj.Position,
                    PhoneNumber = staffObj.PhoneNumber,
                });


            }

            return Ok(staffDto);


        }
    }
}
