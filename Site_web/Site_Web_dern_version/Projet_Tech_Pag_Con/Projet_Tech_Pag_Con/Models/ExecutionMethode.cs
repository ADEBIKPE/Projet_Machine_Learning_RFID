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

        [ForeignKey("SimulationId")]
        public int? SimulationId { get; set; }
    }
}
