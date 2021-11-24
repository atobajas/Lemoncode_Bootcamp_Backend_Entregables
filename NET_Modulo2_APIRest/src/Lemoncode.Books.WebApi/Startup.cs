using Lemoncode.Books.Application;
using Lemoncode.Books.WebApi.Extensions;
using Lemoncode.Books.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lemoncode.Books.WebApi
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var booksConnectionString = _configuration.GetValue<string>("ConnectionStrings:BooksDatabase");
            services.AddDbContext<BooksDbContext>(options => 
                    options.UseSqlServer(booksConnectionString));

            // aquí los registros de servicios en el contenedor de inyección de dependencia. Por ejemplo para Entity Framework
            services.AddControllers();

            services
                .AddOpenApi()
                .AddBooksDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline (middlewares).
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseMiddleware<BasicAuthMiddleware>();
            app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
