using System.ComponentModel.DataAnnotations;
using WebApiBibliotecaSeg.Validaciones;

namespace WebApiBibliotecaSeg.DTOs
{
    // Se utiliza para transportar datos de la entidad Libros
    public class LibroDTO
    {
        [Required(ErrorMessage = "El campo de {0} es obligatorio")]
        [StringLength(maximumLength: 20, ErrorMessage = "El {0} solo puede tener 20 caracteres como maximo")]
        [PrimeraLetraMayuscula]
        public string titulo { get; set; }
    }
}
