using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Domain
{
    public class AuthorEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? Birth { get; set; }
        public string CountryCode { get; set; }
        // Navigation properties.
        public List<BookEntity> Books { get; set; } = new();
    }
}

//public class Author
//{
//    [Key]
//    public int Id { get; set; }

//    [Required, Column(TypeName = "nvarchar(max)")]
//    public string Name { get; set; }

//    [Column(TypeName = "nvarchar(max)")]
//    public string LastName { get; set; }

//    [Required, Column(TypeName = "datetime")]
//    public DateTime Birth { get; set; }

//    [Column(TypeName = "nvarchar(2)")]
//    public string CountryCode { get; set; }

//    public List<Book> Books = new List<Book>();
//}

