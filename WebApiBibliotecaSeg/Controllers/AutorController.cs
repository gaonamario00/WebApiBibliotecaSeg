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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTOConLibros>> GetById(int id)
        {
            var autor = await dbContext.autores
                .Include(libroDb => libroDb.libroAutor)
                .ThenInclude(LibroAutor => LibroAutor.libro)
                .Include(editorialDb => editorialDb.editoriales)
                .FirstOrDefaultAsync(x => x.id == id);

            autor.libroAutor = autor.libroAutor.OrderBy(x => x.orden).ToList();

            return mapper.Map<AutorDTOConLibros>(autor);

        }

        [HttpPost] 
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
        {

           if(autorCreacionDTO.librosIds == null)
            {
                return BadRequest("No se puede crear el autor sin libros");
            }

            var librosIds = await dbContext.libros
                 .Where(libroDb => autorCreacionDTO.librosIds.Contains(libroDb.id)).Select(x => x.id).ToListAsync();

            if(autorCreacionDTO.librosIds.Count != librosIds.Count)
            {
                return BadRequest("No existe uno de los libros enviados");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            if(autor.libroAutor != null)
            {
                for (int i=0; i<autor.libroAutor.Count; i++)
                {
                    autor.libroAutor[i].orden = i;
                }
            }


            dbContext.Add(autor);
            await dbContext.SaveChangesAsync();



            return Ok();
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
