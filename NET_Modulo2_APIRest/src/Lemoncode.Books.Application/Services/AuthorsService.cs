using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Application.Services
{
    public class AuthorsService
    {
        private readonly BooksDbContext _booksDbContext;
        public AuthorsService(BooksDbContext booksDBContext)
        {
            _booksDbContext = booksDBContext;
        }

        public AuthorDto GetAuthor(int id)
        {
            var authorEntity =
                _booksDbContext
                    .Authors
                    .Include(x => x.Books)
                    .SingleOrDefault(x => x.Id == id);

            if (authorEntity is null)
            {
                throw new KeyNotFoundException($"El autor {id} no existe.");
            }

            return MapAuthorEntityToAuthorDto(authorEntity);
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            var authorEntities =
                _booksDbContext
                .Authors
                .Include(x => x.Books)
                .ToList();
            var authorsDtos = authorEntities.Select(MapAuthorEntityToAuthorDto);
            return authorsDtos;
        }

        public void CreateAuthor(AuthorDto newAuthorDto)
        {
            var newAuthorEntity = MapAuthorDtoToAuthorEntity(newAuthorDto);

            _booksDbContext.Add(newAuthorEntity);
            _booksDbContext.SaveChanges();

            newAuthorDto.Id = newAuthorEntity.Id;
        }

        public void ModifyAuthor(int id, UpdateAuthorDto newAuthorDto)
        {
            var authorEntity = _booksDbContext
                .Authors
                .SingleOrDefault(x => x.Id == id);
            if (authorEntity is null)
            {
                throw new KeyNotFoundException($"El autor {id} no existe.");
            }

            authorEntity.Name = newAuthorDto.Name;
            authorEntity.LastName = newAuthorDto.LastName;
            authorEntity.Birth = DateTime.TryParse(newAuthorDto.Birth.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null;
            authorEntity.CountryCode = newAuthorDto.CountryCode;

            _booksDbContext.SaveChanges();
        }

        public void RemoveAuthor(int id)
        {
            var authorEntity = _booksDbContext.Authors
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (authorEntity is null)
            {
                throw new KeyNotFoundException($"El autor {id} no existe.");
            }

            //if (authorEntity.Books.Count != 0)
            //{
                _booksDbContext.Authors.Remove(authorEntity);
                _booksDbContext.SaveChanges();
            //}
        }

        private AuthorDto MapAuthorEntityToAuthorDto(AuthorEntity authorEntity)
        {
            var authorDto = new AuthorDto()
            {
                Id = authorEntity.Id,
                Name = authorEntity.Name,
                LastName = authorEntity.LastName,
                Birth = DateTime.TryParse(authorEntity.Birth.ToString(), out DateTime temp)
                        ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                        : null,
                CountryCode = authorEntity.CountryCode,
                Books = authorEntity.Books.Select(BooksService.MapBookEntityToBookDto)
            };
            return authorDto;
        }

        private AuthorEntity MapAuthorDtoToAuthorEntity(AuthorDto authorDto)
        {
            var authorEntity = new AuthorEntity
            {
                Id = authorDto.Id,
                Name = authorDto.Name,
                LastName = authorDto.LastName,
                Birth = DateTime.TryParse(authorDto.Birth.ToString(), out DateTime temp)
                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                    : null,
                CountryCode = authorDto.CountryCode,
                Books = authorDto.Books.Select(BooksService.MapBookDtoToBookEntity).ToList()
            };
            return authorEntity;
        }
    }
}
