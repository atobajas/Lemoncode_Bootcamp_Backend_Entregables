using System;

namespace Lemoncode.Books.Application.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
