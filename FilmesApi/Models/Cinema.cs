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
        public int EnderecoId { get; set; }
        
        //virtual
        public virtual Endereco Endereco { get; set; }
    }
}
