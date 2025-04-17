using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Cinema
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string Nome { get; set; }

        //PROPRIEDADES RELAÇÃO 1:1 CINEMA <-> ENDERECO
        //TABELA CINEMA TEM DUAS COLUNAS - UMA SENDO O ID DO ENDEREÇO E OUTRA SENDO O ENDEREÇO EM SI
        public int EnderecoId { get; set; }
        public virtual Endereco Endereco { get; set; }
    }
}
