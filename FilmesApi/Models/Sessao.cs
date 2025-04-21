using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Sessao
    {
        //RELAÇÃO SESSÃO<->FILME
        public int? FilmeId { get; set; }
        public virtual Filme Filme { get; set; }

        //RELAÇÃO SESSÃO<->CINEMA
        public int? CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; }
    }
}
