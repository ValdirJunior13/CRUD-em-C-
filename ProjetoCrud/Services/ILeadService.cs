using ProjetoCrud.DTOs;

namespace ProjetoCrud.Services{
    public interface ILeadService
{
    Task<IEnumerable<LeadResponseDto>> GetLeadsAsync(string status = null);
    Task<LeadResponseDto> GetLeadByIdAsync(int id);
    Task<LeadResponseDto> CreateLeadAsync(CreateLeadDto leadDto);
    Task<bool> UpdateLeadAsync(int id, UpdateLeadDto leadDto);
    Task<bool> DeleteLeadAsync(int id);
}
}
