using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Services;
using Microsoft.AspNetCore.Mvc;


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
        /// <param name="id" example="5">The author id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id:int}")]
        public IActionResult GetAuthor(int id, [FromQuery] bool isDetailed = false)
        {
            var author = _authorsService.GetAuthor(id);
            return Ok(author);
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorDto newAuthor)
        {
            _authorsService.CreateAuthor(newAuthor);
            return CreatedAtAction(nameof(GetAuthor), new { newAuthor.Id }, newAuthor);
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateAuthorDto newAuthor)
        {
            _authorsService.ModifyAuthor(id, newAuthor);
            return Ok();
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _authorsService.RemoveAuthor(id);
            return Ok();
        }
    }
}
