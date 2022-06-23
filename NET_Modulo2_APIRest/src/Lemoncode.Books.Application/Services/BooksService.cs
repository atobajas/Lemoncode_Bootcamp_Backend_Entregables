using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Models.Filters;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lemoncode.Books.Application.Services
{
    public class BooksService
    {
        private readonly BooksDbContext _booksDbContext;
        public BooksService(BooksDbContext booksDBContext)
        {
            _booksDbContext = booksDBContext;
        }

        public async Task<BookInfo> GetBook(int id)
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

            var bookDto = MapBookEntityToBookInfo(bookEntity);
            return bookDto;
        }

        public async Task<IEnumerable<BookInfo>> GetBooks(BooksFilter booksFilter)
        {
            var booksQueryable = _booksDbContext
                .Books
                .Include(x => x.Author)
                .AsQueryable();

            if (booksFilter.Title != null)
            {
                booksQueryable = booksQueryable.Where(x =>
                    x.Title.Contains(booksFilter.Title));
            }

            if (booksFilter.Author != null)
            {
                var author = booksFilter.Author.ToLowerInvariant();
                booksQueryable = booksQueryable.Where(x =>
                    x.Author.Name.Contains(author)
                    || x.Author.LastName.Contains(author));
            }

            var bookDtos =
                booksQueryable
                .ToList()
                .Select(MapBookEntityToBookInfo);

            return bookDtos;
        }

        public async Task CreateBook(BookDto newBookDto)
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

        public async Task ModifyBook(UpdateBookDto newBookDto)
        {
            //if (!IsValidAuthor(newBookDto.AuthorId))
            //{
            //    throw new KeyNotFoundException($"No existe el autor {newBookDto.AuthorId}");
            //}

            var newBookEntity = _booksDbContext.Books.SingleOrDefault(x => x.Id == newBookDto.Id);
            if (newBookEntity is null)
            {
                throw new KeyNotFoundException($"No existe el libro {newBookDto.Id}");
            }

            if (!string.IsNullOrWhiteSpace(newBookDto.Title))
            {
                newBookEntity.Title = newBookDto.Title;
            }

            if (!string.IsNullOrWhiteSpace(newBookDto.Description))
            {
                newBookEntity.Description = newBookDto.Description;
            }

            _booksDbContext.SaveChanges();
        }

        public async Task RemoveBook(int id)
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

        private BookInfo MapBookEntityToBookInfo(BookEntity bookDto)
        {
            var bookInfo =
                new BookInfo
                {
                    Id = bookDto.Id,
                    Title = bookDto.Title,
                    Description = bookDto.Description,
                    Author = $"{bookDto.Author.Name} {bookDto.Author.LastName}",
                    PublishedOn = DateTime.TryParse(bookDto.PublishedOn.ToString(), out DateTime temp)
                                    ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                                    : null,
                };

            return bookInfo;
        }

    }
}
