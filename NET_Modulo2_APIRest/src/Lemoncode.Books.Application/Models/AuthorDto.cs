using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Application.Models
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birth { get; set; }
        public string CountryCode { get; set; }
        public IEnumerable<BookDto> Books { get; init; } = Enumerable.Empty<BookDto>();
    }
}
