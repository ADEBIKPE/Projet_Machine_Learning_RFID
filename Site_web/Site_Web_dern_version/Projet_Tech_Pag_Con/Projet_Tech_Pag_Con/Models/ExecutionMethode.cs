using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_Tech_Pag_Con.Models
{
    public class ExecutionMethode
    {
        public int Id { get; set; }
        public string NomMethode { get; set; }
        public string Details { get; set; }
        public float Performance { get; set; }
        public string MatriceConfusion { get; set; }
        public string Temps_Execution { get; set; }
        public int? SimulationId { get; set; } // Gardez cette propriété pour la clé étrangère

        // Supprimez les attributs ForeignKey ici pour éviter toute confusion

        // Ajoutez une propriété de navigation vers Simulation
        public virtual Simulation Simulation { get; set; }

        public string UserId { get; set; }
        public string UserRoleId { get; set; }

        // Ajoutez les propriétés de navigation vers IdentityUser et IdentityRole
        public virtual IdentityUser User { get; set; }
        public virtual IdentityRole UserRole { get; set; }
    }
}
