using Lemoncode.Books.Application.Services;
using Lemoncode.Books.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

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
        public IActionResult Post([ModelBinder(typeof(BookBinder), Name = "Book"), FromBody] Book newBook)
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

    public class BookBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelMetadata.ModelType.Name;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            if (bindingContext.ModelMetadata.ModelType == typeof(Book))
            {
                valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
                var str = valueProviderResult.Values;
                return Task.CompletedTask;
            }

            if (!int.TryParse(value, out var id))
            {
                // Non-integer arguments result in model state errors
                bindingContext.ModelState.TryAddModelError(
                    modelName, "Author Id must be an integer.");

                return Task.CompletedTask;
            }

            // Model will be null if not found, including for
            // out of range id values (0, -3, etc.)
            //var model = _context.Authors.Find(id);
            //bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
