using Microsoft.AspNetCore.Mvc;

namespace BookApi.Models
{
[ApiExplorerSettings(IgnoreApi = true)]
public class BookAuthor
{
    public int BookId { get; set; }
    public   Book? Book { get; set; }
    public int AuthorId { get; set; }
    public   Author? Author { get; set; }
}
}