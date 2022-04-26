namespace WebApiBibliotecaSeg.Entidades
{
    public class Permisos
    {
        public int Id { get; set; }
        public string tipo { get; set; }
        public int autorId { get; set; }

        // Un permiso solo puede pertenecer a un autor
        public Autor autor { get; set; }
    }
}
