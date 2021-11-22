using Lemoncode.Books.Application.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace Lemoncode.Books.WebApi.Binders
{
    public class BookModelBinderProvider : IModelBinderProvider
    {        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(BookDto))
            {
                return new BinderTypeModelBinder(typeof(BookModelBinder));
            }

            return null;
        }

    }
}
