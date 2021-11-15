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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsRepository _authorsRepository;

        // GET: api/<AuthoresController>
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authorsRepository = _authorsRepository.GetAuthors();
            return Ok(authorsRepository);
        }

        // GET api/<AuthoresController>/5
        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The game id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var authorRepository = _authorsRepository.GetAuthor(id);
            return Ok(authorRepository);
        }

        // POST api/<AuthoresController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthoresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
