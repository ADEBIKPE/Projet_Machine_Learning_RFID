using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;

namespace Projet_Tech_Pag_Con.Models
{
    public class SeedDataExecutMeth
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();
                // S’il y a déjà des executionsMethodes dans la base
                if (context.ExecutionMethode.Any())
                {
                    return; // On ne fait rien
                }
                // Sinon on en ajoute un
                context.ExecutionMethode.AddRange(

                );
                context.SaveChanges();
            }
        }
    }
}
