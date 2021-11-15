using Lemoncode.Books.Application;
using Lemoncode.Books.Infra.Repository.EfCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lemoncode.Books.WebApi.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSoccerDependencies(this IServiceCollection services)
        {
            //services.AddSingleton<IDateTimeService, DateTimeService>();
            //services.AddTransient<GamesCommandService>();
            //services.AddTransient<GamesQueryService>();
            //services.AddSingleton<GameToGameReportMapper>();

            // Registro del repositorio a utilizar
            //services.AddSingleton<IGamesRepository, InMemoryGamesRepository>();
            services.AddTransient<IAuthorsRepository, EfCoreAuthorsRepository>();

            return services;
        }
    }
}
