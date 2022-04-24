using Microsoft.EntityFrameworkCore;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiBibliotecaSeg
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LibroAutor>()
                .HasKey(libro => new { libro.libroId, libro.autorId });
        }

        public DbSet<Libros> libros { get; set; }

        public DbSet<Autor> autores { get; set; }

        public DbSet<Editorial> editorial { get; set; }

        public DbSet<LibroAutor> libroAutor { get; set; }
    }
}
