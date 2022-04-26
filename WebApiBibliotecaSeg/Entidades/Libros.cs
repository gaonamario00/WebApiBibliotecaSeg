using System.ComponentModel.DataAnnotations;
using WebApiBibliotecaSeg.Validaciones;

namespace WebApiBibliotecaSeg.Entidades
{
    public class Libros
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El campo de {0} es obligatorio")]
        [StringLength(maximumLength: 20, ErrorMessage = "El {0} solo puede tener 20 caracteres como maximo")]
        [PrimeraLetraMayuscula]
        public string titulo { get; set; }

        // Libro y autor tienen una relacion ManyToMany
        public List<LibroAutor> libroAutor { get; set; }
    }
}
