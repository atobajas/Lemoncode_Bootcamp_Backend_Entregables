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
        private readonly BooksDbContext _booksDbContext;

        public EfCoreBooksRepository(BooksDbContext booksDbContext)
        {
            _booksDbContext = booksDbContext;
        }

        public Book GetBook(Guid id)
        {
            var bookEntity =
                _booksDbContext
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
                _booksDbContext
                .Books
                .Include(x => x.Author)
                .ToList();
            var books = bookEntities.Select(MapBookEntityToBook);
            return books;
        }

        public void AddBook(Book book)
        {
            var bookEntity = MapBookToBookEntity(book);
            _booksDbContext.Books.Add(bookEntity);
            _booksDbContext.SaveChanges();
        }

        public void RemoveBook(Guid id)
        {
            var bookEntity = _booksDbContext.Books.SingleOrDefault(x => x.BookGuid == id);
            if (bookEntity != null)
            {
                _booksDbContext.Books.Remove(bookEntity);
                _booksDbContext.SaveChanges();
            }
        }

        public void UpdateBook(Guid id, Book book)
        {
            var bookEntity = _booksDbContext.Books.SingleOrDefault(x => x.BookGuid == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }
            var newBook = MapBookToBookEntity(book);
            bookEntity.Title = newBook.Title;
            bookEntity.Description = newBook.Description;
            bookEntity.PublishedOn = newBook.PublishedOn;
            bookEntity.AuthorId = newBook.AuthorId;
            bookEntity.Author = newBook.Author;
            _booksDbContext.SaveChanges();
        }

        public Book MapBookEntityToBook(BookEntity bookEntity)
        {
            var book = new Book(bookEntity.BookGuid)
            {
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                PublishedOn = bookEntity.PublishedOn,
            };
            book.AddAuthor(bookEntity.AuthorId);
            return book;
        }

        public BookEntity MapBookToBookEntity(Book book)
        {
            var _efCoreAuthorRepository = new EfCoreAuthorsRepository(_booksDbContext);
            var author = _efCoreAuthorRepository.GetAuthor(book.Id);
            var authorEntity = _efCoreAuthorRepository.MapAuthorToAuthorEntity(author);
            var bookEntity = new BookEntity()
            {
                BookGuid = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublishedOn = book.PublishedOn,
                AuthorId = book.AuthorId,
                Author = authorEntity
                //Author = GetAuthorByCode(book.AuthorId),
            };
            return bookEntity;
        }

        private AuthorEntity GetAuthorByCode(Guid authorId)
        {
            var authorEntity = _booksDbContext.Authors.Where(a => a.AuthorGuid == authorId).SingleOrDefault();
            return authorEntity;
        }
    }
}
