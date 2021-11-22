using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lemoncode.Books.Application
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        // Aquí los DbSet<YourEntity> que representan tablas
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<BookEntity> Books { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder
        //        //.UseLazyLoadingProxies()
        //        .UseSqlServer(ConnectionString);

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    // Aquí la configuración adicional para relaciones entre entidades (solamente si hiciera falta)
        //    //modelBuilder
        //    //.Entity<GameEntity>()
        //    //.HasOne<TeamEntity>(x => x.HomeTeam)
        //    //.WithMany()
        //    //.OnDelete(DeleteBehavior.Restrict);
        //}
    }
}
