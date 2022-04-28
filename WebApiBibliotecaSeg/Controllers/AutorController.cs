using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBibliotecaSeg.DTOs;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiBibliotecaSeg.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutorController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public AutorController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> GetAll()
        {
            return await dbContext.autores.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerautor")] // se asigna un nombre a la ruta
        public async Task<ActionResult<AutorDTOConLibros>> GetById(int id)
        {

            // Include especifica el atributo el cual se quiere obtener
            // TheInclude especifica aun mas la data a obtener
         
            // la variable autor obtiene los datos: Autor.libroAutor.libro y Autor.permisos.id
            var autor = await dbContext.autores
                // indica que se quiere acceder a los datos del atributo libroAutor
                .Include(libroDb => libroDb.libroAutor)
                // indica que se quiere acceder a los datos del atributo libro
                .ThenInclude(LibroAutor => LibroAutor.libro)
                // indica que se quiere acceder a los datos del atributo permisos
                .Include(permisosDb => permisosDb.permisos)
                .FirstOrDefaultAsync(x => x.id == id);

            // Se ordena la lista con apoyo del atributo orden de libroAutor
            autor.libroAutor = autor.libroAutor.OrderBy(x => x.orden).ToList();

            // Se mapea la variable autor para que sea tipo AutorDTOConLibros y la retorna
            return mapper.Map<AutorDTOConLibros>(autor);

        }

        [HttpPost] 
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
        {
            // Se verifica que se ingrese algo en la lista librosIds
           if(autorCreacionDTO.librosIds == null)
            { return BadRequest("No se puede crear el autor sin libros"); }
           
            var librosIds = await dbContext.libros
                // Verifica que los ids de librosIds existan en la DB
                 .Where(libroDb => autorCreacionDTO.librosIds
                 .Contains(libroDb.id))
                 // Se indica que se quiere obtener solamente el atributo id del libro
                 .Select(x => x.id).ToListAsync();

            // Si la longitud de ambas listas es diferente significa que de alguno o 
            // algunos ids no se encontro registros en la DB
            if(autorCreacionDTO.librosIds.Count != librosIds.Count)
            { return BadRequest("No existe uno de los libros enviados"); }

            // Mapea la variable autorCreacionDTO para ser tipo Autor
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            if(autor.libroAutor != null)
            {
                // Se setean los valores de el orden dependiendo de la cantidad de elementos
                for (int i=0; i<autor.libroAutor.Count; i++)
                {
                    autor.libroAutor[i].orden = i;
                }
            }
            dbContext.Add(autor);
            await dbContext.SaveChangesAsync();
            
            // se mapea la variable autor para que sea tipo AutorDTO
            var autorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("obtenerautor", //se manda el nombre de la ruta definido en el get
                new { id = autor.id}, // se manda el id del autor
                autorDTO); // se manda el objeto que contiene la informacion que se desea mostrar

            /* la razon por la cual se manda una variable tipo AutorDTO en lugar de la tipo Autor
             es por la informacion que esta va a mostrar en la api
             al mandar la tipo Autor se muestra Id, nombre, permisos y libroAutor, la cual no toda se quiere mostrar
             al manda la tipo AutorDTO solamente se va a mostrar el Id, nombre y permisos.*/

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {

            var exist = await dbContext.autores.AnyAsync(x => x.id == id);

            if (!exist)
            {
                return NotFound("La clase especifica no existe");
            }

            if (autor.id != id)
            {
                return BadRequest("El id de la clase no coincide con el establecido con el url");
            }

            dbContext.Update(autor);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.autores.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound("el recurdos no fue encontrado");
            }

            dbContext.Remove(new Autor { id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
