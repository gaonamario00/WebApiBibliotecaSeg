using System.ComponentModel.DataAnnotations;
using WebApiBibliotecaSeg.Validaciones;

namespace WebApiBibliotecaSeg.DTOs
{
    // Se utiliza al momento de crear un nuevo registro de autores
    public class AutorCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }
        // se agrega campo fechaDeRegistro
        public DateTime fechaDeRegistro { get; set; }
        public List<int> librosIds { get; set; }
    }
}
