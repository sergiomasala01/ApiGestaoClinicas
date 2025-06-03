using System.ComponentModel.DataAnnotations;

namespace ApiGestaoClinicas.Models
{
    public class Medico
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
        [StringLength(3, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter 11 caracteres")]
        public string? TipoSanguineo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Especialidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter 2 caracteres")]
        public string? SiglaEstadoCRM { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "O campo {0} precisa ter 6 caracteres")]
        public string? CRM { get; set; }
    }
}
