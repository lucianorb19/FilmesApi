using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;//DbContext

namespace FilmesApi.Data;

public class FilmeContext : DbContext//CLASSE QUE REMEDIA LIGAÇÃO CLASSES E BASE DE DADOS
{
    //PROPRIEDADES

    //PROPRIEDADE DE ACESSO À BD - TORNAM-SE NOMES DAS TABELAS
    public DbSet<Filme> Filmes{ get; set; }
    public DbSet<Cinema> Cinemas{ get; set; }
    public DbSet<Endereco> Enderecos{ get; set; }
    public DbSet<Sessao> Sessoes{ get; set; }


    //CONSTRUTOR QUE USA O CONSTRUTOR BASE (CONSTRUTOR DE DbContext)
    public FilmeContext(DbContextOptions<FilmeContext> opts)
    : base(opts)
    {               
           
    }

    //DEMAIS MÉTODOS
    //MÉTODO QUE DEFINE COMPORTAMENTOS PARA A CRIAÇÃO DE ENTIDADES
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //MÉTODO QUE CRIA A CHAVE PRIMÁRIA DE SESSOES - COMPOSTA por FilmeId + CinemaId
        //E QUE RELACIONA A TABELA SESSOES COM FILME E CINEMA (AGORA QUE ELA É A TABLE QUE GUARDA A
        //RELAÇÃO N:N ENTRE FILME E CINEMA

        //CADA SESSÃO TEM COMO CHAVE PRIMÁRIA FilmeId+CinemaId
        builder.Entity<Sessao>().HasKey(sessao => new { sessao.FilmeId, sessao.CinemaId });

        //COMO A SESSÃO SE RELACIONA COM CINEMA? 
        builder.Entity<Sessao>().HasOne(sessao => sessao.Cinema) //1 SESSÃO -> 1 CINEMA
                                .WithMany(cinema => cinema.Sessoes) //1 CINEMA -> 1 OU MUITAS SESSÕES
                                .HasForeignKey(sessao => sessao.CinemaId); //CHAVE ESTRANGEIRA: SESSAO->CinemaId PARA CHAVE PRIMÁRIA DE Cinema

        //COMO A SESSÃO SE RELACIONA COM FILME? 
        builder.Entity<Sessao>().HasOne(sessao => sessao.Filme) //1 SESSÃO -> 1 FILME
                                .WithMany(filme => filme.Sessoes) //1 FILME -> 1 OU MUITAS SESSÕES
                                .HasForeignKey(sessao => sessao.FilmeId); //CHAVE ESTRANGEIRA: SESSAO->FilmeId PARA CHAVE PRIMÁRIA DE Filme

        builder.Entity<Endereco>().HasOne(endereco => endereco.Cinema) //1 ENDERECO -> 1 CINEMA
                                  .WithOne(cinema => cinema.Endereco) //1 CINEMA -> 1 ENDERECO
                                  .OnDelete(DeleteBehavior.Restrict); //DELEÇÃO RESTRITA
                                                                      //NÃO DELETA SE HOUVER CHAVES PRIMÁRIAS NA TUPLA
    
    
    }



}
