using Lemoncode.Books.Domain;
using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application
{
    public interface IAuthorsRepository
    {
        IEnumerable<Author> GetAuthors();
        Author GetAuthor(Guid id);
        void AddAuthor(Author author);
        void RemoveAuthor(Guid id);
        void UpdateAuthor(Guid id, Author author);
    }
}
