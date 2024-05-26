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
        public DbSet<Projet_Tech_Pag_Con.Models.ExecutionMethodesAdmin> ExecutionMethodesAdmin { get; set; }
        public DbSet<Projet_Tech_Pag_Con.Models.SimulationAdmin> SimulationAdmin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurer les relations entre ExecutionMethode et Simulation
            modelBuilder.Entity<ExecutionMethode>()
                .HasOne(em => em.Simulation)  // Utiliser la propriété de navigation Simulation
                .WithMany()  // Une ExecutionMethode peut être liée à plusieurs Simulation
                .HasForeignKey(em => em.SimulationId)  // Utiliser SimulationId comme clé étrangère
                .OnDelete(DeleteBehavior.Restrict);  // Supprimer en cascade les ExecutionMethode associées à une Simulation supprimée

            // Configurer la relation entre Simulation et IdentityUser
            modelBuilder.Entity<Simulation>()
                .HasOne(s => s.Utilisateur)
                .WithMany()
                .HasForeignKey(s => s.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
