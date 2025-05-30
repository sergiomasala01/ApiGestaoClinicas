using System.ComponentModel.DataAnnotations;

namespace ApiGestaoClinicas.Models
{
    public class Convenio
    {
        [Key]
        public Guid Id { get; set; }
        public string? Nome { get; set; }
    }
}
