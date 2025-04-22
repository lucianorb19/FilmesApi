using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using FilmesApi.Profiles;
using Microsoft.AspNetCore.Mvc;


namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]//DESIGNA A ROTA /endereco (VEM DO NOME DA CLASS)
    public class EnderecoController : ControllerBase
    {
        //PROPRIEDADES - ATRIBUTOS
        private FilmeContext _context;
        private IMapper _mapper;


        //CONSTRUTOR
        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //DEMAIS MÉTODOS
        //MÉTODO QUE ADICIONA UM ENDEREÇO A BD
        [HttpPost]
        public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaEnderecosPorId), new { Id = endereco.Id }, endereco);
        }


        //MÉTODO QUE MOSTRA TODOS OS ENDEREÇOS CADASTRADOS
        /// <summary>
        /// Método que lista todos endereços da aplicação
        /// </summary>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Em caso de leitura bem sucedida</response>
        [HttpGet]
        public IEnumerable<ReadEnderecoDto> RecuperaEnderecos()
        {
            return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos);
        }


        //MÉTODO QUE MOSTRA UM ENDEREÇO, DADO SEU ID
        [HttpGet("{id}")]
        public IActionResult RecuperaEnderecosPorId(int id)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if(endereco != null)
            {
                ReadEnderecoDto enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
                return Ok(enderecoDto);
            }
            return NotFound();
        }


        //MÉTODO QUE ATUALIZA TODO UM ENDEREÇO, DADO SEU ID
        [HttpPut("{id}")]
        public IActionResult AtualizaEndereco(int id, [FromBody] UpdateEnderecoDto enderecoDto)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if(endereco == null)
            {
                return NotFound();
            }
            _mapper.Map(enderecoDto, endereco);//ATUALIZA NO BD
            _context.SaveChanges();
            return NoContent();
        }


        //MÉTODO QUE DELETA UM ENDEREÇO, DADO SEU ID
        [HttpDelete("{id}")]
        public IActionResult DeletaEndereco(int id)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco == null)
            {
                return NotFound();
            }
            _context.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
