using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApi.Dtos;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly BookApiDbContext _context;

        public AuthorController(BookApiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            if (!authors.Any())
                return NotFound();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetAuthor(int id)
        {
            var auther = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            if (auther == null)
                return NotFound();
            return Ok(auther);
        }
        [HttpPost]
        public async Task<ActionResult<Genre>> CreateAuthor([FromBody] string autherName, DateTime birthDate)
        {
            try
            {
                if (autherName == null)
                {
                    return BadRequest("Name data is required.");
                }

                var existingauthor = await _context.Authors.FirstOrDefaultAsync(g => g.Name == autherName);
                if (existingauthor != null)
                {
                    return Conflict("Author with the same name already exists.");
                }

                var author = new Author
                {
                    Name = autherName,
                    BirthDate = birthDate
                };

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Author>> UpdateAuthor(int id, [FromBody] AuthorDto authorDto)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound($"Author with id {id} not found.");
                }

                author.Name = authorDto.Name;
                author.BirthDate = authorDto.BirthDate;

                await _context.SaveChangesAsync();

                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{authorId}")]
        public async Task<ActionResult<Genre>> DeleteGenre(int authorId)
        {
            try
            {
                var genre = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
                if (genre == null)
                {
                    return NotFound($"Book with id {authorId} not found.");
                }

                _context.Authors.Remove(genre);
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