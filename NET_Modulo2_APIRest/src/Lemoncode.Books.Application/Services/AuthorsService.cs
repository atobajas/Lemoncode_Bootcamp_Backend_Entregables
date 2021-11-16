using Lemoncode.Books.Domain;
using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Application.Services
{
    public class AuthorsService
    {
        private readonly IAuthorsRepository _authorsRepository;
        public AuthorsService(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }

        public Author GetAuthor(Guid id)
        {
            return _authorsRepository.GetAuthor(id);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _authorsRepository.GetAuthors();
        }

        public Guid CreateAuthor(Author author)
        {
            var newId = Guid.NewGuid();
            author.Birth =
                DateTime.TryParse(author.Birth.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;

            var newAuthor = new Author(newId, author.Name, author.LastName, author.Birth, author.CountryCode);

            _authorsRepository.AddAuthor(newAuthor);
            return newId;
        }

        public void ModifyAuthor(Guid id, Author author)
        {
            author.Birth =
                DateTime.TryParse(author.Birth.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;
            _authorsRepository.UpdateAuthor(id, author);
        }

        public void RemoveAuthor(Guid id)
        {
            _authorsRepository.RemoveAuthor(id);
        }
    }
}
