using System;
using System.Text.Json.Serialization;

namespace Lemoncode.Books.Application.Models
{
    public class BookDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public int AuthorId { get; set; }
    }
}
