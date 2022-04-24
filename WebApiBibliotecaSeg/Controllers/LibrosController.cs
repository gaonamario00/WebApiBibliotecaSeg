﻿using AutoMapper;
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


        [HttpGet("{id:int}")] //Se puede usar ? para que no sea obligatorio el parametro /{param=Gustavo}  getAlumno/{id:int}/
        public async Task<ActionResult<GetLibroDTO>> Get(int id)
        {
            //var libro = await dbContext.libros.FirstOrDefaultAsync(x => x.id == id);
            var libro = await dbContext.libros
                .Include(dbLibro => dbLibro.libroAutor)
                .ThenInclude(dbLibroAutor => dbLibroAutor.autor)
                .FirstOrDefaultAsync(libroDb => libroDb.id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return mapper.Map<GetLibroDTO>(libro);

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

            var existeLibroMismoTitulo = await dbContext.libros.AnyAsync(x => x.titulo == libroDTO.titulo);

            if (existeLibroMismoTitulo)
            {
                return BadRequest($"Ya existe un autor con el nombre {libroDTO.titulo}");
            }

            var alumno = mapper.Map<Libros>(libroDTO);

            dbContext.Add(alumno);
            await dbContext.SaveChangesAsync();

            new EscribirEnArchivoMsg("nuevosRegistros.txt","Titulo: "+libroDTO.titulo + ", agregado el ");

            return Ok();

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
