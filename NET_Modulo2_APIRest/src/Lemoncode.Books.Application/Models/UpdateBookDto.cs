namespace Lemoncode.Books.Application.Models
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string? Title { get; init; } = default;
        public string? Description { get; init; } = default;
    }
}
