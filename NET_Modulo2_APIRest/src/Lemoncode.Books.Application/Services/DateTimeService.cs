using Lemoncode.Books.Application.Services.Abstractions;
using System;

namespace Lemoncode.Books.Application.Services
{
    public class DateTimeService : IDateTimeFactory
    {
        public DateTime? GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
