using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBibliotecaSeg.DTOs;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiBibliotecaSeg.Controllers
{
    [ApiController]
    [Route("autor/{autorId:int}/editoriales")]
    public class EditorialController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public EditorialController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EditorialDTO>>> Get(int autorId)
        {
            var existeAutor = await dbContext.autores.AnyAsync(AutorDb => AutorDb.id == autorId);
            if (!existeAutor)
            {
                return NotFound();
            }

            var editoriales  = await dbContext.autores.Where(AutorDb => AutorDb.id == autorId).ToListAsync();

            return mapper.Map<List<EditorialDTO>>(editoriales);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int autorId, EditorialCreacionDTO editorialCreacionDTO)
        {
            var existeAutor = await dbContext.autores.AnyAsync(AutorDb => AutorDb.id == autorId);
            if (!existeAutor)
            {
                return NotFound();
            }

            var editorial = mapper.Map<Editorial>(editorialCreacionDTO);
            editorial.autorId = autorId;
            dbContext.Add(editorial);

            await dbContext.SaveChangesAsync();
            return Ok();

        }

     }
}

