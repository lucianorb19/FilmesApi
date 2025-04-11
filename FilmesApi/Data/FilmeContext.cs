using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;//DbContext

namespace FilmesApi.Data
{
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
}
