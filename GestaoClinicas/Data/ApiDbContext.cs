using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {           
        }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Convenio> Convenios { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Servico> Servicos { get; set; }
    }
}
