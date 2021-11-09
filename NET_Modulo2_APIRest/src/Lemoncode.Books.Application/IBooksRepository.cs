using Lemoncode.Books.Domain;
using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetBooks();
        Book GetBook(Guid id);
        void AddBook(Book book);
        void RemoveBook(Guid id);
        void UpdateBook(Guid id, Book book);
    }
}
