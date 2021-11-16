using Lemoncode.Books.Application.Services;
using Lemoncode.Books.Domain;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Lemoncode.Books.WebApi.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorsService _authorsService;

        public AuthorsController(AuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        // GET: api/<AuthorsController>
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _authorsService.GetAuthors();
            return Ok(authors);
        }

        // GET api/<AuthorsController>/5
        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The author id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var author = _authorsService.GetAuthor(id);
            return Ok(author);
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author value)
        {
            var id = _authorsService.CreateAuthor(value);
            return CreatedAtAction(nameof(GetAuthor), new { id }, value);
        }

        // PATCH api/<AuthorsController>/5
        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromBody] Author author)
        {
            _authorsService.ModifyAuthor(id, author);
            return Ok();
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _authorsService.RemoveAuthor(id);
            return Ok();
        }
    }
}
