namespace BookApi.Dtos
{
    public record LoginDto
    {
        public string UserName {get; init;} = null!;
        public string Password {get; init;} = null!;
    }
}