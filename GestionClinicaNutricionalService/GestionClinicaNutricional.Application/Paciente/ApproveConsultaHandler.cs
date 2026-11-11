using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class ApproveConsultaHandler(IConsultaInicialRepository consultaInicialRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<ApproveConsultaCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ApproveConsultaCommand request, CancellationToken cancellationToken)
        {
            await consultaInicialRepository.ApproveConsulta(request.ConsultaId);
            await unitOfWork.CommitAsync(cancellationToken);
            return request.ConsultaId;
        }
    }
}