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

        public Guid CreateAuthor(string name, string lastname, string countryCode, string birth = null)
        {
            var newId = Guid.NewGuid();
            DateTime? birthDate = DateTime.Parse(birth).ToUniversalTime();
            var author = new Author(newId, name, lastname, birthDate, countryCode );

            _authorsRepository.AddAuthor(author);
            return newId;
        }
    }
}
