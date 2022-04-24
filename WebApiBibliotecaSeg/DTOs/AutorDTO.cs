namespace WebApiBibliotecaSeg.DTOs
{
    public class AutorDTO
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public List<EditorialDTO> editoriales { get; set; }
    }
}
