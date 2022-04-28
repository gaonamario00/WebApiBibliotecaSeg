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
        // se agrega campo fechaDeRegistro
        public DateTime? fechaDeRegistro { get; set; }
        
        // Un autor puede tener muchos permisos (OneToMany)
        public List<Permisos> permisos { get; set; }

        // Libro y autor tienen una relacion ManyToMany
        public List<LibroAutor> libroAutor { get; set; }

    }
}
