using System;

namespace Lemoncode.Books.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public bool IsPublished => PublishedOn.HasValue;
        public Guid AuthorId { get; private set; }

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
