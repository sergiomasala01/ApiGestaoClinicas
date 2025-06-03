using ApiGestaoClinicas.Data;
using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Controllers
{
    [ApiController]
    [Route("api/convenios")]
    public class ConvenioController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public ConvenioController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Convenio>>> GetConvenios()
        {
            return await _context.Convenios.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Convenio>> GetConvenio(Guid id)
        {
            var convenio = await _context.Convenios.FindAsync(id);

            if (convenio == null) return NotFound("Convênio não encontrado.");

            return Ok(convenio);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Convenio>> PostConvenio(Convenio convenio)
        {
            if (convenio == null) return Problem("Erro ao cadastrar convênio, contate o suporte!");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(convenio);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Convenios.Add(convenio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConvenio), new { id = convenio.Id }, convenio);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Convenio>> PutConvenio(Guid id, Convenio convenio)
        {
            if (id != convenio.Id) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var erroValidacao = await Validacoes(convenio);
            if (erroValidacao != null) return Conflict(erroValidacao);

            _context.Entry(convenio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!ConvenioExists(id)) return NotFound("Médico não encontrado.");
                throw;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Convenio>> DeleteConvenio(Guid id)
        {
            var convenio = await _context.Convenios.FindAsync(id);
            if (convenio == null) return NotFound("Médico não encontrado.");

            _context.Convenios.Remove(convenio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string?> Validacoes(Convenio convenio)
        {
            var nomeExists = await _context.Convenios.AnyAsync(e => e.Nome == convenio.Nome && e.Id != convenio.Id);

            if (nomeExists) return ("Erro ao cadastrar convênio, já existe um cadastro com esse nome.");

            return null;
        }

        private bool ConvenioExists(Guid id)
        {
            return (_context.Convenios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
