using ApiGestaoClinicas.Data;
using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Controllers
{
    [ApiController]
    [Route("api/atendimentos")]
    public class AtendimentoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public AtendimentoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Atendimento>>> GetAtendimentos()
        {
            return await _context.Atendimentos.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Atendimento>> GetAtendimento(Guid id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);

            if (atendimento == null) return NotFound("Atendimento não encontrado.");

            return Ok(atendimento);
        }

        [HttpGet("medico/{nomeMedico}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Atendimento>>> GetAtendimentosPorMedico(string nomeMedico)
        {
            var medico = await _context.Medicos.Where(m => m.Nome.Contains(nomeMedico)).ToListAsync();

            if (medico == null) return NotFound("Nenhum médico encontrado.");

            var idMedicos = medico.Select(m => m.Id).ToList();
            var atendimentos = await _context.Atendimentos.Where(a => idMedicos.Contains(a.IdMedico)).ToListAsync();

            if (atendimentos == null || atendimentos.Count == 0) return NotFound("Nenhum atendimento encontrado para o médico informado.");

            return Ok(atendimentos);
        }

        [HttpGet("paciente/{nomePaciente}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Atendimento>>> GetAtendimentosPorPaciente(string nomePaciente)
        {
            var paciente = await _context.Pacientes.Where(m => m.Nome.Contains(nomePaciente)).ToListAsync();

            if (paciente == null) return NotFound("Nenhum paciente encontrado.");

            var idPacientes = paciente.Select(m => m.Id).ToList();
            var atendimentos = await _context.Atendimentos.Where(a => idPacientes.Contains(a.IdPaciente)).ToListAsync();

            if (atendimentos == null || atendimentos.Count == 0) return NotFound("Nenhum atendimento encontrado para o paciente informado.");

            return Ok(atendimentos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Atendimento>> PostAtendimento(Atendimento atendimento)
        {
            if (atendimento == null) return Problem("Erro ao agendar, contate o suporte!");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(atendimento);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAtendimento), new { id = atendimento.Id }, atendimento);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Atendimento>> PutAtendimento(Guid id, Atendimento atendimento)
        {
            if (id != atendimento.Id) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(atendimento);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Entry(atendimento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!AtendimentoExists(id)) return NotFound("Atendimento não encontrado.");
                throw;
            }

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Atendimento>> DeleteAtendimento(Guid id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento == null) return NotFound("Atendimento não encontrado.");

            _context.Atendimentos.Remove(atendimento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string?> Validacoes(Atendimento atendimento)
        {
            var pacienteExists = await _context.Pacientes.AnyAsync(p => p.Id == atendimento.IdPaciente);
            var medicoExists = await _context.Medicos.AnyAsync(m => m.Id == atendimento.IdMedico);
            var convenioExists = await _context.Convenios.AnyAsync(c => c.Id == atendimento.IdConvenio);
            var servicoExists = await _context.Servicos.AnyAsync(s => s.Id == atendimento.IdServico);

            if (!pacienteExists) return ("Erro ao vincular paciente, não existe um cadastro com esse Id.");
            if (!medicoExists) return ("Erro ao vincular médico, não existe um cadastro com esse Id.");
            if (!convenioExists) return ("Erro ao vincular convênio, não existe um cadastro com esse Id.");
            if (!servicoExists) return ("Erro ao vincular serviço, não existe um cadastro com esse Id.");

            if (!Enum.IsDefined(typeof(Situacao), atendimento.Situacao))
            {
                return "Erro ao vincular Situação, não existe um cadastro com esse nome";
            }

            return null;
        }

        private bool AtendimentoExists(Guid id)
        {
            return (_context.Atendimentos?.Any(e => e.Id == id)).GetValueOrDefault(); 
        }
    }
}
