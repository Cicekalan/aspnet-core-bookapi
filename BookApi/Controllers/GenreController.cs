using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : ControllerBase
    {
        private readonly BookApiDbContext _context;

        public GenreController(BookApiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenres()
        {
            var genres = await _context.Genres.ToListAsync();
            if (!genres.Any())
                return NotFound();
            return Ok(genres);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetGenre(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);
            if (genre == null)
                return NotFound();
            return Ok(genre);
        }
        [HttpPost]
        public async Task<ActionResult<Genre>> CreateGenre([FromBody] string genreName)
        {
            try
            {
                if (genreName == null)
                {
                    return BadRequest("Genre data is required.");
                }
                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (existingGenre != null)
                {
                    return Conflict("Genre with the same name already exists.");
                }
                var genre = new Genre
                {
                    Name = genreName
                };
                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetGenre), new { id = genre.GenreId }, genre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Genre>> UpdateGenre(int id, [FromBody] string genreName)
        {
            try
            {
                var genre = await _context.Genres.FindAsync(id);

                if (genre == null)
                {
                    return NotFound();
                }

                if (genreName == null)
                {
                    return BadRequest("Genre data is required.");
                }

                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (existingGenre != null)
                {
                    return Conflict("Another genre with the same name already exists.");
                }

                genre.Name = genreName;

                _context.Genres.Update(genre);
                await _context.SaveChangesAsync();

                return Ok(genre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{genreId}")]
        public async Task<ActionResult<Genre>> DeleteGenre(int genreId)
        {
            try
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == genreId);
                if (genre == null)
                {
                    return NotFound($"Book with id {genreId} not found.");
                }

                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Book and associated authors deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }


}
