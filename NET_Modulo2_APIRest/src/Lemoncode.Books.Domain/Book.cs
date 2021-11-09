using System;

namespace Lemoncode.Books.Domain
{
    public class Book
    {
        public Guid Id { get; }
        public string Title { get; set; }

        public DateTime? PublishedOn { get; set; }

        public string Description { get; set; }

        public int AuthorId { get; set; }

        public Book(Guid id)
        {
            Id = id;
        }
    }
}
