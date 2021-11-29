﻿using FluentAssertions;
using Lemoncode.Books.Application.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Lemoncode.Books.FunctionalTests.TestSupport;
using System.Collections.Generic;

namespace Lemoncode.Books.FunctionalTests.UseCases.Book
{
    public static class CreateAndGetBookTests
    {
        public class Given_An_Author_With_Two_Books_When_Getting_Author_And_Two_Books
            : FunctionalTest
        {

            private HttpResponseMessage _result;
            private string _authorId;
            private AuthorDto _newAuthor;
            private IEnumerable<BookDto> _expectedBooks;

            protected override async Task Given()
            {
                _newAuthor =
                    new AuthorDto
                    {
                        Name = "foo",
                        LastName = "bar",
                        Birth = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        CountryCode = "ES"
                    };

                var responseAuthor = await HttpClientAuthorized.PostAsJsonAsync("api/authors", _newAuthor);
                if (!responseAuthor.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author");
                }
                _authorId = responseAuthor.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorId = int.Parse(_authorId);

                var newBookOne =
                    new BookDto
                    {
                        Title = "title one",
                        Description = "description one",
                        AuthorId = authorId,
                        PublishedOn = null
                    };

                var responseBookOne = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookOne);
                if (!responseBookOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book one");
                }
                var bookOneIdAsText = responseBookOne.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookOneId = int.Parse(bookOneIdAsText!);

                var newBookTwo =
                    new BookDto
                    {
                        Title = "title two",
                        Description = "description two",
                        AuthorId = authorId,
                        PublishedOn = null
                    };

                var responseBookTwo = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookTwo);
                if (!responseBookTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book two");
                }
                var bookTwoIdAsText = responseBookTwo.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookTwoId = int.Parse(bookTwoIdAsText!);

                _expectedBooks =
                    new List<BookDto>
                    {
                        new()
                        {
                            Id = bookOneId,
                            Title = newBookOne.Title,
                            Description = newBookOne.Description,
                            PublishedOn = newBookOne.PublishedOn
                        },
                        new()
                        {
                            Id = bookTwoId,
                            Title = newBookTwo.Title,
                            Description = newBookTwo.Description,
                            PublishedOn = newBookTwo.PublishedOn
                        }
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync($"api/books");
            }

            [Fact]
            public void Then_It_Should_Return_Status_Code_Ok()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Fact]
            public async Task Then_It_Should_Return_The_Expected_Author()
            {
                var json = await _result.Content.ReadAsStringAsync();
                var authorInfo = JsonSerializer.Deserialize<AuthorDto>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                authorInfo.Books.Should().BeEquivalentTo(_expectedBooks);
            }
        }
    }
}
