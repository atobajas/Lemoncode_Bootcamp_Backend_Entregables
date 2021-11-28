using System.IO;
using Lemoncode.Books.Application.Models.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Lemoncode.Books.WebApi.Extensions
{
    public static class OpenApiExtensions
    {        
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            var mainAssemblyName = typeof(Startup).Assembly.GetName().Name;
            var applicationAssemblyName = typeof(BooksFilter).Assembly.GetName().Name;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Lemoncode.Books.WebApi",
                    Version = "v1",
                    Description = "Lemoncode Bootcamp - Arquitectura Limpia",
                    Contact = new OpenApiContact
                    {
                        Name = "Antonio Tobajas",
                        Email = "atobajas@gmail.com"
                    }
                });

                var xmlCommentsWebApi = Path.Combine(System.AppContext.BaseDirectory, $"{mainAssemblyName}.xml");
                c.IncludeXmlComments(xmlCommentsWebApi);
                var xmlCommentsApplication = Path.Combine(System.AppContext.BaseDirectory, $"{applicationAssemblyName}.xml");
                c.IncludeXmlComments(xmlCommentsApplication);

                c.AddSecurityDefinition(
                    "basic",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "basic",
                        In = ParameterLocation.Header,
                        Description = "Basic Authorization header"
                    });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[]{}
                        }
                    });
            });

            return services;
        }

        public static void UseOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lemoncode Books v1"));
        }
    }
}
