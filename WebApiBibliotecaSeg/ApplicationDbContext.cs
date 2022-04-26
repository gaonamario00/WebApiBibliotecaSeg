using Microsoft.EntityFrameworkCore;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiBibliotecaSeg
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        // Construye un modelo basado en la forma de las entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setea a libroId y autorId como primary key de la tabla libroAutor
            modelBuilder.Entity<LibroAutor>()
                .HasKey(libro => new { libro.libroId, libro.autorId });
        }

        public DbSet<Libros> libros { get; set; }

        public DbSet<Autor> autores { get; set; }

        public DbSet<Permisos> permisos { get; set; }

        public DbSet<LibroAutor> libroAutor { get; set; }
    }
}
