﻿using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Models.Filters;
using Lemoncode.Books.Application.Services;
using Lemoncode.Books.WebApi.Binders;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="title" example="api/books? title = foo & author = bar">Filters</param>
        /// <param name="author" example="api/books? title = foo & author = bar">Filters</param>
        [HttpGet]
        public IActionResult GetBooks([FromQuery] BooksFilter booksFilter)
        {
            var books = _booksService.GetBooks(booksFilter);
            return Ok(books);
        }

        // GET api/<BooksController>/5
        /// <param name="id" example="5">The book id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id:int}")]
        public IActionResult GetBook(int id, [FromQuery] bool isDetailed = false)
        {
            var book = _booksService.GetBook(id);
            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        public IActionResult CreateBook([FromBody] BookDto newBook)
        {
            _booksService.CreateBook(newBook);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }
        // Usando ModelBinder específico
        //public IActionResult Post([ModelBinder(typeof(BookModelBinder), Name = "authorid"), FromBody] BookDto newBook)
        //{
        //    var id = _booksService.CreateBook(newBook);
        //    return CreatedAtAction(nameof(GetBook), new { id }, newBook);
        //}

        // PUT api/<BooksController>/5
        [HttpPut("{id:int}")]
        public IActionResult ModifyBook(int id, [FromBody] UpdateBookDto newBook)
        {
            if (id != newBook.Id)
            {
                return BadRequest("Id no coincide");
            }
            _booksService.ModifyBook(newBook);
            return Ok();
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public IActionResult RemoveBook(int id)
        {
            _booksService.RemoveBook(id);
            return Ok();
        }
    }
}
