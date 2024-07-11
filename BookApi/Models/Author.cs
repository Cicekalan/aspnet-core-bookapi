namespace BookApi.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public  ICollection<BookAuthor>? BookAuthors { get; set; }
        public DateTime BirthDate { get; set; }
    }
}