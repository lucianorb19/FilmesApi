using System.ComponentModel.DataAnnotations;
namespace FilmesApi.Models;

public class Filme
{
    [Key]//CAMPO Id É A CHAVE DA ENTIDADE/TABELA
    [Required]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Título do filme obrigatório")]
    [MaxLength(50, ErrorMessage = "Título do filme não pode exceder 50 caractéres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "Gênero do filme obrigatório")]
    [StringLength(50, ErrorMessage = "Gênero do filme não pode exceder 50 caractéres")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "Duração do filme obrigatória")]
    [Range(50,600, ErrorMessage ="Duração do filme precisa ser de 50 minutos a 10H")]
    public int Duracao { get; set; }//DURAÇÃO EM MINUTOS
}
