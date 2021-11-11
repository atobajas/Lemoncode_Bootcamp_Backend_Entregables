using Lemoncode.Books.Application;
using Lemoncode.Books.Application.Entities;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Infra.Repository.EfCore
{
    public class EfCoreBooksRepository : IBooksRepository
    {
        private BooksDbContext _booksContext;

        public EfCoreBooksRepository(BooksDbContext booksContext)
        {
            _booksContext = booksContext;
        }

        public Book GetBook(Guid id)
        {
            var bookEntity =
                _booksContext
                    .Books
                    .Include(x => x.Author)
                    .SingleOrDefault(x => x.BookGuid == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            var book = MapBookEntityToBook(bookEntity);
            return book;
        }

        public IEnumerable<Book> GetBooks()
        {
            var bookEntities =
                _booksContext
                .Books
                .Include(x => x.Author)
                .ToList();
            var books = bookEntities.Select(MapBookEntityToBook);
            return books;
        }

        public void AddBook(Book book)
        {
            var bookEntity = MapBookToBookEntity(book);
            _booksContext.Books.Add(bookEntity);
            _booksContext.SaveChanges();
        }

        public void RemoveBook(Guid id)
        {
            var bookEntity = _booksContext.Books.SingleOrDefault(x => x.BookGuid == id);
            if (bookEntity != null)
            {
                _booksContext.Books.Remove(bookEntity);
                _booksContext.SaveChanges();
            }
        }

        public void UpdateBook(Guid id, Book book)
        {
            var bookEntity = _booksContext.Books.SingleOrDefault(x => x.BookGuid == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }
            var updateBook = MapBookToBookEntity(book);
            bookEntity.Title = updateBook.Title;
            bookEntity.Description = updateBook.Description;
            bookEntity.PublishedOn = updateBook.PublishedOn;
            bookEntity.AuthorId = updateBook.AuthorId;
            bookEntity.Author = updateBook.Author;
            _booksContext.SaveChanges();
        }

        public Book MapBookEntityToBook(BookEntity bookEntity)
        {
            var book = new Book(bookEntity.BookGuid);
            book.Title = bookEntity.Title;
            book.Description = bookEntity.Description;
            book.PublishedOn = bookEntity.PublishedOn;
            book.AddAuthor(bookEntity.AuthorId);
            return book;
        }

        public BookEntity MapBookToBookEntity(Book book)
        {
            var bookEntity = new BookEntity()
            {
                BookGuid = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublishedOn = book.PublishedOn,
                AuthorId = book.AuthorId,
                Author = GetAuthorByCode(book.AuthorId),
            };
            return bookEntity;
        }

        private AuthorEntity GetAuthorByCode(Guid authorId)
        {
            var authorEntity = _booksContext.Authors.Where(a => a.AuthorGuid == authorId).SingleOrDefault();
            return authorEntity;
        }
    }
}
