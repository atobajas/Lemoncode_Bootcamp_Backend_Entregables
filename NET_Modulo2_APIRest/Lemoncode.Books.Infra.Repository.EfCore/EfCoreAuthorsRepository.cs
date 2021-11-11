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
        private BooksDbContext _booksDbContext;

        public EfCoreAuthorsRepository(BooksDbContext booksDbContext)
        {
            booksDbContext = _booksDbContext;
        }

        public Author GetAuthor(Guid id)
        {
            var authorEntity = 
                _booksDbContext
                    .Authors
                    .Include(x => x.Books)
                    .SingleOrDefault(x => x.AuthorGuid == id);
            return MapAuthorEntityToAuthor(authorEntity);
        }

        private Author MapAuthorEntityToAuthor(AuthorEntity authorEntity)
        {
            var _efCoreBookRepository = new EfCoreBooksRepository(_booksDbContext);
            var author = new Author(authorEntity.AuthorGuid);
            author.Name = authorEntity.Name;
            author.LastName = authorEntity.LastName;
            author.Birth = authorEntity.Birth;
            author.CountryCode = authorEntity.CountryCode;
            var authorsEntities = authorEntity.Books;
            foreach(var localBook in authorsEntities)
            {
                author.AddBook(_efCoreBookRepository.MapBookEntityToBook(localBook));
            }
            return author;
        }

        public IEnumerable<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }

        public void AddAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void RemoveAuthor(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthor(Guid id, Author author)
        {
            throw new NotImplementedException();
        }
    }
}
