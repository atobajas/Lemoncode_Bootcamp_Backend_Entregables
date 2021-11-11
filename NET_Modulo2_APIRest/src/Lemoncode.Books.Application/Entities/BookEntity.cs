using System;

namespace Lemoncode.Books.Application.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }
        public Guid BookGuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }        
        public Guid AuthorId { get; set; }
        // Navigation properties
        public AuthorEntity Author { get; set; }
    }
}
