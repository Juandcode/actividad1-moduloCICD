using System.ComponentModel.DataAnnotations;
using GestionClinicaNutricional.Domain;

namespace GestionClinicaNutricionalService.Contracts
{
    public record CreatePlanAlimenticioRequest
    {
        [Required] public string Nombre { get; init; }
        [Required]public string Descripcion { get; init; }
        [Required]public DuracionPlan DuracionPlan { get; init; }
        [Required]public DateTime FechaVencimiento { get; init; }
        [Required]public EstadoPlan EstadoPlan { get; init; }
        [Required]public ICollection<PlanComidaRequest> PlanComidas { get; init; }
    }

    public record PlanComidaRequest
    {
        [Required]public string Nombre { get; set; }
        [Required]public string Descripcion { get; set; }
        [Required]public CategoriaComida Categoria { get; init; }
    }
}