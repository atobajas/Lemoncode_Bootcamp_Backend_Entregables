using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Application.Services
{
    public class BooksService
    {
        private readonly BooksDbContext _booksDbContext;
        public BooksService(BooksDbContext booksDBContext)
        {
            _booksDbContext = booksDBContext;
        }

        public BookDto GetBook(int id)
        {
            var bookEntity =
                _booksDbContext
                    .Books
                    .Include(x => x.Author)
                    .SingleOrDefault(x => x.Id == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            var bookDto = MapBookEntityToBookDto(bookEntity);
            return bookDto;
        }

        public IEnumerable<BookDto> GetBooks()
        {
            var bookDtos =
                _booksDbContext
                .Books
                .Include(x => x.Author)
                .ToList()
                .Select(MapBookEntityToBookDto);

            return bookDtos;
        }

        public void CreateBook(BookDto newBookDto)
        {
            if (!IsValidAuthor(newBookDto.AuthorId))
            {
                throw new KeyNotFoundException($"No existe el autor {newBookDto.AuthorId}");
            }

            var newBookEntity = MapBookDtoToBookEntity(newBookDto);

            _booksDbContext.Books.Add(newBookEntity);
            _booksDbContext.SaveChanges();

            newBookDto.Id = newBookEntity.Id;
        }

        public void ModifyBook(int id, BookDto newBookDto)
        {
            if (!IsValidAuthor(newBookDto.AuthorId))
            {
                throw new KeyNotFoundException($"No existe el autor {newBookDto.AuthorId}");
            }

            var newBookEntity = _booksDbContext.Books.SingleOrDefault(x => x.Id == id);
            if (newBookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            newBookEntity.Title = newBookDto.Title;
            newBookEntity.Description = newBookDto.Description;
            newBookEntity.PublishedOn = DateTime.TryParse(newBookDto.PublishedOn.ToString(), out DateTime temp)
                                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                                    : null;
            newBookEntity.AuthorId = newBookDto.AuthorId;

            _booksDbContext.SaveChanges();
        }

        public void RemoveBook(int id)
        {
            var bookEntity = _booksDbContext.Books.SingleOrDefault(x => x.Id == id);
            if (bookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {id}");
            }

            _booksDbContext.Books.Remove(bookEntity);
            _booksDbContext.SaveChanges();
        }

        private bool IsValidAuthor(int authorCode)
        {
            var author = _booksDbContext.Authors.SingleOrDefault(x => x.Id == authorCode);
            return !(author is null);
        }

        internal static BookDto MapBookEntityToBookDto(BookEntity bookEntity)
        {
            var bookDto = new BookDto()
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                PublishedOn = DateTime.TryParse(bookEntity.PublishedOn.ToString(), out DateTime temp)
                                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                                    : null,
                AuthorId = bookEntity.AuthorId
            };
            return bookDto;
        }

        internal static BookEntity MapBookDtoToBookEntity(BookDto bookDto)
        {
            var bookEntity = new BookEntity()
            {
                Id = bookDto.Id,
                Title = bookDto.Title,
                Description = bookDto.Description,
                PublishedOn = DateTime.TryParse(bookDto.PublishedOn.ToString(), out DateTime temp)
                                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                                    : null,
                AuthorId = bookDto.AuthorId,
            };
            return bookEntity;
        }
    }
}
