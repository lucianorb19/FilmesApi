using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos
{
    public class ReadFilmeDto
    {
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public int Duracao { get; set; }
        public DateTime HoraDaConsulta { get; set; } = DateTime.Now;//VARIÁVEL EXCLUSIVA DO ReadFilmeDto
                                                                    //Hora que o filme foi consultado
        public ICollection<ReadSessaoDto> Sessoes{ get; set; }
    }
}
