using ApiGestaoClinicas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Data
{
    public class ApiDbContext : DbContext
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

        //Microsoft.EntityFrameworkCore.Model.Validation[30000] <---- Aviso quando fiz a migration (algo sobre o range dos decimais)
    }
}
