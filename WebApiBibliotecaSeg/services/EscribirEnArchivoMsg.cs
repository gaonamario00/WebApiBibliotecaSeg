namespace WebApiBibliotecaSeg.services
{
    public class EscribirEnArchivoMsg
    {

        private readonly string nombreArchivo;
        private readonly string data;

        // Mediante el constructor ejecuta el metodo DoWork() que eventualmente escribe en el archivo.
        public EscribirEnArchivoMsg(string nombreArchivo, string data)
        {
            this.nombreArchivo = nombreArchivo;
            this.data = data;
            DoWork();
        }

        // Manda a llamar le metodo Escribir()
        public void DoWork()
        {
            Escribir(data+" " + DateTime.Now.ToString("G"));
        }

        // Realiza la escritura en el archivo especificado y lo guarda en la carpeta "ArchivosTxt"
        public void Escribir(string msg)
        {
            string ruta = @"ArchivosTxt/"+nombreArchivo;
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(msg);
                writer.Close();
            }
        }

    }
}
