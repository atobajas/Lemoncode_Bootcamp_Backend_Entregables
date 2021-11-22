using System;

namespace Lemoncode.Books.Application.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public Guid AuthorGuid { get; private set; }
    }
}
