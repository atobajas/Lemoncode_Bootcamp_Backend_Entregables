using System;

namespace Lemoncode.Books.Domain
{
    public class Book
    {
        public Guid Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public bool IsPublished => PublishedOn.HasValue;
        public Guid AuthorId { get; set; }

        public Book(Guid id)
        {
            Id = id;
            AuthorId = Guid.Empty;
        }

        public void AddAuthor(Guid authorCode)
        {
            AuthorId = authorCode;
        }
    }
}
