namespace WebApiBibliotecaSeg.DTOs
{
    // Relaciona como un autor puede tener varios libros (ManyToMany)
    public class AutorDTOConLibros: AutorDTO
    {
        public List<GetLibroDTO> Libros { get; set; }
    }
}
