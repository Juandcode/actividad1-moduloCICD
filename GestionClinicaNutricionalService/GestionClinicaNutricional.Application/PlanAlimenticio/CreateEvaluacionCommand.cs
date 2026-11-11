using System.Text.Json.Serialization;
using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    public record CreateEvaluacionCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid PlanAlimenticioId { get; init; }
        public string Descripcion { get; init; }
    }
}