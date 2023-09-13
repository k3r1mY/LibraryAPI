using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;

        public AuthorController(LibraryManagementSystemContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns author by id, or just all authors.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] int? id = null)
        {
            IQueryable<Author> query = _context.Authors;

            if (id.HasValue)
            {
                Author authorDomain = await query.FirstOrDefaultAsync(author => author.AuthorId == id.Value);

                if (authorDomain == null)
                {
                    return NotFound(new { Message = "No Author with that ID found.", Id = id });
                }

                AuthorDto authorDto = new AuthorDto
                {
                    AuthorId = authorDomain.AuthorId,
                    FirstName = authorDomain.FirstName,
                    LastName = authorDomain.LastName,
                    Nationality = authorDomain.Nationality,
                    Email = authorDomain.Email,
                };

                return Ok(authorDto);
            }

            List<Author> authorsDomain = await query.ToListAsync();
            List<AuthorDto> authorDtos = authorsDomain.Select(authorsDomain => new AuthorDto
            {
                AuthorId = authorsDomain.AuthorId,
                FirstName = authorsDomain.FirstName,
                LastName = authorsDomain.LastName,
                Nationality = authorsDomain.Nationality,
                Email = authorsDomain.Email,
            }).ToList();

            return Ok(authorDtos);
        }

        /// <summary>
        /// Add a new author.
        /// </summary>
        /// <param name="addAuthorRequestDto"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] AddAuthorRequestDto addAuthorRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author newAuthor = new Author
            {
                FirstName = addAuthorRequestDto.FirstName,
                LastName = addAuthorRequestDto.LastName,
                Email = addAuthorRequestDto.Email,
                Nationality = addAuthorRequestDto.Nationality,
            };

            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

            AuthorDto authorDto = new AuthorDto
            {
                AuthorId = newAuthor.AuthorId,
                FirstName = newAuthor.FirstName,
                LastName = newAuthor.LastName,
                Nationality = newAuthor.Nationality,
                Email = newAuthor.Email,
            };

            return CreatedAtAction(nameof(GetAuthors), new { id = authorDto.AuthorId }, authorDto);
        }
    }
}
