using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using FilmesApi.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;//USO DO [Route("[controller]")] E [ApiController]

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]//DESIGNA A ROTA Filme (VEM DO NOME DA CLASS)
    public class FilmeController : ControllerBase
    {
        //LISTA DE FILMES (SUBSTITUÍDA PELA CONEXÃO COM BD)
        //private static List<Filme> filmes = new List<Filme>();
        //private static int id = 0;//VARIÁVEL QUE SERÁ ATRIBUÍDA AO CAMPO Id do objeto Filme

        private FilmeContext _context;//ATRIBUTO DE CONEXÃO COM BD
        private IMapper _mapper;//ATRIBUTO DTO Filme

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //DOCUMENTAÇÃO SWAGGER
        /// <summary>
        /// Adiciona um filme à base de dados.
        /// </summary>
        /// <param name="filmeDto">DTO usado pelo mapper para efetivar as mudanças na base de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Em caso de inserção bem sucedida</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {                         //[FromBody] DESGINA QUE O OBJETO filmeDto VIRÁ DO CORPO DA REQUISIÇÃO

            //filme.Id = id++;// 0, 1, 2....
            //filmes.Add(filme);
            
            //Objeto filme recebe um objeto Filme a partir do mapeamento de filmeDTO
            Filme filme = _mapper.Map<Filme>(filmeDto);
            _context.Filmes.Add(filme); //FILME ADICIONADO A BD
            _context.SaveChanges(); //MUDANÇAS SALVAS NA BD
            return CreatedAtAction(nameof(RecuperaFilmePorId), 
                                   new { id = filme.Id }, 
                                   filme);

            //CreatedAtAction - MÉTODO PADRÃO REST - RETORNA O OBJETO ADICIONADO E O SEU CAMINHO
            //nameof(RecuperaFilmePorId) new { id = filme.Id } - CAMINHO DO OBJETO CRIADO
            //filme - OBJETO CRIADO
        }


        //MÉTODO QUE LISTA VÁRIOS FILMES DA APLICAÇÃO - PULANDO skip FILMES INICIAIS
        //E MOSTRANDO OS PRÓXIMOS take FILMES
        /// <summary>
        /// Método que lista vários filmes da aplicação
        /// </summary>
        /// <param name="skip"> Quantos filmes iniciais serão desconsiderados</param>
        /// <param name="take"> Quantos filmes serão mostrados</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Em caso de leitura bem sucedida</response>
        [HttpGet] //DESIGNA QUE O MÉTODO ABAIXO OBTEM INFORMAÇÕES DA APLICAÇÃO
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, //SEM DEFINIR, skip É 0
                                                 [FromQuery] int take = 50) //SEM DEFINIR, take É 50
        {
            //return filmes.Skip(skip).Take(take);//LISTA DE FILMES
            //return _context.Filmes.Skip(skip).Take(take);//LISTA DE FILMES
            //RETORNO É UMA MAPPER DA LISTA DTO DO TIPO ReadFilmeDto
            return _mapper.Map<List<ReadFilmeDto>>
                                    (_context.Filmes.Skip(skip).Take(take));

        }


        //MÉTODO QUE RETORNA O PRIMEIRO FILME ENCONTRADO, DADO SEU ID
        /// <summary>
        /// Método que retorna o primeiro filme encontrado, dado seu ID
        /// </summary>
        /// <param name="id"> Identificador único para cada filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Em caso de leitura bem sucedida</response>
        [HttpGet("{id}")]//MÉTODO ABAIXO USA O VERBO GET, MAS COM ID, DIFERENTE DO ACIMA
        public IActionResult RecuperaFilmePorId(int id)
        {      //IActionResult - TIPO DE OBJETO QUE VEM DA INTERFACE ControllerBase
               //SERVE PARA GERAR RETORNOS QUE SÃO RESULTADOS DE REQUISIÇÃO
               //NESSE CASO, NotFound() E Ok() SÃO MÉTODOS COM RETORNO DO TIPO IActionResult

            //var filme = filmes.FirstOrDefault(filme => filme.Id == id);
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();//SE NÃO HOUVER RESULTADO - ERRO 404 - PADRÃO REST
            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);//O RETORNO É UM Dto
            return Ok(filmeDto);//SE HOUVER RESULTADO - NORMAL - 200 OK
        }


        //MÉTODO QUE ATUALIZA UM FILME, DADO SEU ID, COM AS INFORMAÇÕES DO BODY
        //SE NÃO ENCONTRAR O FILME PELO ID - NOT FOUND
        /// <summary>
        /// Método que atualiza um filme, dado seu id, 
        /// com as informações do body da requisição
        /// </summary>
        /// <param name="id">Identificador único do filme</param>
        /// <param name="filmeDto">DTO usado pelo mapper para efetivar as mudanças na base de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Em caso de atualização bem sucedida</response>
        [HttpPut("{id}")]//HttpPut - DESIGNA UMA ATUALIZAÇÃO COMPLETA DO OBJETO
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            //SALVO O FILME PROCURADO EM filme
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            _mapper.Map(filmeDto, filme);//SE ENCONTRAR, filmeDto É MAPEADO/ATUALIZADO PARA filme - MUDANÇA NO BANCO
            _context.SaveChanges();
            return NoContent(); //RETORNO REST PARA UPDATE (204 NoContent)
        }


        //MÉTODO QUE ATUALIZA PARCIALMENTE UM FILME, DADO SEU ID, COM AS INFORMAÇÕES DO BODY
        /// <summary>
        /// Método que atualiza parcialmente um filme, dado seu id, 
        /// com as informações do body da requisição
        /// </summary>
        /// <param name="id">Identificador único do filme</param>
        /// <param name="filmeDto">JsonPatchDocument, que convertido 
        /// para um DTO UpdateFilmeDto, é usado pelo mapper para efetivar
        /// as mudanças na base de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Em caso de atualização bem sucedida</response>
        [HttpPatch("{id}")]//HttpPatch - DESIGNA UMA ATUALIZAÇÃO PARCIAL DO OBJETO
        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {                                                 //RECEBER NO PARÂMETRO UM patch DE ATUALIZAÇÃO
            //SALVO O FILME PROCURADO EM filme
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();

            //VALIDAÇÃO DO PATCH RECEBIDO 
            //1º TORNAR O OBJETO Filme ENCONTRADO NO BANCO, UM UpdateFilmeDto - PARA PODER VERIFICAR SE É APLICÁVEL
            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme); //*ADICIONAR EM FilmeProfile CreateMap<Filme, UpdateFilmeDto>();

            //TENTO APLICAR AS MUDANÇAS DO PATCH AO OBJETO filmeParaAtualizar
            patch.ApplyTo(filmeParaAtualizar, ModelState);

            //SE NÃO CONSEGUIR VALIDAR A ATUALIZAÇÃO COM O PATCH, O ERRO É INDICADO
            if (!TryValidateModel(ModelState))
            {
                return ValidationProblem(ModelState);
            }
            //SE TUDO DER CERTO E O FILME FOR ATUÁLIZAVEL, CONSIDERANDO AS MUDANÇAS INDICADAS
            _mapper.Map(filmeParaAtualizar, filme);//filmeParaAtualizar É MAPEADO/ATUALIZADO PARA filme - MUDANÇA NO BANCO
            _context.SaveChanges();
            return NoContent(); //RETORNO REST PARA UPDATE (204 NoContent)
        }

        /// <summary>
        /// Método que deleta um filme da base de dados
        /// </summary>
        /// <param name="id">
        /// Identificador único do filme
        /// </param>
        /// <returns>IActionResult</returns>
        /// <response code="204">
        ///Em caso de atualização bem sucedida
        ///</response>  
        [HttpDelete("{id}")]//DESIGNA QUE A FUNÇÃO ABAIXO UTILIZARÁ DELEÇÃO
        public IActionResult DeletaFilmes(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            _context.Remove(filme);//DELETA O FILME
            _context.SaveChanges();
            return NoContent();
        }





    }
}
