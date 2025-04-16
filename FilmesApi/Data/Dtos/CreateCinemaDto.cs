using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos
{
    public class CreateCinemaDto
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string Nome { get; set; }
    }
}
