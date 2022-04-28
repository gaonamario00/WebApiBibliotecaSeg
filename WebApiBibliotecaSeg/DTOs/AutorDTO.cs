namespace WebApiBibliotecaSeg.DTOs
{
    // Se utiliza para transportar datos de la entidad autor
    public class AutorDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public List<PermisosDTO> permisos { get; set; }
    }
}
