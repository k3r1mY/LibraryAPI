using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/publishers")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;

        public PublisherController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Publishers.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<List<Publisher>>> GetPublishers()
        {
            // Get data from database
            var publishersDomain = await _context.Publishers.ToListAsync();

            // Map domain to DTO
            var publishersDto = new List<PublisherDto>();
            foreach (Publisher publisherDomain in publishersDomain)
            {
                publishersDto.Add(new PublisherDto()
                {
                    PublisherId = publisherDomain.PublisherId,
                    Name = publisherDomain.Name,
                    Email = publisherDomain.Email,

                });
            }

            // Return DTO
            return Ok(publishersDto);
        }
    }
}
