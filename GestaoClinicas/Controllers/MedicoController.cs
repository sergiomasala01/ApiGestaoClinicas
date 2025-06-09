using ApiGestaoClinicas.Data;
using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/medicos")]
    public class MedicoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public MedicoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedicos()
        {
            return await _context.Medicos.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Medico>> GetMedico(Guid id)
        {
            var medico = await _context.Medicos.FindAsync(id);

            if (medico == null) return NotFound("Médico não encontrado.");

            return Ok(medico);
        }

        [HttpGet("{crm:int}/{siglaEstado}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Medico>> GetMedicoCRM(string crm, string siglaEstado)
        {
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == crm && m.SiglaEstadoCRM == siglaEstado);

            if (medico == null) return NotFound("Médico não encontrado.");

            return Ok(medico);
        }

        [Authorize(Roles = "Admin, Recepcao")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Medico>> PostMedico(Medico medico)
        {
            if (medico == null) return Problem("Erro ao cadastrar médico, contate o suporte!");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(medico);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedico), new {id = medico.Id}, medico);
        }

        [Authorize(Roles = "Admin, Recepcao")]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Medico>> PutMedico(Guid id, Medico medico)
        {
            if (id != medico.Id) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(medico);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Entry(medico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!MedicoExists(id)) return NotFound("Médico não encontrado.");
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
        public async Task<ActionResult<Medico>> DeleteMedico(Guid id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null) return NotFound("Médico não encontrado.");

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string?> Validacoes(Medico medico)
        {
            var cpfExists = await _context.Medicos.AnyAsync(e => e.CPF == medico.CPF && e.Id != medico.Id);
            var telefoneExists = await _context.Medicos.AnyAsync(e => e.Telefone == medico.Telefone && e.Id != medico.Id);
            var emailExists = await _context.Medicos.AnyAsync(e => e.Email == medico.Email && e.Id != medico.Id);
            var crmExistis = await _context.Medicos.AnyAsync(e => e.CRM == medico.CRM && e.SiglaEstadoCRM == medico.SiglaEstadoCRM && e.Id != medico.Id);

            if (cpfExists) return ("Erro ao cadastrar médico, já existe um cadastro para esse CPF.");
            if (telefoneExists) return ("Erro ao cadastrar médico, já existe um cadastro para esse telefone.");
            if (emailExists) return ("Erro ao cadastrar médico, já existe um cadastro para esse e-mail.");
            if (crmExistis) return ("Erro ao cadastrar médico, já existe um cadastro para esse CRM.");

            return null;
        }
        private bool MedicoExists(Guid id)
        {
            return (_context.Medicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}