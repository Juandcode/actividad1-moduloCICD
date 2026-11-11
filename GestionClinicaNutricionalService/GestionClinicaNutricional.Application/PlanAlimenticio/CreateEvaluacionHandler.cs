using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    public class CreateEvaluacionHandler(
        IPlanAlimenticioRepository planAlimenticioRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateEvaluacionCommand, Result<Guid>>
    
    {
        public async Task<Result<Guid>> Handle(CreateEvaluacionCommand request, CancellationToken cancellationToken)
        {
            Domain.PlanAlimenticio? planAlimenticion = await planAlimenticioRepository.GetByIdAsync(request.PlanAlimenticioId);
            var evaluacion = new Evaluacion()
            {
                Descripcion = request.Descripcion,
                Fecha = DateTime.Now,
            };
            await planAlimenticioRepository.AddEvaluacion(planAlimenticion!, evaluacion);
            await unitOfWork.CommitAsync(cancellationToken);
            return evaluacion.Id;
        }
    }
}