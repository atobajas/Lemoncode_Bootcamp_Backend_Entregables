using Lemoncode.Books.Application;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lemoncode.Books.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        // GET: api/<BooksController>
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var books = _booksRepository.GetBooks();
            return Ok(books);
        }

        // GET api/<BooksController>/5
        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The book id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var book = _booksRepository.GetBook(id);
            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
