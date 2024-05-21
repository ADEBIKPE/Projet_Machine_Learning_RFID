using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Models;

namespace Projet_Tech_Pag_Con.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Projet_Tech_Pag_Con.Models.Simulation>? Simulation { get; set; } 
        public DbSet<Projet_Tech_Pag_Con.Models.ExecutionMethode>? ExecutionMethode { get; set; } 
    }
}
