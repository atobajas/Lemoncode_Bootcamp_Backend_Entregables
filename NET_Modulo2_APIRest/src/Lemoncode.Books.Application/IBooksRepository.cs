using Lemoncode.Books.Domain;
using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application
{
    public interface IBooksRepository
    {
        IEnumerable<BookEntity> GetBooks();
        BookEntity GetBook(Guid id);
        void AddBook(BookEntity book);
        void RemoveBook(Guid id);
        void UpdateBook(Guid id, BookEntity book);
        IEnumerable<AuthorEntity> GetAuthors();
        AuthorEntity GetAuthor(Guid id);
        void AddAuthor(AuthorEntity author);
        void RemoveAuthor(Guid id);
        void UpdateAuthor(Guid id, AuthorEntity author);
    }
}
