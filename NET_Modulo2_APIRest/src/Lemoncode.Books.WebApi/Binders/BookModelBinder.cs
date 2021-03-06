using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lemoncode.Books.WebApi.Binders
{
    public class CustomModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            string valueFromBody = string.Empty;

            using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                valueFromBody = sr.ReadToEndAsync().ToString();
            }

            if (string.IsNullOrEmpty(valueFromBody))
            {
                return Task.CompletedTask;
            }

            string values = Convert.ToString(((JValue)JObject.Parse(valueFromBody)["value"]).Value);

            var splitData = values.Split(new char[] { '|' });
            if (splitData.Length >= 2)
            {
                var newBook = new BookDto()
                {
                    Title = splitData[0],
                    Description = splitData[1],
                    PublishedOn = DateTime.TryParse(splitData[2].ToString(), out DateTime temp)
                        ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                        : null,
                    AuthorId = int.Parse(splitData[3])
                };

                Guid.TryParse(splitData[3], out var authorId);
                bindingContext.Result = ModelBindingResult.Success(newBook);
            }

            return Task.CompletedTask;
        }
    }

    public class BookModelBinder : IModelBinder
    {
        private readonly AuthorsService _authorsService;

        public BookModelBinder(AuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

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
                bindingContext.ModelState.TryAddModelError(
                    modelName, "Author doesn´t be null or empty.");
                return Task.CompletedTask;
            }

            if (!Guid.TryParse(value, out var authorId))
            {
                // Non-guid arguments result in model state errors
                bindingContext.ModelState.TryAddModelError(
                    modelName, "Author Id must be an Guid.");
                return Task.CompletedTask;
            }
            
            var book = new BookDto()
            {
                Title = bindingContext.ValueProvider.GetValue(nameof(BookDto.Title)).FirstValue,
                Description = bindingContext.ValueProvider.GetValue(nameof(BookDto.Description)).FirstValue,
                PublishedOn = DateTime.TryParse(bindingContext.ValueProvider.GetValue(nameof(BookDto.PublishedOn)).FirstValue, 
                                                out DateTime temp)
                                                ? new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0)
                                                : null,
                AuthorId = int.Parse(nameof(BookDto.AuthorId))
            };
            bindingContext.Result = ModelBindingResult.Success(book);
            return Task.CompletedTask;
        }
    }
}
