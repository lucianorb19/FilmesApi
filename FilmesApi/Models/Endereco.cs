﻿using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models;

public class Endereco
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Logradouro { get; set; }
    
    [Required]
    public int Numero { get; set; }

    //PROPRIEDADE RELAÇÃO 1:1 CINEMA<->ENDEREÇO
    public virtual Cinema Cinema { get; set; }

}
