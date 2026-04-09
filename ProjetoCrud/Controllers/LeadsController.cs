using Microsoft.AspNetCore.Mvc;
using ProjetoCrud.DTOs;
using ProjetoCrud.Services; 

namespace LeadSyncApi.Controllers
{
    [ApiController] 
    [Route("api/[controller]")] 
    public class LeadsController : ControllerBase
    {

        private readonly ILeadService _leadService;

        public LeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadResponseDto>>> GetLeads([FromQuery] string status = null)
        {
            var leads = await _leadService.GetLeadsAsync(status);
            return Ok(leads); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadResponseDto>> GetLead(int id)
        {
            var lead = await _leadService.GetLeadByIdAsync(id);
            
            if (lead == null) return NotFound(); 
            
            return Ok(lead);
        }

        [HttpPost]
        public async Task<ActionResult<LeadResponseDto>> CreateLead(CreateLeadDto leadDto)
        {
            try
            {
                var responseDto = await _leadService.CreateLeadAsync(leadDto);
                return CreatedAtAction(nameof(GetLead), new { id = responseDto.Id }, responseDto);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateLead(int id, UpdateLeadDto leadDto)
        {
            var sucesso = await _leadService.UpdateLeadAsync(id, leadDto);
            
            if (!sucesso) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            var sucesso = await _leadService.DeleteLeadAsync(id);
            
            if (!sucesso) return NotFound();

            return NoContent();
        }
    }
}