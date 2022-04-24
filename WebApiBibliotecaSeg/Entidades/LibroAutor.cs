namespace WebApiBibliotecaSeg.Entidades
{
    public class LibroAutor
    {
        public int libroId { get; set; }
        public int autorId { get; set; }
        public int orden { get; set; }
        public Libros libro { get; set; }
        public Autor autor { get; set; }
    }
}
