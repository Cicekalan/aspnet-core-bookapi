namespace BookApi.Dtos
{
    public record AuthorDto
    {
        public string? Name { get; init; }
        public DateTime BirthDate { get; init; }
    }
}