using Lemoncode.Books.Application;
using Lemoncode.Books.Application.Entities;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Infra.Repository.EfCore
{
    public class EfCoreAuthorsRepository : IAuthorsRepository
    {
        private readonly BooksDbContext _booksDbContext;

        public EfCoreAuthorsRepository(BooksDbContext booksDbContext)
        {
            _booksDbContext = booksDbContext;
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

        public Author MapAuthorEntityToAuthor(AuthorEntity authorEntity)
        {
            var _efCoreBookRepository = new EfCoreBooksRepository(_booksDbContext);

            var author = new Author(
                authorEntity.AuthorGuid,
                authorEntity.Name,
                authorEntity.LastName,
                authorEntity.Birth,
                authorEntity.CountryCode
                );
            foreach (var localBookEntity in authorEntity.Books)
            {
                author.AddBook(_efCoreBookRepository.MapBookEntityToBook(localBookEntity));
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
    }
}
