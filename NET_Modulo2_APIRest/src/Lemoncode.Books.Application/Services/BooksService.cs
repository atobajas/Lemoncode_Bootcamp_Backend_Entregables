using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Application.Services
{
    public class BooksService
    {
        private readonly BooksDbContext _booksDbContext;
        public BooksService(BooksDbContext booksDBContext)
        {
            _booksDbContext = booksDBContext;
        }

        public BookDto GetBook(int id)
        {
            var bookEntity =
                _booksDbContext
                    .Books
                    .Include(x => x.Author)
                    .SingleOrDefault(x => x.Id == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            //var book = MapBookEntityToBook(bookEntity);
            return bookEntity;
        }

        public IEnumerable<BookDto> GetBooks()
        {
            var bookDtos =
                _booksDbContext
                .Books
                .Include(x => x.Author)
                .ToList();
                //.Select(MapBookEntityToBook);

            return bookDtos;
        }

        public void CreateBook(BookDto book)
        {

            var newBook = new BookEntity()
            {
                Title = book.Title,
                Description = book.Description,
                PublishedOn = DateTime.TryParse(book.PublishedOn.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null
            };

            _booksDbContext.Books.Add(newBook);
            _booksDbContext.SaveChanges();

            book.Id = newBook.Id;
        }

        public void ModifyBook(int id, BookDto book)
        {
            book.PublishedOn =
                DateTime.TryParse(book.PublishedOn.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;
            if (!IsValidAuthor(newBook.AuthorId))
            {
                throw new KeyNotFoundException($"No existe el autor {newBook.AuthorId}");
            }

            var bookEntity = _booksDbContext.Books.SingleOrDefault(x => x.BookGuid == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            var newBookEntity = MapBookToBookEntity(newBook);
            bookEntity.Title = newBookEntity.Title;
            bookEntity.Description = newBookEntity.Description;
            bookEntity.PublishedOn = newBookEntity.PublishedOn;
            bookEntity.AuthorId = newBookEntity.AuthorId;
            bookEntity.Author = newBookEntity.Author;
            _booksDbContext.SaveChanges();

        }

        public void RemoveBook(int id)
        {
            var bookEntity = _booksDbContext.Books.SingleOrDefault(x => x.Id == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            _booksDbContext.Books.Remove(bookEntity);
            _booksDbContext.SaveChanges();
        }

        private bool IsValidAuthor(Guid authorCode)
        {
            var author = GetAuthor(authorCode);
            return !(author is null);
        }
    }
}
