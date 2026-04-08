using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCrud.Data;
using ProjetoCrud.Model;
using ProjetoCrud.DTOs;

namespace LeadSyncApi.Controllers
{
    [ApiController] 
    [Route("api/[controller]")] 
    public class LeadsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeadsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadResponseDto>>> GetLeads([FromQuery] string status = null)
        {
            var query = _context.Leads.AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(l => l.Status.ToLower() == status.ToLower());
            }

            var leads = await query.ToListAsync(); 

            var response = leads.Select(l => new LeadResponseDto
            {
                Id = l.Id,
                Name = l.Name,
                Email = l.Email,
                Status = l.Status,
                SourceSystem = l.SourceSystem
            });

            return Ok(response); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadResponseDto>> GetLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound(); 
            }

            var responseDto = new LeadResponseDto
            {
                Id = lead.Id,
                Name = lead.Name,
                Email = lead.Email,
                Status = lead.Status,
                SourceSystem = lead.SourceSystem
            };

            return Ok(responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<LeadResponseDto>> CreateLead(CreateLeadDto leadDto)
        {
            var emailExists = await _context.Leads.AnyAsync(l => l.Email == leadDto.Email);
            if (emailExists)
            {
                return Conflict(new { message = "Um lead com este e-mail já existe." }); 
            }

            var novoLead = new Lead
            {
                Name = leadDto.Name,
                Email = leadDto.Email,
                SourceSystem = leadDto.SourceSystem,
                Status = "Novo", 
                CreatedAt = DateTime.UtcNow
            };

            _context.Leads.Add(novoLead); 
            await _context.SaveChangesAsync(); 

            var responseDto = new LeadResponseDto
            {
                Id = novoLead.Id,
                Name = novoLead.Name,
                Email = novoLead.Email,
                Status = novoLead.Status,
                SourceSystem = novoLead.SourceSystem
            };

            return CreatedAtAction(nameof(GetLead), new { id = novoLead.Id }, responseDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateLead(int id, UpdateLeadDto leadDto)
        {
            var leadExistente = await _context.Leads.FindAsync(id);
            if (leadExistente == null)
            {
                return NotFound();
            }

            leadExistente.Name = leadDto.Name;
            leadExistente.Status = leadDto.Status;
        
            try 
            {
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateException) 
            {
                return StatusCode(500, "Erro ao atualizar o banco de dados");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound(); 
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}