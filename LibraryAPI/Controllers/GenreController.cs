using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic;

namespace LibraryAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly LibraryManagementSystemContext _context;

        public GenreController(LibraryManagementSystemContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns all Genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Genre>>> GetGenres()
        {
            // Get data from database
            var genresDomain = await _context.Genres.ToListAsync();

            // Map domain to DTO
            var genresDto = new List<GenreDto>();
            foreach (Genre genreDomain in genresDomain)
            {
                genresDto.Add(new GenreDto()
                {
                    GenreId = genreDomain.GenreId,
                    GenreName = genreDomain.GenreName,
                    Description = genreDomain.Description,

                });
            }

            // Return DTO
            return Ok(genresDto);
        }

        /// <summary>
        /// Add a new Genre.
        /// </summary>
        /// <param name="genreRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddGenre([FromBody] AddGenreRequestDto genreRequestDto)
        {
            Genre newGenre = new Genre
            {
                GenreName = genreRequestDto.GenreName,
                Description = genreRequestDto.Description,
            };

            _context.Genres.Add(newGenre);
            _context.SaveChanges();

            GenreDto genreDto = new GenreDto
            {
                GenreId = newGenre.GenreId,
                GenreName = newGenre.GenreName,
                Description = newGenre.Description,
            };



            return CreatedAtAction(nameof(GetGenres), new { id = genreDto.GenreId }, genreDto);
        }
    }
}




