﻿using System.ComponentModel.DataAnnotations;

namespace ApiGestaoClinicas.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O campo {0}  é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo {0}  é obrigatório")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "O campo {0} presisa ter entre {2} e {1} caracteres")]
        public string? Password { get; set; }
    }
}
