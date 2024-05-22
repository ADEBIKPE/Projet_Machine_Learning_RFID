namespace Projet_Tech_Pag_Con.Models
{
    public class ExecutionMethodesAdmin
    {
        public int Id { get; set; }
        public string NomMethode { get; set; }
        public string Details { get; set; }
        public float Performance { get; set; }
        public string MatriceConfusion { get; set; }
        public string Temps_Execution { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("SimulationId")]
        public int? SimulationId { get; set; }
        public string UserId { get; set; }
        public string UserRoleId { get; set; }

    }

}
