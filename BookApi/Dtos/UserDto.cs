namespace BookApi.Dtos
{
    public record UserDto
    {
        public string Name { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}