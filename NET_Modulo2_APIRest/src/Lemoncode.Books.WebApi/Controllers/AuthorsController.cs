using Lemoncode.Books.Application;
using Lemoncode.Books.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
        //[HttpPost]
        //public IActionResult CreateAuthor([FromBody] string value)
        //{
            //TODO terminar esto
            //var id = _authorsService.CreateAuthor(value);
            //return CreatedAtAction(nameof(GetGameReport), new { id = id }, newGame);
        //}

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
