using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    internal class ProximaEvaluacionHandler(IPlanAlimenticioRepository planAlimenticioRepository, IUnitOfWork unitOfWork): IRequestHandler<ProximaEvaluacionCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ProximaEvaluacionCommand request, CancellationToken cancellationToken)
        {
            await planAlimenticioRepository.ProgramarProximoControl(request.PlanAlimenticioId, request.ProximaFecha);
            await unitOfWork.CommitAsync(cancellationToken);
            return request.PlanAlimenticioId;
        }
    }
}