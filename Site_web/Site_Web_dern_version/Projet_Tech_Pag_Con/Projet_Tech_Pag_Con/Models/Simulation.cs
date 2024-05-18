using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Projet_Tech_Pag_Con.Models
{
    public class Simulation
    {
        public int Id { get; set; }

        [Display(Name = "Date de la simulation")]
        [DataType(DataType.DateTime)]
        public DateTime DateSimulation { get; set; } = DateTime.Now;
        public string UtilisateurId { get; set; }
        //public virtual IdentityUser user;
    }
}
