using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    internal class GetEvaluacionesHandler(IPlanAlimenticioRepository planAlimenticioRepository): IRequestHandler<GetEvaluacionesCommand, Result<List<Evaluacion>>>
    {
        public async Task<Result<List<Evaluacion>>> Handle(GetEvaluacionesCommand request, CancellationToken cancellationToken)
        {
            return await planAlimenticioRepository.GetEvaluaciones(request.PlanAlimenticioId);
        }
    }
}