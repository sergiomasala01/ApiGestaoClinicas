using System.ComponentModel.DataAnnotations;
using ApiGestaoClinicas.Data;
using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/servicos")]
    public class ServicoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ServicoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Servico>>> GetServicos()
        {
            return await _context.Servicos.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Servico>> GetServico(Guid id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null) return NotFound("Serviço não encotrado.");

            return Ok(servico);
        }

        [Authorize(Roles = "Admin, Recepcao")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Servico>> PostServico(Servico servico)
        {
            if (servico == null) return NotFound("Serviço não encotrado.");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(servico);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServico), new { id = servico.Id }, servico);
        }

        [Authorize(Roles = "Admin, Recepcao")]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Servico>> PutServico(Guid id, Servico servico)
        {
            if (id != servico.Id) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(servico);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Entry(servico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!ServicoExists(id)) return NotFound("Serviço não encontrado.");
                throw;
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Servico>> DeleteServico(Guid id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null) return NotFound("Serviço não encontrado.");

            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string?> Validacoes(Servico servico)
        {
            var nomeExists = await _context.Servicos.AnyAsync(e => e.Nome == servico.Nome && e.Id != servico.Id);

            if (nomeExists) return ("Erro ao cadastrar serviço, já existe um cadastro com esse nome.");

            return null;
        }

        private bool ServicoExists(Guid id)
        {
            return (_context.Medicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
