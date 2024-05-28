using Microsoft.AspNetCore.Identity;
using System;

namespace Projet_Tech_Pag_Con.Models
{
    public class SimulationAdmin
    {
        public int Id { get; set; }
        public DateTime DateSimulationAdmin { get; set; }
        public string NomMethodeAdmin { get; set; }
        public string DetailsAdmin { get; set; }
        public float PerformanceAdmin { get; set; }
        public string MatriceConfusionAdmin { get; set; }
        public string Temps_ExecutionAdmin { get; set; }
        public int? SimulationIdAdmin { get; set; }
        public string UserId { get; set; }

        // Navigation property
        public IdentityUser User { get; set; }
    }
}
