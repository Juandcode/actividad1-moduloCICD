using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class GetPacienteByIdHandler(IPacienteRepository pacienteRepository)
        : IRequestHandler<GetPacienteByIdQuery, Result<Domain.Paciente>>
    {
        public async Task<Result<Domain.Paciente>> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken)
        {
            var paciente = await pacienteRepository.GetByIdAsync(request.Id, true);
            // if (paciente == null)
            // {
            //     return Result<Domain.Paciente>.Failure(new Error("404","Paciente no encontrado.", ErrorType.NotFound));
            // }
            return paciente;
        }
    }
}
