using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class GetPacienteByIdQuery : IRequest<Result<Domain.Paciente>>
    {
        public Guid Id { get; set; }
    }
}
