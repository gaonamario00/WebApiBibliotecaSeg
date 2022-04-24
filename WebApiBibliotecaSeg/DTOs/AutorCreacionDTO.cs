using System.ComponentModel.DataAnnotations;
using WebApiBibliotecaSeg.Validaciones;

namespace WebApiBibliotecaSeg.DTOs
{
    public class AutorCreacionDTO
    {
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }
        public List<int> librosIds { get; set; }
    }
}
