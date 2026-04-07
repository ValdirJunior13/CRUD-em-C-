// Arquivo: Controllers/LeadsController.cs
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necessário para os métodos Async do EF (ex: ToListAsync)
using ProjetoCrud.Data;
using ProjetoCrud.Model;

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
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads([FromQuery] string status = null)
        {
    
            var query = _context.Leads.AsQueryable(); 


            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(l => l.Status.ToLower() == status.ToLower());
            }

            var leads = await query.ToListAsync(); 
            return Ok(leads); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound(); 
            }

            return Ok(lead);
        }

        [HttpPost]
        public async Task<ActionResult<Lead>> CreateLead(Lead lead)
        {

            var emailExists = await _context.Leads.AnyAsync(l => l.Email == lead.Email);
            if (emailExists)
            {
                return Conflict(new { message = "Um lead com este e-mail já existe." }); 
            }
            lead.CreatedAt = DateTime.UtcNow;

            _context.Leads.Add(lead); 
            await _context.SaveChangesAsync(); 
            return CreatedAtAction(nameof(GetLead), new { id = lead.Id }, lead);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Lead>> UpdateLead(int id, Lead leadAtualizado)
        {
           if(id != leadAtualizado.Id)
            {
                return BadRequest(new {message = "O Id da URL não corresponde ao solicitado"});
            }

            var leadExistente = await _context.Leads.FindAsync(id);
            if(leadExistente == null)
            {
                return NotFound();
            }

            leadExistente.Name = leadAtualizado.Name;
        leadExistente.Status = leadAtualizado.Status;
        
        try{
            await _context.SaveChangesAsync();
        }catch (DbUpdateException){
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