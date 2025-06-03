using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiGestaoClinicas.Models
{
    public enum Situacao
    {
        Agendado,
        Cancelado,
        Atendido,
        Faltou
    }

    public class Atendimento
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public Guid IdPaciente { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public Guid IdMedico { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public Guid IdConvenio { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public Guid IdServico { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.DateTime)]
        public DateTime Inicio { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.DateTime)]
        public DateTime Fim { get; set; }

        [Required]
        public Situacao Situacao { get; set; }
    }
}
