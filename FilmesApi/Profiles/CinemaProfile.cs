using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;

namespace FilmesApi.Profiles
{
    public class CinemaProfile : Profile
    {
        public CinemaProfile()
        {
            CreateMap<CreateCinemaDto, Cinema>();

            //USADO NO CinemaController->RecuperaCinemas - CONFIGURAR COMO O AUTOMAPPER FUNCIONA
            CreateMap<Cinema, ReadCinemaDto>()
                .ForMember(cinemaDto => cinemaDto.Endereco,
                opt => opt.MapFrom(cinema => cinema.Endereco))
                .ForMember(cinemaDto => cinemaDto.Sessoes,
                opt => opt.MapFrom(cinema => cinema.Sessoes));
            //ForMember(cinemaDto  - PARA O MEMBRO DO DESTINO, QUE É DO TIPO ReadCinemaDto
            //=> cinemaDto.Endereco - ACESSANDO O CAMPO Endereco, QUE É UM CAMPO DESSE OBJETO
            //opt => opt.MapFrom(cinema => cinema.Endereco - QUERO PEGAR, DA ORIGEM, O CAMPO Endereco

            CreateMap<UpdateCinemaDto, Cinema>();

        }
    }
}
