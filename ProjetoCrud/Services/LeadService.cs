using Microsoft.EntityFrameworkCore;
using ProjetoCrud.Data;
using ProjetoCrud.DTOs;
using ProjetoCrud.Model;

namespace ProjetoCrud.Services
{
    public class LeadService : ILeadService
    {
        private readonly AppDbContext _context;
        public LeadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeadResponseDto>> GetLeadsAsync(string status = null)
        {
            var query = _context.Leads.AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(l => l.Status.ToLower() == status.ToLower());
            }

            var leads = await query.ToListAsync(); 

            return leads.Select(l => new LeadResponseDto
            {
                Id = l.Id,
                Name = l.Name,
                Email = l.Email,
                Status = l.Status,
                SourceSystem = l.SourceSystem
            });
        }

        public async Task<LeadResponseDto> GetLeadByIdAsync(int id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null) return null;

            return new LeadResponseDto
            {
                Id = lead.Id,
                Name = lead.Name,
                Email = lead.Email,
                Status = lead.Status,
                SourceSystem = lead.SourceSystem
            };
        }

        public async Task<LeadResponseDto> CreateLeadAsync(CreateLeadDto leadDto)
        {
            var emailExists = await _context.Leads.AnyAsync(l => l.Email == leadDto.Email);
            if (emailExists) throw new Exception("Um lead com este e-mail já existe."); 

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

            return new LeadResponseDto
            {
                Id = novoLead.Id,
                Name = novoLead.Name,
                Email = novoLead.Email,
                Status = novoLead.Status,
                SourceSystem = novoLead.SourceSystem
            };
        }

        public async Task<bool> UpdateLeadAsync(int id, UpdateLeadDto leadDto)
        {
            var leadExistente = await _context.Leads.FindAsync(id);
            if (leadExistente == null) return false;

            leadExistente.Name = leadDto.Name;
            leadExistente.Status = leadDto.Status;
        
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLeadAsync(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}