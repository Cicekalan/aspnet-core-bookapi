namespace BookApi.Dtos
{
    public record BookUpdateDto
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
        public int[]? AuthorId { get; init; }
        public int GenreId { get; init; }
    }
}