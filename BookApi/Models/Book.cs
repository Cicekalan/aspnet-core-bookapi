namespace BookApi.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public   ICollection<BookAuthor>? BookAuthors { get; set; } 
        public int GenreId {get;set;}
        public Genre? Genre {get;set;}
    }
}