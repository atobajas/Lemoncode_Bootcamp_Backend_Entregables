using Lemoncode.Books.Domain;
using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application.Services
{
    public class AuthorsService
    {
        private readonly IBooksRepository _booksepository;
        public AuthorsService(IBooksRepository booksRepository)
        {
            _booksepository = booksRepository;
        }

        public Author GetAuthor(Guid id)
        {
            return _booksepository.GetAuthor(id);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _booksepository.GetAuthors();
        }

        public Guid CreateAuthor(Author author)
        {
            var newId = Guid.NewGuid();
            author.Birth =
                DateTime.TryParse(author.Birth.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;

            var newAuthor = new Author(newId, author.Name, author.LastName, author.Birth, author.CountryCode);

            _booksepository.AddAuthor(newAuthor);
            return newId;
        }

        public void ModifyAuthor(Guid id, Author author)
        {
            author.Birth =
                DateTime.TryParse(author.Birth.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;
            _booksepository.UpdateAuthor(id, author);
        }

        public void RemoveAuthor(Guid id)
        {
            _booksepository.RemoveAuthor(id);
        }
    }
}
