using System.Text.Json;
using System.Text.Json.Serialization;
using BookApi.Dtos;
using BookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers
{        
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly BookApiDbContext _context;

        public BookController(BookApiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBook()
        {
            var books = await _context.Books
                .Include(b => b.BookAuthors!)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.Genre)
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Description,
                    Authors = b.BookAuthors!.Select(ba => new
                    {
                        ba.Author!.Name,
                        ba.Author.BirthDate
                    })
                .ToList(),
                    Genre = b.Genre!.Name
                })
                .ToListAsync();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books
                    .Where(b => b.BookId == id)
                    .Include(b => b.BookAuthors!)
                        .ThenInclude(ba => ba.Author)
                    .Include(b => b.Genre)
                    .FirstOrDefaultAsync();

                if (book == null)
                {
                    return NotFound(); 
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] BookInsertDto bookInsertDto)
        {
            try
            {
                var book = new Book
                {
                    GenreId = bookInsertDto.GenreId,
                    Title = bookInsertDto.Title,
                    Description = bookInsertDto.Description
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                foreach (var authorId in bookInsertDto.AuthorId!)
                {
                    var bookAuthor = new BookAuthor
                    {
                        AuthorId = authorId,
                        BookId = book.BookId
                    };

                    _context.BookAuthors.Add(bookAuthor);
                }
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, new
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Message = "Book created successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("bookId")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
                if (book == null)
                {
                    return NotFound($"Book with id {bookId} not found.");
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                List<BookAuthor> bookAuthors = await _context.BookAuthors
                    .Where(ba => ba.BookId == bookId)
                    .ToListAsync();

                if (bookAuthors.Count > 0)
                {
                    _context.BookAuthors.RemoveRange(bookAuthors);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Book and associated authors deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }

}