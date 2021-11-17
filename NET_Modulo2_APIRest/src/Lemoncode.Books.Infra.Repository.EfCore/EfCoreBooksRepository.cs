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
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            _booksDbContext.Books.Remove(bookEntity);
            _booksDbContext.SaveChanges();
        }

        public void UpdateBook(Guid id, Book newBook)
        {
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

        public static Book MapBookEntityToBook(BookEntity bookEntity)
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
            var author = _efCoreAuthorRepository.GetAuthor(book.AuthorId);
            var authorEntity = _efCoreAuthorRepository.MapAuthorToAuthorEntity(author);
            var bookEntity = new BookEntity()
            {
                BookGuid = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublishedOn = book.PublishedOn,
                AuthorId = book.AuthorId,
                Author = authorEntity
            };
            return bookEntity;
        }

        private AuthorEntity GetAuthorByCode(Guid authorId)
        {
            var authorEntity = _booksDbContext.Authors.Where(a => a.AuthorGuid == authorId).SingleOrDefault();
            return authorEntity;
        }

        private bool IsValidAuthor(Guid authorCode)
        {
            var _efCoreAuthorRepository = new EfCoreAuthorsRepository(_booksDbContext);
            var author = _efCoreAuthorRepository.GetAuthor(authorCode);
            return !(author is null);
        }
    }
}
