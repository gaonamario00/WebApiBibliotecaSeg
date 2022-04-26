namespace WebApiBibliotecaSeg.DTOs
{
    // Relaciona como un libro puede tener varios autores (ManyToMany)
    public class LibrosDTOConAutor: GetLibroDTO
    {
        public List<AutorDTO> autores { get; set; }
    }
}
