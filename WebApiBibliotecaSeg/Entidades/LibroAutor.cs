namespace WebApiBibliotecaSeg.Entidades
{
    // Se utiliza para relacionar la tabla de libros y la de autores
    public class LibroAutor
    {
        public int libroId { get; set; }
        public int autorId { get; set; }

        // Variable de apoyo para ordenar una lista
        public int orden { get; set; }

        public Libros libro { get; set; }
        public Autor autor { get; set; }
    }
}
