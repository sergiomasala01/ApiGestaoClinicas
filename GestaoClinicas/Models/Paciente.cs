using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Models
{
    public class Paciente
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        public DateOnly DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "O campo {0} precisa ter 11 caracteres")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O campo {0} precisa ter 11 caracteres")]
        public string? CPF { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter 2 ou 3 caracteres")]
        public string? TipoSanguineo { get; set; }

        [Required(ErrorMessage = "Campo {0} é obrigatório")]
        [Range(0, 3, ErrorMessage = "O campo {0} precisa ser {1} a {2} metros, Ex: 1.80")]
        public decimal Altura { get; set; }

        [Required(ErrorMessage = "Campo {0} é obrigatório")]
        [Range(0, 400, ErrorMessage = "O campo {0} precisa ser {1} a {2} Kilos, Ex: 80")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Endereco { get; set; }
    }
}
