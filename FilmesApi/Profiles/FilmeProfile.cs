using FilmesApi.Data.Dtos;
using AutoMapper;
using FilmesApi.Models;

namespace FilmesApi.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<UpdateFilmeDto, Filme>();
        CreateMap<Filme, UpdateFilmeDto>();
        CreateMap<Filme, ReadFilmeDto>()
            .ForMember(filmeDto => filmeDto.Sessoes, 
            opt => opt.MapFrom(filme => filme.Sessoes));
        //ForMember(filmeDto  - PARA O MEMBRO DO DESTINO, QUE É DO TIPO ReadFilmeDto
        //=> cinemaDto.Sessoes - ACESSANDO O CAMPO Sessoes, QUE É UM CAMPO DESSE OBJETO
        //opt => opt.MapFrom(filme => filme.Sessoes - QUERO PEGAR, DA ORIGEM, O CAMPO Sessoes
    }
}
