using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessaoController : ControllerBase
    {
        //PROPRIEDADES
        private FilmeContext _context;
        private IMapper _mapper;


        //CONSTRUTOR
        public SessaoController(FilmeContext context, IMapper mapper) //ctor
        {
            _context = context;
            _mapper = mapper;
        }

        //MÉTODO QUE ADICIONA UMA SESSÃO AO SISTEMA
        [HttpPost]
        public IActionResult AdicionaSessao(CreateSessaoDto dto)
        {
            Sessao sessao = _mapper.Map<Sessao>(dto);
            _context.Sessoes.Add(sessao);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaSessoesPorId), new { filmeId = sessao.FilmeId, 
                                                                       cinemaId = sessao.CinemaId}
                                                                     , sessao);
        }


        //MÉTODO QUE MOSTRA AS SESSÕES DO SISTEMA
        [HttpGet]
        public IEnumerable<ReadSessaoDto> RecuperaSessoes()
        {
            return _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes.ToList());
        }


        //MÉTODO QUE MOSTRA UMA SESSÃO, DADO SEU ID
        [HttpGet("{filmeId}/{cinemaId}")]
        public IActionResult RecuperaSessoesPorId(int filmeId, int cinemaId)
        {
            Sessao sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId &&
                                                                      sessao.CinemaId == cinemaId);
            if(sessao != null)
            {
                ReadSessaoDto sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);
                return Ok(sessaoDto);
            }
            return NotFound();
        }
    }
}
