using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class ApproveConsultaCommand: IRequest<Result<Guid>>
    {
        public Guid ConsultaId { get; init; }
    }
}