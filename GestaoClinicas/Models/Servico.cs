using System.ComponentModel.DataAnnotations;

namespace ApiGestaoClinicas.Models
{
    public class Servico
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0, 99999, ErrorMessage = "O campo {0} precisa conter no máximo 5 caracteres")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, 600, ErrorMessage = "O campo {0} deve conter no minimo {1} e no maximo {2} minutos")]
        public int Duracao { get; set; }
    }
}
