using Microsoft.EntityFrameworkCore;
using ProjetoCrud.Model;

namespace ProjetoCrud.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Lead> Leads{get; set;}
    }
}