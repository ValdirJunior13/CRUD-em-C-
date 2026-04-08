namespace ProjetoCrud.DTOs
{
    public class CreateLeadDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SourceSystem { get; set; }
    }

    public class LeadResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string SourceSystem { get; set; }
    }

    public class UpdateLeadDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }
}