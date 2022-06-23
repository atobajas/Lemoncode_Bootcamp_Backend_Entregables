using System;

namespace Lemoncode.Books.Application.Services.Abstractions
{
    public interface IDateTimeFactory
    {
        DateTime? GetUtcNow();
    }
}
 