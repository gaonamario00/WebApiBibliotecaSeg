using AutoMapper;
using WebApiBibliotecaSeg.DTOs;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiAlumnosSeg.Utilidades
{
    //Clase para especificar la configuracion del automapper
    // Profile es una clase de AutoMapper que nos permite crear el maping usando los nombres de las clases
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap nos permite mapear un objeto de un tipo a otro

            //CreateMap<Origen, Destino>();

            // Se utiliza para especificar que atributos de estas clases se quiere utilizar 
            // ForMember( destino => destino.atributoEspecificoDeDestino,
            //            opciones => opciones.MapFrom(origen => origen.atributoEspecificoDeOrigen);
            // );

            // En este caso estamos usando funciones para el atributo de origen, el cual es retornado por cada una de estas

            CreateMap<LibroDTO, Libros>();
            CreateMap<Libros, GetLibroDTO>();

            CreateMap<Libros, LibrosDTOConAutor>()
                .ForMember(LibroDTO => LibroDTO.autores, opciones => opciones.MapFrom(MapLibrosDTOAutor));

            CreateMap<AutorCreacionDTO, Autor>()
                .ForMember(autor => autor.libroAutor, opciones => opciones.MapFrom(MapLibroAutor));

            CreateMap<Autor, AutorDTO>();

            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            // ReverseMap: Cree una asignación de tipos desde el destino hasta el tipo de origen,
            // con la validación deshabilitada. Esto permite el mapeo bidireccional.
            CreateMap<AutorPatchDTO, Autor>().ReverseMap();

            CreateMap<PermisosCreacionDTO, Permisos>();
            CreateMap<Permisos, PermisosDTO>();
        }

        // Tranfiere datos de una lista de libroAutor a una lista de AutorDTO
          private List<AutorDTO> MapLibrosDTOAutor(Libros libro,
              GetLibroDTO getLibroDTO // Variable sin usar pero necesaria para poder estar esta funcion para personalizar
                                      // el mapper aunque es indiferente si es tipo GetLibroDTO o LibrosDTOConAutor
                                      // ya que LibrosDTOConAutor hereda de GetLibroDTO
              )
        {
            var result = new List<AutorDTO>();

            if (libro.libroAutor == null) { return result; }

            foreach(var librosAutor in libro.libroAutor)
            {
                result.Add(new AutorDTO()
                {
                    id = librosAutor.autorId,
                    nombre = librosAutor.autor.nombre
                });
            }

            return result;
        }

        // Tranfiere datos de una lista de libroAutor a una lista de GetLibroDTO
        private List<GetLibroDTO> MapAutorDTOLibros(Autor autor, 
            AutorDTO autorDTO // Variable sin usar pero necesaria para poder estar esta funcion para personalizar
                              // el mapper aunque es indiferente si es tipo AutorDTO o AutorDTOConLibros
                              // ya que AutorDTOConLibros hereda de AutorDTO
            )
        {
            var result = new List<GetLibroDTO>();

            if (autor.libroAutor == null)
            {
                return result;
            }

            foreach(var librosAutor in autor.libroAutor)
            {
                result.Add(new GetLibroDTO()
                {
                    id = librosAutor.libroId,
                    titulo = librosAutor.libro.titulo
                });
            }

            return result;

        }

        // Tranfiere datos de una lista de librosIds a una lista de LibroAutor
        private List<LibroAutor> MapLibroAutor(AutorCreacionDTO autorCreacionDTO, Autor autor)
        {
            var result = new List<LibroAutor>();

            if (autorCreacionDTO.librosIds == null) { return result; }

            foreach(var libroId in autorCreacionDTO.librosIds)
            {
                result.Add(new LibroAutor()
                {
                    libroId = libroId
                });
            }

            return result;

        }

    }
}