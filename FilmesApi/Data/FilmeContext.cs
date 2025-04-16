using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;//DbContext

namespace FilmesApi.Data;

public class FilmeContext : DbContext//CLASSE QUE REMEDIA LIGAÇÃO CLASSES E BASE DE DADOS
{
    //PROPRIEDADES

    //PROPRIEDADE DE ACESSO À BD
    public DbSet<Filme> Filmes{ get; set; }
    public DbSet<Cinema> Cinemas{ get; set; }
    public DbSet<Endereco> Enderecos{ get; set; }


    //CONSTRUTOR QUE USA O CONSTRUTOR BASE (CONSTRUTOR DE DbContext)
    public FilmeContext(DbContextOptions<FilmeContext> opts)
    : base(opts)
    {               
           
    }
}
