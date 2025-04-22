using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaController : ControllerBase
    {
        //PROPRIEDADES-ATRIBUTOS
        private FilmeContext _context;
        private IMapper _mapper;
        
        //CONSTRUTOR
        public CinemaController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //DEMAIS MÉTODOS
        //MÉTODO QUE ADICIONA UM CINEMA AO BANCO
        [HttpPost]
        public IActionResult AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(
                nameof(RecuperaCinemasPorId), new { Id = cinema.Id }, cinemaDto);
        }


        //MÉTODO QUE
        //MOSTRA TODOS OS CINEMAS DA APLICAÇÃO (SEM PARÂMETROS NA REQUISIÇÃO)
        //MOSTRA OS CINEMAS CUJO ENDEREÇO SEJA enderecoId
        [HttpGet]
        public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery] int? enderecoId = null)
        {
            //SEM PARÂMETROS NA URL - RETORNA TODOS CINEMAS
            if(enderecoId == null)
            {
                return _mapper.Map<List<ReadCinemaDto>>(
                _context.Cinemas.ToList());
            }

            //COM PARÂMETRO - RETORNA CINEMAS CUJO ENDEREÇO SEJA enderecoId
            return _mapper.Map<List<ReadCinemaDto>>
                (_context.Cinemas.FromSqlRaw($"SELECT Id, Nome, EnderecoId FROM cinemas where cinemas.EnderecoId = {enderecoId}").ToList());
        }


        //MÉTODO QUE RETORNA UM CINEMA DADO SEU ID
        [HttpGet("{id}")]
        public IActionResult RecuperaCinemasPorId(int id)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema != null)
            {
                ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
                return Ok(cinemaDto);
            }
            return NotFound();
        }


        //MÉTODO QUE ATUALIZA UM OBJETO CINEMA POR COMPLETO
        //NÃO HÁ UPDATE COM VERBO PATCH, CONSIDERANDO QUE CINEMA SÓ TEM UM CAMPO
        //MODIFICÁVEL - NOME
        [HttpPut("{id}")]
        public IActionResult AtualizaCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null) return NotFound();
            _mapper.Map(cinemaDto, cinema);//REALIZAÇÃO DO UPDATE NA BASE
            _context.SaveChanges();
            return NoContent();
        }


        //MÉTODO QUE DELETA UM CINEMA, DADO SEU ID
        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(int id)
        {
            Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null) return NotFound();
            _context.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
