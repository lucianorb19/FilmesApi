# FilmesApi
 Projeto de uma Web API Restful desenvolvida com .NET 6  
 A API é capaz de guardar, num banco de dados relacional, registros de filmes com:  
 * Título;
 * Gênero;
 * Duração.

 As operações que podem ser realizadas pela API são:  
 * Inserção de filme na BD;
 * Consulta a todos os filmes cadastrados;
 * Consulta a um filme específico, dado seu ID;
 * Atualização de todos os campos do filme;
 * Atualização parcial dos campos do filme;
 * Deleção.

 ## DOWNLOADS NECESSÁRIOS
 **_PARA O SISTEMA OPERACIONAL_**
 * VisualStudio Community 2022;
* .NET 6.0.402;
* MySQL Community 8.0.31 (MySQL Server e MySQLWorkbench);
* Postman - Versão mais recente

**_NO VISUAL STUDIO_**
_Ferramentas -> Gerenciador de Pacotes NuGet- Gerenciar pacotes solução->Procurar-> Baixar_
* Microsoft.EntityFrameworkCore - (v. 6.0.10);
* MicrosoftEntityFrameworkCore.Tools (v. 6.0.10);
* Pomelo.EntityFrameworkCore.MySql - (v. 6.0.2);
* AutoMapper (v. 12.0.0);
* AutoMapper.Extensions.Microsoft.DependencyInjection (v.12.0.0);
* Microsoft.AspNetCore.Mvc.NewtonSoftJson (v.6.0.1)

## ANOTAÇÕES / COMO O PROJETO FOI DESENVOLVIDO
### MACETES
-Alt + Enter com mouse posicionado sobre a abertura dos colchetes- transforma a estrutura do código para estrutura de arquivo
```
namespace FilmesApi.Models
{
    public class Filme
    {
    }
}
```
PARA
```
namespace FilmesApi.Models;

public class Filme
{
}
```
-Alt + Enter com mouse posicionado sobre o erro de importação de arquivo - constrói a importação automaticamente

### TEORIA
#### API - Application Programming Interface
Uma interface de serviço - seguindo suas regras, a API disponibiliza serviços sem que o cliente precise se preocupar em como ela faz isso no servidor desse respectivo serviço.

#### Rest - um tipo de arquitetura de API - um padrão. Uma API que segue esse padrão é chamada de Restful.


### ESTRUTURA FRAMEWORK .NET 6.0 (CONSIDERANDO O PROJETO EXEMPLO INICIAL)
#### Program - Onde a aplicação é iniciada, configurações, definições, dependências,... 

#### WeatherForecast.cs - Modelo de API base

#### Appsettings.Development.json e appsettings.json - definições de bancos de dados, senhas, usuários,...

#### Controllers -> WeatherForecastController.cs - responsável por receber as requisições do usuário

#### launchSettings.json -> profiles - define a url da aplicação

### CRIANDO UMA APLICAÇÃO DO 0 - FILMESAPI

APAGAR Controllers -> WeatherForecastController
APAGAR WeatherForecast

CRIAR Controllers -> FilmeController.cs (classe)
```
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;//USO DO [Route("[controller]")] E [ApiController]

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]//DESIGNA A ROTA Filme (VEM DO NOME DA CLASS)
    public class FilmeController : ControllerBase
    {
        //LISTA DE FILMES
        private static List<Filme> filmes = new List<Filme>();

        //MÉTODO QUE ADICIONA UM OBJETO FILME À LISTA
        [HttpPost] // DESIGNA QUE O MÉTODO ABAIXO INSERE INFORMAÇÕES NA APLICAÇÃO
        public void adicionaFilme([FromBody] Filme filme)
        {                         //[FromBody] DESGINA QUE O PARÂMETRO VIRÁ DO CORPO DA REQUISIÇÃO
            filmes.Add(filme);
            Console.WriteLine($"{filme.Titulo}\n{filme.Duracao}");//VERIFICAÇÃO DA INFORMAÇÃO
        }
    }
}
```

CRIAR Models->Filme.cs
```
namespace FilmesApi.Models;

public class Filme
{
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }//DURAÇÃO EM MINUTOS
}
```

Em launchSettings.json - MUDAR
```
"launchBrowser": false, //BROWSER NÃO ABRE AUTOMATICAMENTE AO EXECUTAR A APLICAÇÃO
```

### INSERÇÃO DE DADOS NA APLICAÇÃO - POST
Criação de um método que adiciona um objeto filme na memória, dado um estrutura json passada pelo Post. Isso é definido pelo método adicionaFilme no controlador
```
private static int id = 0;//VARIÁVEL QUE SERÁ ATRIBUÍDA AO CAMPO Id do objeto Filme

//MÉTODO QUE ADICIONA UM OBJETO FILME À LISTA
[HttpPost] // DESIGNA QUE O MÉTODO ABAIXO INSERE INFORMAÇÕES NA APLICAÇÃO
public void AdicionaFilme([FromBody] Filme filme)
{                         //[FromBody] DESGINA QUE O PARÂMETRO VIRÁ DO CORPO DA REQUISIÇÃO
            
    filme.Id = id++;// 0, 1, 2....
    filmes.Add(filme);
            
}
```
Porém, ao seguir o padrão Rest, sempre que algo for adicionado, é necessário, além da inserção, retornar para o usuário o objeto adicionado e o caminho para ele. Isso pode ser feito pelo método CreatedAtAction().
```
private static int id = 0;//VARIÁVEL QUE SERÁ ATRIBUÍDA AO CAMPO Id do objeto Filme

//MÉTODO QUE ADICIONA UM OBJETO FILME À LISTA
[HttpPost] // DESIGNA QUE O MÉTODO ABAIXO INSERE INFORMAÇÕES NA APLICAÇÃO
public IActionResult AdicionaFilme([FromBody] Filme filme)
{                         //[FromBody] DESGINA QUE O PARÂMETRO VIRÁ DO CORPO DA REQUISIÇÃO
            
    filme.Id = id++;// 0, 1, 2....
    filmes.Add(filme);
    return CreatedAtAction(nameof(RecuperaFilmePorId), 
                           new { id = filme.Id }, 
                           filme);

    //CreatedAtAction - MÉTODO PADRÃO REST - RETORNA O OBJETO ADICIONADO E O SEU CAMINHO
    //nameof(RecuperaFilmePorId) new { id = filme.Id } - CAMINHO DO OBJETO CRIADO
    //filme - OBJETO CRIADO
}
```

### VALIDAR DADOS DO USUÁRIO COM _Data Annotations_

Validar a entrada de dados do usuário pode ser feita na própria classe em Models
```
using System.ComponentModel.DataAnnotations;
namespace FilmesApi.Models;

public class Filme
{
    [Required(ErrorMessage = "Título do filme obrigatório")]
    [MaxLength(50, ErrorMessage = "Título do filme não pode exceder 50 caractéres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "Gênero do filme obrigatório")]
    [MaxLength(50, ErrorMessage = "Gênero do filme não pode exceder 50 caractéres")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "Duração do filme obrigatória")]
    [Range(50,600, ErrorMessage ="Duração do filme precisa ser de 50 minutos a 10H")]
    public int Duracao { get; set; }//DURAÇÃO EM MINUTOS
}
```
Assim, toda vez que um objeto Filme for usado, seus atributos/propriedades seguirão estas restrições de
[Required] - atributo não pode ser vazio
[MaxLength] ou [StringLength] - tamanho máximo da string
[Range] - valor mínimo e máximo
(ErrorMessage = “...”) - personaliza a mensgem de erro mostrada na tela


### LEITURA DE DADOS NA APLICAÇÃO - GET

#### TODOS OS DADOS
Utilizando a mesma classe (FilmeController), agora com um método que recebe HttpGet
```
[HttpGet] //DESIGNA QUE O MÉTODO ABAIXO OBTEM INFORMAÇÕES DA APLICAÇÃO
public IEnumerable<Filme> recuperaFilmes()
{
    return filmes;//LISTA DE FILMES
}
```

#### DADOS QUE CORRESPONDEM A UM CRITÉRIO
Utilizando a mesma classe (FilmeController), agora com um método que recebe HttpGet e um parâmetro id
```
//MÉTODO QUE RETORNA O PRIMEIRO FILME ENCONTRADO, DADO SEU ID
[HttpGet("{id}")]//MÉTODO ABAIXO USA O VERBO GET, MAS COM ID, DIFERENTE DO ACIMA
public Filme? RecuperaFilmePorId(int id)
{      //Filme? - NULLABLE - RETORNO PODE ASSUMIR VALOR NULL

    return filmes.FirstOrDefault(filme => filme.Id == id);
}
```
Mudada também a estrutura da classe filme
```
public int Id { get; set; }
```

### PAGINAÇÃO DE DADOS COM SKIP E TAKE
Quando a quantidade de dados em memória for muito grande, pode ser útil dar ao usuário a opção de mostrar partes desses dados somente. Isso pode ser feito com os métodos Skip e Take aplicados na função que usar o verbo Get no Controlador.
```
//MÉTODO QUE LISTA VÁRIOS FILMES DA APLICAÇÃO - PULANDO skip FILMES INICIAIS
//E MOSTRANDO OS PRÓXIMOS take FILMES
[HttpGet] //DESIGNA QUE O MÉTODO ABAIXO OBTEM INFORMAÇÕES DA APLICAÇÃO
public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, //SEM DEFINIR, skip É 0
                                         [FromQuery] int take = 50) //SEM DEFINIR, take É 50
{
    return filmes.Skip(skip).Take(take);//LISTA DE FILMES
}
```

### CONEXÃO COM BASE DE DADOS
Downloads necessários
Ferramentas -> Gerenciador de Pacotes NuGet- Gerenciar pacotes solução->Procurar-> Baixar  
Microsoft.EntityFrameworkCore - versão. 6.0.10;  
MicrosoftEntityFrameworkCore.Tools versão. 6.0.10;  
Pomelo.EntityFrameworkCore.MySql - versão 6.0.2;  


CRIAR PASTA Data -> Vai intermediar a ligação entre a base de dados e as entidades da aplicação  
Data-> class FilmeContext
```
public class FilmeContext : DbContext//CLASSE QUE REMEDIA LIGAÇÃO FILME (ENTIDADE) <-> FILME (BD)
{
    //PROPRIEDADES

    //PROPRIEDADE DE ACESSO AOS FILMES DA BD
    public DbSet<Filme> Filmes{ get; set; }


    //CONSTRUTOR QUE USA O CONSTRUTOR BASE (CONSTRUTOR DE DbContext)
    public FilmeContext(DbContextOptions<FilmeContext> opts)
    : base(opts)
    {               
               
    }
}
```

Configurar arquivo appsettings.json, considerando
Servidor local, nome da base de dados filme, usuário root e senha root
```
"AllowedHosts": "*",
"ConnectionStrings": {
  "FilmeConnection": "server=localhost;database=filme;user=root;password=root"
}
```

Configurar program.cs
```
using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//VARIÁVEL PARA DEFINIÇÕES ABAIXO
var connectionString = builder.Configuration.GetConnectionString("FilmeConnection");

builder.Services.AddDbContext<FilmeContext>(opts =>
opts.UseMySql(connectionString,
              ServerVersion.AutoDetect(connectionString)));
```
CONFIGURAR  Models->Class Filme
```
public class Filme
{
    [Key]//CAMPO Id É A CHAVE DA ENTIDADE/TABELA
    [Required]
public int Id { get; set; }
.
.
.
}
```

Executar as mudanças do código para a BD
Ferramentas-> Gerenciador de Pacotes NuGet->Console Gerenciador de Pacotes    
_Add-Migration CriandoTabelaDeFilmes_ - Constrói a estrutura da tabela  
_Update-Database_ - aplica as mudanças na base de dados MySql  


Com a estrutura do BD criado e com a conexão com BD configurada no código, aplicar as mudanças para que o código efetivamente utilize essa conexão, na classe FilmeController
```
public class FilmeController : ControllerBase
{
    //LISTA DE FILMES (SUBSTITUÍDA PELA CONEXÃO COM BD)
    //private static List<Filme> filmes = new List<Filme>();
    //private static int id = 0;//VARIÁVEL QUE SERÁ ATRIBUÍDA AO CAMPO Id do objeto Filme

    private FilmeContext _context;//VARIÁVEL DE CONEXÃO COM BD

    public FilmeController(FilmeContext context)
    {
        _context = context;
    }

    //MÉTODO QUE ADICIONA UM OBJETO FILME À LISTA
    [HttpPost] // DESIGNA QUE O MÉTODO ABAIXO INSERE INFORMAÇÕES NA APLICAÇÃO
    public IActionResult AdicionaFilme([FromBody] Filme filme)
    {                         //[FromBody] DESGINA QUE O PARÂMETRO VIRÁ DO CORPO DA REQUISIÇÃO

        //filme.Id = id++;// 0, 1, 2....
        //filmes.Add(filme);
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
    [HttpGet] //DESIGNA QUE O MÉTODO ABAIXO OBTEM INFORMAÇÕES DA APLICAÇÃO
    public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, //SEM DEFINIR, skip É 0
                                             [FromQuery] int take = 50) //SEM DEFINIR, take É 50
    {
        //return filmes.Skip(skip).Take(take);//LISTA DE FILMES
        return _context.Filmes.Skip(skip).Take(take);//LISTA DE FILMES
    }


    //MÉTODO QUE RETORNA O PRIMEIRO FILME ENCONTRADO, DADO SEU ID
    [HttpGet("{id}")]//MÉTODO ABAIXO USA O VERBO GET, MAS COM ID, DIFERENTE DO ACIMA
    public IActionResult RecuperaFilmePorId(int id)
    {      //IActionResult - TIPO DE OBJETO QUE VEM DA INTERFACE ControllerBase
           //SERVE PARA GERAR RETORNOS QUE SÃO RESULTADOS DE REQUISIÇÃO
           //NESSE CASO, NotFound() E Ok() SÃO MÉTODOS COM RETORNO DO TIPO IActionResult

        //var filme = filmes.FirstOrDefault(filme => filme.Id == id);
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();//SE NÃO HOUVER RESULTADO - ERRO 404 - PADRÃO REST
        return Ok(filme);//SE HOUVER RESULTADO - NORMAL - 200 OK
    }
}
```

## DTO - Data Transfer Object - Em Criar um Filme (adicionaFilme)

Downloads necessários:  
Ferramentas->Gerenciador de Pacotes Nuget->   
AutoMapper (v. 12.0.0)  
AutoMapper.Extensions.Microsoft.DependencyInjection (v.12.0.0)  

Para que a classe Filmes fique menos exposta no contato com as requisições dos POSTs e GETs, é recomendável usar uma classe DTO, que vai intermediar a ligação entre a FilmeController e Filme, e vai, baseada no Model Filme, mostrar e fornecer informações para as requisições. Sendo assim, a nova classe FilmeDto fica em Data->Dtos-> Classe CreateFilmeDto
```
public class CreateFilmeDto//CLASSE QUE INTERMEDIA ENTRE Controll e Filme
{

    [Required(ErrorMessage = "Título do filme obrigatório")]
    [StringLength(50, ErrorMessage = "Título do filme não pode exceder 50 caractéres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "Gênero do filme obrigatório")]
    [StringLength(50, ErrorMessage = "Gênero do filme não pode exceder 50 caractéres")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "Duração do filme obrigatória")]
    [Range(50, 600, ErrorMessage = "Duração do filme precisa ser de 50 minutos a 10H")]
    public int Duracao { get; set; }//DURAÇÃO EM MINUTOS
} 
```

Para que seja possível usar essa classe DTO em FilmeController, baixar  
AutoMapper e AutoMapper.Extensions.Microsoft.DependencyInjection.  
Em Program
```
//AUTOMAPPER PODE SER USADO EM TODA A APLICAÇÃO
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Criar Profiles->FilmeProfile.cs, que vai ser a classe que vai possibilitar mapear um objeto CreateFilmeDto em Filme.

using FilmesApi.Data.Dtos;
using AutoMapper;
using FilmesApi.Models;

namespace FilmesApi.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeDto, Filme>();
    }
}
```

E as devidas mudanças são feitas em FilmeController, dado que agora a classe usada nos parâmetros de AdicionaFilme é CreateFilmeDto, e não Filme.
```
private IMapper _mapper;//ATRIBUTO DTO Filme

public FilmeController(FilmeContext context, IMapper mapper)
{
    _context = context;
    _mapper = mapper;
}

//MÉTODO QUE ADICIONA UM OBJETO FILME À LISTA
[HttpPost] // DESIGNA QUE O MÉTODO ABAIXO INSERE INFORMAÇÕES NA APLICAÇÃO
public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
{                         //[FromBody] DESGINA QUE O PARÂMETRO VIRÁ DO CORPO DA REQUISIÇÃO

    //filme.Id = id++;// 0, 1, 2....
    //filmes.Add(filme);
            
    //Objeto filme recebe um objeto Filme a partir do mapeamento de filmeDTO
    Filme filme = _mapper.Map<Filme>(filmeDto);
    _context.Filmes.Add(filme); //FILME ADICIONADO A BD
    _context.SaveChanges(); //MUDANÇAS SALVAS NA BD
    return CreatedAtAction(nameof(RecuperaFilmePorId), 
                           new { id = filme.Id }, 
                           filme);
}
```

### Update Filme (já com DTO) - PUT
Atualizar as informações do objeto com o verbo Http PUT, que atualiza um objeto completo.

Data->Dtos-> Classe UpdateFilmeDto (com mesmo conteúdo de CreateFilmeDto)

Profile->FilmeProfile.cs -> Adicionar ao construtor o método que vai possibilitar o AutoMap a conveter UpdateFilmeDto para Filme
```
public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<UpdateFilmeDto, Filme>();
    }
}
```
### Update Filme (já com DTO) - PATCH
Downloads necessários:  
Ferramentas->Geranciador de Pacotes Nuget->   
Microsoft.AspNetCore.Mvc.NewtonSoftJson (v.6.0.1)  

Atualizar as informações do objeto com o verbo Http PATCH, que atualiza parcialmente um objeto.


Programs->
```
builder.Services.AddControllers().AddNewtonsoftJson();


Profiles-> FilmeProfile
 public FilmeProfile()
 {
     CreateMap<CreateFilmeDto, Filme>();
     CreateMap<UpdateFilmeDto, Filme>();
     CreateMap<Filme, UpdateFilmeDto>();
 }
```
FilmeController
```
//MÉTODO QUE ATUALIZA PARCIALMENTE UM FILME, DADO SEU ID, COM AS INFORMAÇÕES DO BODY
[HttpPatch("{id}")]//HttpPatch - DESIGNA UMA ATUALIZAÇÃO PARCIAL DO OBJETO
public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
{                                                 //RECEBER NO PARÂMETRO UM patch DE ATUALIZAÇÃO
    //SALVO O FILME PROCURADO EM filme
    var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
    if (filme == null) return NotFound();

    //VALIDAÇÃO DO PATCH RECEBIDO 
    //1º TORNAR O OBJETO Filme ENCONTRADO NO BANCO, UM UpdateFilmeDto - PARA PODER VERIFICAR SE É APLICÁVEL O PATCH
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
```

### DELETE FILME
```
[HttpDelete("{id}")]//DESIGNA QUE A FUNÇÃO ABAIXO UTILIZARÁ DELEÇÃO
public IActionResult DeletaFilmes(int id)
{
    var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
    if (filme == null) return NotFound();
    _context.Remove(filme);//DELETA O FILME
    _context.SaveChanges();
    return NoContent();
}
```
### LEITURA DE DADOS - COM DTO

Data->Dtos->ReadFilmeDTO
```
public class ReadFilmeDto
{
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }
    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;//VARIÁVEL EXCLUSIVA DO ReadDto
                                                                //Hora que o filme foi consultado
}
```

FilmeController-> Ambos métodos de leitura de dados modificados para retornar DTOs
```
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
```
FilmeProfile->Adicionar
```
 CreateMap<Filme, ReadFilmeDto>();
```

### DOCUMENTAÇÃO DA APLICAÇÃO
A documentação, pelo swagger, pode ser feita através dos próprios métodos
FilmeController->
```
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
{                         //[FromBody] DESGINA QUE O PARÂMETRO VIRÁ DO CORPO DA REQUISIÇÃO

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

Program->
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FilmesApi", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


Para habilitar a exibição de documentação no swagger
Clique duplo no projeto->
 <PropertyGroup>
   <TargetFramework>net6.0</TargetFramework>
   <Nullable>enable</Nullable>
   <ImplicitUsings>enable</ImplicitUsings>
<GenerateDocumentationFile>true</GenerateDocumentationFile>
 </PropertyGroup>
```

 



