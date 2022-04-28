using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBibliotecaSeg.DTOs;
using WebApiBibliotecaSeg.Entidades;
using WebApiBibliotecaSeg.services;

namespace WebApiBibliotecaSeg.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetLibroDTO>>> Get()
        {
            var libros = await dbContext.libros.ToListAsync();
            return mapper.Map<List<GetLibroDTO>>(libros);
        }


        [HttpGet("{id:int}", Name = "obtenerlibro")] // se asigna un nombre a la ruta
        public async Task<ActionResult<LibrosDTOConAutor>> Get(int id)
        {

            // Include especifica el atributo el cual se quiere obtener
            // TheInclude especifica aun mas la data a obtener

            // la variable libro obtiene los datos: Libros.libroAutor.autor
            var libro = await dbContext.libros
                // indica que se quiere acceder a los datos del atributo libroAutor
                .Include(dbLibro => dbLibro.libroAutor)
                // indica que se quiere acceder a los datos del atributo autor
                .ThenInclude(dbLibroAutor => dbLibroAutor.autor)
                .FirstOrDefaultAsync(libroDb => libroDb.id == id);

            if (libro == null)
            {
                return NotFound();
            }

            // mapea la variable libro a tipo LibrosDTOConAutor
            // regresa un tipo LibrosDTOConAutor
            return mapper.Map<LibrosDTOConAutor>(libro);

        }

        [HttpGet("{titulo}")]
        public async Task<ActionResult<List<GetLibroDTO>>> Get([FromRoute] string titulo)
        {
            var libros = await dbContext.libros.Where(x => x.titulo.Contains(titulo)).ToListAsync();

            if(libros.Count == 0)
            {
                new EscribirEnArchivoMsg("registroConsultado.txt", "Titulo ingresado: " + titulo + ", sin resultados");
            }
            else
            {
                new EscribirEnArchivoMsg("registroConsultado.txt", "Titulo ingresado: " + titulo + ", con "+libros.Count+" resultados, obtenido el ");

                foreach (var libro in libros)
                {
                    new EscribirEnArchivoMsg("registroConsultado.txt", "Resultado: " + libro.titulo + ", obtenido el ");
                }
            }

            return mapper.Map<List<GetLibroDTO>>(libros);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LibroDTO libroDTO)
        {
            //Ejemplo para validar desde el controlador con la BD con ayuda del dbContext
            var existeLibroMismoTitulo = await dbContext.libros.
                AnyAsync(x => x.titulo == libroDTO.titulo);

            if (existeLibroMismoTitulo)
            {
                return BadRequest($"Ya existe un autor con el nombre {libroDTO.titulo}");
            }

            // mapea la variable libro a tipo libroDTO
            var libro = mapper.Map<Libros>(libroDTO);

            // agrega el libro a la BD
            dbContext.Add(libro);
            await dbContext.SaveChangesAsync();

            new EscribirEnArchivoMsg("nuevosRegistros.txt","Titulo: "+libroDTO.titulo + ", agregado el ");

            // se mapea la variable libro para que sea tipo GetLibroDTO
            var libroDTO_ = mapper.Map<GetLibroDTO>(libro);

            return CreatedAtRoute("obtenerlibro", //se manda el nombre de la ruta definido en el get
                new {id = libro.id}, // se manda el id de libro
                libroDTO_); // se manda el objeto que contiene la informacion que se desea mostrar

            /* la razon por la cual se manda una variable tipo GetLibroDTO en lugar de la tipo Libros
             es por la informacion que esta va a mostrar en la api
             al mandar la tipo Libros se muestra Id, titulo y libroAutor, la cual no toda se quiere mostrar
             al manda la tipo GetLibroDTO solamente se va a mostrar el Id y el titulo.*/
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Libros libro, int id)
        {
            var exist = await dbContext.libros.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound();
            }

            if (libro.id != id)
            {
                return BadRequest("El id del libro no coincide con el establecido en la url.");
            }

            dbContext.Update(libro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.libros.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Libros()
            {
                id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
