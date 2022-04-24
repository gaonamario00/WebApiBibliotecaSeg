namespace WebApiBibliotecaSeg.services
{
    public class EscribirEnArchivoProfe : IHostedService
    {
        // El atributo env proporciona información sobre el entorno
        // web en el que se ejecuta la applicacion.
        private readonly IWebHostEnvironment env;
        
        private readonly string nombreArchivo = "MsjProfe.txt";
        private Timer timer;

        public EscribirEnArchivoProfe(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            Escribir("El Profe Gustavo Rodriguez es el mejor. Fecha: " + DateTime.Now.ToString("G"));
        }

        public void Escribir(string msg)
        {
            string ruta = @"ArchivosTxt/" + nombreArchivo;
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(msg);
                writer.Close();
            }
        }
    }
}
