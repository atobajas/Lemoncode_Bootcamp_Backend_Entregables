using FluentAssertions;
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

namespace Lemoncode.Books.FunctionalTests.UseCases
{
    public static class CreateAndGetAuthorTests
    {
        public class Given_An_Author_When_Getting_Author
            : FunctionalTest
        {
            private AuthorDto _newAuthor;
            private HttpResponseMessage _result;
            private string _authorId;
            private AuthorDto _expectedAuthorInfo;

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

                var response = await HttpClientAuthorized.PostAsJsonAsync("api/authors", _newAuthor);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author");
                }
                _authorId = response.Headers.Location?.AbsolutePath.Split("/").Last();

                _expectedAuthorInfo =
                    new AuthorDto
                    {
                        Id = int.Parse(_authorId!),
                        Name = _newAuthor.Name,
                        LastName = _newAuthor.LastName,
                        Birth = _newAuthor.Birth,
                        CountryCode = _newAuthor.CountryCode,
                        Books = Enumerable.Empty<BookDto>()
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync($"api/authors/{_authorId}");
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
                authorInfo.Should().BeEquivalentTo(_expectedAuthorInfo);
            }
        }
    }
}
