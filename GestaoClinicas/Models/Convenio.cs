using System.ComponentModel.DataAnnotations;

namespace ApiGestaoClinicas.Models
{
    public class Convenio
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O campo {0} precisa ter {2} a {1} caracteres")]
        public string? Nome { get; set; }
    }
}
