using System.ComponentModel.DataAnnotations;
using WebApiBibliotecaSeg.Validaciones;

namespace WebApiBibliotecaSeg.Entidades
{
    public class Autor
    {

        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string nombre { get; set; }

        public List<Editorial> editoriales { get; set; }

        public List<LibroAutor> libroAutor { get; set; }

    }
}
