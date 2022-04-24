using AutoMapper;
using WebApiBibliotecaSeg.DTOs;
using WebApiBibliotecaSeg.Entidades;

namespace WebApiAlumnosSeg.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<LibroDTO, Libros>();
            CreateMap<Libros, GetLibroDTO>();

            CreateMap<Libros, LibrosDTOConAutor>()
                .ForMember(LibroDTO => LibroDTO.autores, opciones => opciones.MapFrom(MapLibrosDTOAutor));

            CreateMap<AutorCreacionDTO, Autor>()
                .ForMember(autor => autor.libroAutor, opciones => opciones.MapFrom(MapLibroAutor));

            CreateMap<Autor, AutorDTO>();


            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            CreateMap<EditorialCreacionDTO, Editorial>();
            CreateMap<Editorial, EditorialDTO>();
        }

        private List<AutorDTO> MapLibrosDTOAutor(Libros libro, GetLibroDTO getLibroDTO)
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

        private List<GetLibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
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