using Lemoncode.Books.Application;
using Lemoncode.Books.Application.Services;
using Lemoncode.Books.Infra.Repository.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lemoncode.Books.WebApi.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddBooksDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddTransient<AuthorsService>();
            services.AddTransient<BooksService>();

            // Registro del repositorio a utilizar
            //services.AddSingleton<IAuthorsRepository, InMemoryAuthorsRepository>();
            services.AddTransient<IAuthorsRepository, EfCoreAuthorsRepository>();
            services.AddTransient<IBooksRepository, EfCoreBooksRepository>();

            return services;
        }
    }
}
