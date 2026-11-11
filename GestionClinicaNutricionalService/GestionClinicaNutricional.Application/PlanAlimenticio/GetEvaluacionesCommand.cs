using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    public class GetEvaluacionesCommand: IRequest<Result<List<Evaluacion>>>
    {
        public Guid PlanAlimenticioId { get; set; }
    }
}