using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.Books.Domain
{
    public class Author
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birth { get; set; }
        public string CountryCode { get; set; }

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
