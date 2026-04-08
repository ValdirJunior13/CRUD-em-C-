
using System.ComponentModel.DataAnnotations;

namespace ProjetoCrud.Model
{
    public class Lead
    {
        [Key]
        public int Id { get; set; }
        
        public string ExternalId { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        public string Email { get; set; } = string.Empty;
        
        public string SourceSystem { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}