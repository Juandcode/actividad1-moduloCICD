using System.Text.Json.Serialization;
using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    public record CreatePlanAlimenticioCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore] public Guid PacienteId { get; init; }
        public string Nombre { get; init; }
        public string Descripcion { get; init; }
        public DuracionPlan DuracionPlan { get; init; }
        public DateTime FechaVencimiento { get; init; }
        public EstadoPlan EstadoPlan { get; init; }
        public ICollection<PlanComida> PlanComidas { get; init; }
    }
}