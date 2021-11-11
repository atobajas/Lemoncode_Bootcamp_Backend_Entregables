using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Domain
{
    public class Author
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? Birth { get; set; }
        public string CountryCode { get; set; } = string.Empty;

        private readonly List<Book> _books = new();
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        public Author(Guid id)
        {
            Id = id;
        }

        public void AddBook(Book book)
        {
            _books.Add(book);
        }
    }
}
