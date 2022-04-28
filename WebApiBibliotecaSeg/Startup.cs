using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApiBibliotecaSeg.Filtros;
using WebApiBibliotecaSeg.services;

namespace WebApiBibliotecaSeg
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Puede ser en cualquier clase donde se desee consultar el controller
            // Mala practica
            //Principio solid, nuestras clases deberian de depender de abstracciones y no de tipos concretos

            // var LibrosController = new LibrosController(new ApplicacionDbContext(null))
            //    new services()
            // );


            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeException));
            }).AddJsonOptions(x =>
            // AddNewtonsoftJson(): Configura funciones específicas como formateadores de entrada y salida
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );

            ///////////
            services.AddTransient<IService, ServiceA>();
            services.AddScoped<ServiceScoped>();
            services.AddSingleton<ServiceSingleton>();
            services.AddTransient<ServiceTransient>();
            ///////////
            services.AddTransient<FiltroDeAccion>();
            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // os permite que se ejecute la clase durante la ejecución del programa.
            //services.AddHostedService<EscribirEnArchivoProfe>();

            // Ejemplo en clase
            //services.AddHostedService<EscribirEnArchivo>();

            //////////////////////////
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiBibliotecaSeg", Version = "v2" });
            });
             services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
             ILogger<Startup> logger
            )
        {
            //app.Use(async (context, siguiente) =>
            //{
            //using (var ms = new MemoryStream())
            //{
            //    //se asinga el body del responde en una variable y se le da el valor de memorystream
            //    var bodyoriginal = context.Response.Body;
            //    context.Response.Body = ms;

            //    //permite continuar con la linea
            //    await siguiente(context);

            //    //guardamos lo que le respondemos al cliente en el string
            //    ms.Seek(0, SeekOrigin.Begin);
            //    string response = new StreamReader(ms).ReadToEnd();
            //    ms.Seek(0, SeekOrigin.Begin);

            //    //leemos el stream y lo colocamos como estaba
            //    await ms.CopyToAsync(bodyoriginal);
            //    context.Response.Body = bodyoriginal;

            //    logger.LogInformation(response);

            //}
            //});
            ////////////////////
            //* Marca Error
            //app.UseMiddleware<ResponseHttpMiddleware>();
            //app.UseResponseHttpMiddleware();
            //*
            ////////////////////
            //Se ejecuta la pagina web solamente mostrando el mensaje
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Interceptar peticiones");
            //}
            //);

            //De esta manera el mensaje solo se muestra en la ruta especificada
            //app.Map("/ruta1", app =>
            //{
            //    app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Interceptar peticiones");
            //    });
            //});

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }



    }
}
