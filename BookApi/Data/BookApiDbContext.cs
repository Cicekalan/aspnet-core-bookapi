using BookApi.Models;
using Microsoft.EntityFrameworkCore;

public class BookApiDbContext : DbContext
{
    public BookApiDbContext(DbContextOptions<BookApiDbContext> options) :
    base(options)
    {

    }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

            
        modelBuilder.Entity<Author>().HasData(
            new Author { AuthorId = 1, Name = "John Ronald Reuel Tolkien", BirthDate = new DateTime(1892, 1, 3) },
            new Author { AuthorId = 2, Name = "George Orwell", BirthDate = new DateTime(1903, 6, 25) },
            new Author { AuthorId = 3, Name = "Jane Austen", BirthDate = new DateTime(1775, 12, 16) }
        );

        modelBuilder.Entity<Genre>().HasData(
            new Genre { GenreId = 1, Name = "Fantasy" },
            new Genre { GenreId = 2, Name = "Dystopian" },
            new Genre { GenreId = 3, Name = "Romance" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = 1, Title = "The Hobbit", Description = "A fantasy novel by J.R.R. Tolkien.", GenreId = 1 },
            new Book { BookId = 2, Title = "1984", Description = "A dystopian social science fiction novel by George Orwell.", GenreId = 2 },
            new Book { BookId = 3, Title = "Pride and Prejudice", Description = "A romantic novel by Jane Austen.", GenreId = 3 }
        );

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor { BookId = 1, AuthorId = 1 },
            new BookAuthor { BookId = 1, AuthorId = 2 },
            new BookAuthor { BookId = 2, AuthorId = 2 },
            new BookAuthor { BookId = 3, AuthorId = 3 }
        );
    }
}