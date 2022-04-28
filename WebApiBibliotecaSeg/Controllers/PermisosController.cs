using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBibliotecaSeg.DTOs;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiBibliotecaSeg.Controllers
{
    [ApiController]
    // La ruta nos indica que este es un controlador dependiende de autor
    // Tambien nos indica que un autor puede tener varios permisos y que 
    // un permiso solo puede pertenecer a un autor
    [Route("autor/{autorId:int}/permisos")]
    public class PermisosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public PermisosController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermisosDTO>>> Get(int autorId)
        {
            // verifica que exista el registro en la base de datos
            var existeAutor = await dbContext.autores.
                AnyAsync(AutorDb => AutorDb.id == autorId);

            if (!existeAutor)
            { return NotFound(); }

            // filtra los registros de permisos por autor
            var permisos = await dbContext.permisos.
                Where(permisosDB => permisosDB.autorId == autorId).ToListAsync();

            // mapea la variable permisos a permisosDTO y la retorna
            return mapper.Map<List<PermisosDTO>>(permisos);
        }

        // se agrega el endpoint para poder ver solo un permiso por id
        [HttpGet("{id:int}", Name ="obtenerpermiso")] // se asigna un nombre a la ruta
        public async Task<ActionResult<PermisosDTO>> GetById(int id)
        {
            // busca un registro en la base de datos por id
            var permiso = await dbContext.permisos.FirstOrDefaultAsync(permisoDb => permisoDb.Id == id);

            // si el registro no exisre regresa un NotFound
            if(permiso == null) { return NotFound(); }

            // se mapea la variable permiso para que sea tipo PermisosDTO y se retorna
            return mapper.Map<PermisosDTO>(permiso);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(int autorId, PermisosCreacionDTO permisosCreacionDTO)
        {
            // verifica que exista el registro en la base de datos
            var existeAutor = await dbContext.autores.
                AnyAsync(AutorDb => AutorDb.id == autorId);
            if (!existeAutor)
            { return NotFound(); }

            // mapea la variable permisosCreacionDTO para que sea tipo Permisos
            // y la almacena a permisos
            var permisos = mapper.Map<Permisos>(permisosCreacionDTO);
            
            // hace que el autorId recibido como parametro
            // lo guarda a el atributo permisos
            permisos.autorId = autorId;
            dbContext.Add(permisos);

            await dbContext.SaveChangesAsync();
            
            // Se mapea la variable permisos para que sea tipo
            var permisoDTO = mapper.Map<PermisosDTO>(permisos);

            return CreatedAtRoute(
                "obtenerpermiso", //se manda el nombre de la ruta definido en el get
                new {id = permisos.Id, autorId = autorId}, // se manda el id de permiso y el id de autor
                                                           // ya que este controller depende de autorController
                permisoDTO); // se manda el objeto que contiene la informacion que se desea mostrar

            /* la razon por la cual se manda una variable tipo PermisosDTO en lugar de la tipo Permisos
             es por la informacion que esta va a mostrar en la api
             al mandar la Permisos se muestra Id, tipo, autorId y autor, la cual no toda se quiere mostrar
             al manda la tipo PermisosDTO solamente se va a mostrar el Id y el tipo.*/

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int autorId, int id, PermisosCreacionDTO permisoCreacionDTO)
        {
            // Verifica si existe el autor, si no existe retorna 404
            var existeAutor = await dbContext.autores.AnyAsync(autorBD => autorBD.id == autorId);
            if(!existeAutor) { return NotFound(); }

            // Verifica si existe el permiso, si no existe retorna 404
            var existePermiso = await dbContext.permisos.AnyAsync(permisosBD => permisosBD.Id == id);
            if(!existePermiso) { return NotFound(); }

            // mapea la variable permisoCreacionDTO a tipo permiso
            var permiso = mapper.Map<Permisos>(permisoCreacionDTO);
            
            // Hace que el id de la variable permiso sea el id de permiso recibido
            permiso.Id = id;

            // Hace que el autorId de la variable permiso sea el autorId recibido
            permiso.autorId = autorId;

            // actualiza el registro en la base de datos
            dbContext.Update(permiso);
            await dbContext.SaveChangesAsync();

            //204: «Sin contenido». Este código significa que el servidor ha procesado
            //con éxito la solicitud, pero no va a devolver ningún contenido.
            return NoContent();

        }

    }
}

