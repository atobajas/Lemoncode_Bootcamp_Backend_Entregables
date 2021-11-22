using Lemoncode.Books.Application.Services;
using Lemoncode.Books.Domain;
using Lemoncode.Books.WebApi.Binders;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Lemoncode.Books.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        // GET: api/<BooksController>
        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _booksService.GetBooks();
            return Ok(books);
        }

        // GET api/<BooksController>/5
        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The book id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id}")]
        public IActionResult GetBook(Guid id, [FromQuery] bool isDetailed = false)
        {
            var book = _booksService.GetBook(id);
            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        //public IActionResult Post([FromBody] Book newBook)
        //{
        //    var id = _booksService.CreateBook(newBook);
        //    return CreatedAtAction(nameof(GetBook), new { id }, newBook);
        //}
        public IActionResult Post([ModelBinder(typeof(BookModelBinder), Name = "authorid"), FromBody] Book newBook)
        {
            var id = _booksService.CreateBook(newBook);
            return CreatedAtAction(nameof(GetBook), new { id }, newBook);
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Book newBook)
        {
            _booksService.ModifyBook(id, newBook);
            return Ok();
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _booksService.RemoveBook(id);
            return Ok();
        }
    }
}
