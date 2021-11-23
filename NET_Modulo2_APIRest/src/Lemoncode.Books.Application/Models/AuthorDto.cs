using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Lemoncode.Books.Application.Models
{
    public class AuthorDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birth { get; set; }
        public string CountryCode { get; set; }
        public IEnumerable<BookDto> Books { get; init; } = Enumerable.Empty<BookDto>();
    }
}
