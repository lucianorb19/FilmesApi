# FilmesApi
 Projeto de uma Web API desenvolvida com .NET 6

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


### ESTRUTURA FRAMEWORK .NET 6.0
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

### VALIDAR DADOS DO USUÁRIO COM Data Annotations

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

### TODOS OS DADOS
Utilizando a mesma classe (FilmeController), agora com um método que recebe HttpGet
```
[HttpGet] //DESIGNA QUE O MÉTODO ABAIXO OBTEM INFORMAÇÕES DA APLICAÇÃO
public IEnumerable<Filme> recuperaFilmes()
{
    return filmes;//LISTA DE FILMES
}
```

### DADOS QUE CORRESPONDEM A UM CRITÉRIO
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



 



