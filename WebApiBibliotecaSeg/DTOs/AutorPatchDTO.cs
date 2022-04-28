using System.ComponentModel.DataAnnotations;
using WebApiBibliotecaSeg.Validaciones;

namespace WebApiBibliotecaSeg.DTOs
{
    // Clase para usar en el endpoint HttpPatch
    public class AutorPatchDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }
        public DateTime fechaDeRegistro { get; set; }
    }
}
