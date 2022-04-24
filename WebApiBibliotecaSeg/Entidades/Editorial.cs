namespace WebApiBibliotecaSeg.Entidades
{
    public class Editorial
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public int autorId { get; set; }
        public Autor autor { get; set; }
    }
}
