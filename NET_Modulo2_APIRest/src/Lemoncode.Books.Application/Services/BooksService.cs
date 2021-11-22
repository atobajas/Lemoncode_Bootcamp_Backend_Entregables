using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Domain;
using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application.Services
{
    public class BooksService
    {
        private readonly IBooksRepository _booksRepository;
        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public Book GetBook(Guid id)
        {
            return _booksRepository.GetBook(id);
        }

        public IEnumerable<Book> GetBooks()
        {
            return _booksRepository.GetBooks();
        }

        public Guid CreateBook(BookDto book)
        {
            var newId = Guid.NewGuid();

            var newBook = new Book(newId)
            {
                Title = book.Title,
                Description = book.Description,
                PublishedOn = DateTime.TryParse(book.PublishedOn.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null
            };
            newBook.AddAuthor(book.AuthorGuid);

            _booksRepository.AddBook(newBook);
            return book.Id;
        }

        public void ModifyBook(Guid id, Book book)
        {
            book.PublishedOn =
                DateTime.TryParse(book.PublishedOn.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;
            _booksRepository.UpdateBook(id, book);
        }

        public void RemoveBook(Guid id)
        {
            _booksRepository.RemoveBook(id);
        }

    }
}
