using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain
{
    public class PlanAlimenticio: AggregateRoot
    {
        [Key]
        public Guid Id { get;  set; }
        public string Nombre { get;  set; }
        public string Descripcion { get;  set; }
        public DuracionPlan DuracionPlan { get;  set; }
        public EstadoPlan EstadoPlan { get;  set; }
        public DateTime FechaVencimiento { get;  set; }
        public DateTime FechaSiguienteControl { get;  set; }
        public virtual ICollection<Evaluacion> Evaluaciones { get; set; } =  new List<Evaluacion>();
        public virtual ICollection<PlanComida> PlanComidas { get; set; } =  new List<PlanComida>();
        
        // public Guid ConsultaInicialId { get;  set; }
        // public virtual ConsultaInicial ConsultaInicial { get; set; }
        
        public Guid PacienteId { get;  set; }
        public virtual Paciente Paciente { get; set; }
    }
}