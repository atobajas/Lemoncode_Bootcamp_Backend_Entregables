using System;

namespace Lemoncode.Books.Application.Models
{
    public class UpdateAuthorDto
    {
        // Use for HTTP PUT
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birth { get; set; }
        public string CountryCode { get; set; }
    }
}
