using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;
namespace Projet_Tech_Pag_Con.Models
{
    public class SeedDataSimu
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();
                // S’il y a déjà des simulations dans la base
                if (context.Simulation.Any())
                {
                    return; // On ne fait rien
                }
                // Sinon on en ajoute un
                context.Simulation.AddRange(

                );
                context.SaveChanges();
            }
        }
    }
}
