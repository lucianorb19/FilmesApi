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
