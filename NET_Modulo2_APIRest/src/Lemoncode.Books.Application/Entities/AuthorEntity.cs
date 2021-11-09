using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application
{
    public class AuthorEntity
    {
        public int Id { get; set; }

        public Guid AuthorGuid { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime? Birth { get; set; }

        public string CountryCode { get; set; }

        // Navigation properties.
        public List<BookEntity> Books { get; set; } = new();
    }
}
