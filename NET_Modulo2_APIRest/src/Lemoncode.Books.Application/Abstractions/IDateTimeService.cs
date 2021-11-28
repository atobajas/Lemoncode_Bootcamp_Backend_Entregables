using System;

namespace Lemoncode.Books.Application.Services.Abstractions
{
    public interface IDateTimeService
    {
        DateTime GetUtcNow();
    }
}
