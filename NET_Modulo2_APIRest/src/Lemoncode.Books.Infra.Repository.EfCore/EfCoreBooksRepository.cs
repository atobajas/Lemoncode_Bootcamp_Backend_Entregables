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

        public Author GetAuthor(Guid id)
        {
            var authorEntity =
                _booksDbContext
                    .Authors
                    .Include(x => x.Books)
                    .SingleOrDefault(x => x.AuthorGuid == id);
            if (authorEntity is null)
            {
                throw new KeyNotFoundException($"El autor {id} no existe.");
            }
            return MapAuthorEntityToAuthor(authorEntity);
        }

        public IEnumerable<Author> GetAuthors()
        {
            var authorEntities =
                _booksDbContext
                .Authors
                .Include(x => x.Books)
                .ToList();
            var authors = authorEntities.Select(MapAuthorEntityToAuthor);
            return authors;
        }

        public void AddAuthor(Author author)
        {
            var authorEntity = MapAuthorToAuthorEntity(author);
            _booksDbContext.Authors.Add(authorEntity);
            _booksDbContext.SaveChanges();
        }

        public void RemoveAuthor(Guid id)
        {
            var authorEntity = _booksDbContext.Authors
                .Where(x => x.AuthorGuid == id)
                .FirstOrDefault();

            if (authorEntity is null)
            {
                throw new KeyNotFoundException($"El autor {id} no existe.");
            }

            _booksDbContext.Authors.Remove(authorEntity);
            _booksDbContext.SaveChanges();
        }

        public void UpdateAuthor(Guid id, Author newAuthor)
        {
            var authorEntity =
                _booksDbContext
                    .Authors
                    .SingleOrDefault(x => x.AuthorGuid == id);
            if (authorEntity is null)
            {
                throw new KeyNotFoundException($"El autor {id} no existe.");
            }
            var newAuthorEntity = MapAuthorToAuthorEntity(newAuthor);
            authorEntity.Name = newAuthorEntity.Name;
            authorEntity.LastName = newAuthorEntity.LastName;
            authorEntity.Birth = newAuthorEntity.Birth;
            authorEntity.CountryCode = newAuthorEntity.CountryCode;
            authorEntity.Books = newAuthorEntity.Books;
            _booksDbContext.SaveChanges();
        }

        public static Author MapAuthorEntityToAuthor(AuthorEntity authorEntity)
        {
            var author = new Author(
                authorEntity.AuthorGuid,
                authorEntity.Name,
                authorEntity.LastName,
                authorEntity.Birth,
                authorEntity.CountryCode
                );
            foreach (var localBookEntity in authorEntity.Books)
            {
                author.AddBook(EfCoreBooksRepository.MapBookEntityToBook(localBookEntity));
            }
            return author;
        }

        public AuthorEntity MapAuthorToAuthorEntity(Author author)
        {
            var _efCoreBookRepository = new EfCoreBooksRepository(_booksDbContext);
            var authorEntity = new AuthorEntity
            {
                AuthorGuid = author.Id,
                Name = author.Name,
                LastName = author.LastName,
                Birth = author.Birth,
                CountryCode = author.CountryCode,
            };
            foreach (var localBook in author.Books)
            {
                if (!(_efCoreBookRepository.GetBook(localBook.Id) is null))
                {
                    authorEntity.Books.Add(_efCoreBookRepository.MapBookToBookEntity(localBook));
                }
            };
            return authorEntity;
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
            var author = GetAuthor(book.AuthorId);
            var authorEntity = MapAuthorToAuthorEntity(author);
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
            var author = GetAuthor(authorCode);
            return !(author is null);
        }
    }
}
