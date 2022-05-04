using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IConfiguration configuration;

        public AutorController(ApplicationDbContext context, IMapper mapper,
            IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("provedoresDeConfiguracion")]
        public ActionResult<string> GetConfiguracion()
        {
            return configuration["proveedorTerminal"];
        }

        [HttpGet]
        [AllowAnonymous]
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

            if (autor == null) { return NotFound(); }

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

            // Setea el orden de libroAutor y al mismo tiempo
            // actualiza los campos de autor
            OrdenarPorAlumnos(autor);

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

        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(Autor autor, int id)
        //{

        //    var exist = await dbContext.autores.AnyAsync(x => x.id == id);

        //    if (!exist)
        //    {
        //        return NotFound("La clase especifica no existe");
        //    }

        //    if (autor.id != id)
        //    {
        //        return BadRequest("El id de la clase no coincide con el establecido con el url");
        //    }

        //    dbContext.Update(autor);
        //    await dbContext.SaveChangesAsync();

        //    return Ok();
        //}

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AutorCreacionDTO autorCreacionDTO)
        {
            // La variable autorDB contiene solo La info de Autor.libroAutor
            var autorDB = await dbContext.autores
                // Indica que solo se quiere acceder a la variable libroAutor de autores
                .Include(x => x.libroAutor)
                // Indica que trae solo el primer registro que encuentre
                .FirstOrDefaultAsync(x => x.id == id);

            if (autorDB == null) { return NotFound(); }

            // mapea la variable autorCreacionDTO a tipo autorDB, o sea, a tipo Autor
            autorDB = mapper.Map(autorCreacionDTO, autorDB);

            // Setea el orden de libroAutor y al mismo tiempo
            // actualiza los campos de autor
            OrdenarPorAlumnos(autorDB);

            // guarda los cambios en la base de datos
            await dbContext.SaveChangesAsync();

            //204: «Sin contenido». Este código significa que el servidor ha procesado
            //con éxito la solicitud, pero no va a devolver ningún contenido.
            return NoContent();
        }

        // Setea el orden de libroAutor y al mismo tiempo
        // actualiza los campos de autor
        private void OrdenarPorAlumnos(Autor autor)
        {
            if (autor.libroAutor != null)
            {
                for (int i = 0; i < autor.libroAutor.Count; i++)
                {
                    autor.libroAutor[i].orden = i;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Verifica si existe el registro
            var exist = await dbContext.autores.AnyAsync(x=>x.id==id);

            // Si no existe retorna 404
            if(!exist) { return NotFound(); }

            // Elimina el registro del id recibido
            dbContext.Remove(new Autor { id = id});

            // Guarda cambios en la DB
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id,
            // permite modificar un registro sin afectar a otro
            // en este caso, modifica el autor sin afectar los libros
            JsonPatchDocument<AutorPatchDTO> patchDocument)
        {
            // si es nulo regresa BadRequest()
          if(patchDocument == null) { return BadRequest(); }

            // obtiene el primer registro con el id recibido
            var autorDB = await dbContext.autores.FirstOrDefaultAsync(x => x.id == id);

            // Si no existe el registro se regrese 404
            if (autorDB == null) { return NotFound(); }

            // se mapea la variable autorDB a AutorPatchDTO
            var autorDTO = mapper.Map<AutorPatchDTO>(autorDB);

            // Actualiza o parchea el registro
            patchDocument.ApplyTo(autorDTO);

            // Valida la instancia del modelo especificado.
            var isValid = TryValidateModel(autorDTO);

            // Si no fue valida regresa BadRequest()
            if (!isValid) { return BadRequest(); }

            // mapea la variable autorDTO a tipo Autor
            mapper.Map(autorDTO, autorDB);

            // Guarda los cambios en la BD
            await dbContext.SaveChangesAsync();

            //204: «Sin contenido». Este código significa que el servidor ha procesado con éxito la solicitud,
            // pero no va a devolver ningún contenido.
            return NoContent();
        }

    }
}
