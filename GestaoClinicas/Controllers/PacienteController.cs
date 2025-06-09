using ApiGestaoClinicas.Data;
using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/pacientes")]
    public class PacienteController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public PacienteController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {          
            return await _context.Pacientes.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Paciente>> GetPaciente(Guid id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null) return NotFound("Paciente não encontrado.");

            return Ok(paciente);
        }

        [HttpGet("{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Paciente>> GetPacienteCPF(string cpf)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CPF == cpf);

            if (paciente == null) return NotFound("Paciente não encontrado.");

            return Ok(paciente);
        }

        [Authorize(Roles = "Admin, Recepcao")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        {
            if (paciente == null) return Problem("Erro ao cadastrar paciente, contate o suporte!");
            
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(paciente);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new {id = paciente.Id}, paciente);
        }

        [Authorize(Roles = "Admin, Recepcao")]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Paciente>> PutPaciente(Guid id, Paciente paciente)
        {
            if (id != paciente.Id) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(paciente);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!PacienteExists(id)) return NotFound("Paciente não encontrado.");
                throw;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Paciente>> DeletePaciente(Guid id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound("Paciente não encontrado.");

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<string?> Validacoes(Paciente paciente)
        {
            var cpfExists = await _context.Pacientes.AnyAsync(e => e.CPF == paciente.CPF && e.Id != paciente.Id);
            var telefoneExists = await _context.Pacientes.AnyAsync(e => e.Telefone == paciente.Telefone && e.Id != paciente.Id);
            var emailExists = await _context.Pacientes.AnyAsync(e => e.Email == paciente.Email && e.Id != paciente.Id);

            if (cpfExists) return ("Erro ao cadastrar paciente, já existe um cadastro para esse CPF.");
            if (telefoneExists) return ("Erro ao cadastrar paciente, já existe um cadastro para esse telefone.");
            if (emailExists) return ("Erro ao cadastrar paciente, já existe um cadastro para esse e-mail.");

            return null;
        }
        private bool PacienteExists(Guid id)
        {
            return (_context.Pacientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
