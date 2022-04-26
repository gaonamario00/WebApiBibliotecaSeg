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
            return Ok();

        }

     }
}

