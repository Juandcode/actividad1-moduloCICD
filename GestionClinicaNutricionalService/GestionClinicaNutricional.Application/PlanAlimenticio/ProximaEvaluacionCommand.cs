using System.Text.Json.Serialization;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    public record ProximaEvaluacionCommand: IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid PlanAlimenticioId { get; init; }
        public DateTime ProximaFecha { get; init; }
    }
}