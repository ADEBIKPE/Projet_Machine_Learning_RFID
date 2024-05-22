using Microsoft.AspNetCore.Identity;

namespace Projet_Tech_Pag_Con.Models
{
    public class SimulationAdmin
    {
        public int Id { get; set; }
        public DateTime DateSimulation { get; set; }
        public string UtilisateurId { get; set; }

        // Navigation property
        public IdentityUser Utilisateur { get; set; }
    }
}
