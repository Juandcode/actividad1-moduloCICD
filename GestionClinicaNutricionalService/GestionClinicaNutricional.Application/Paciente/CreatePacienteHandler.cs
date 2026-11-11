using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    internal class CreatePacienteHandler(IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreatePacienteCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreatePacienteCommand request, CancellationToken cancellationToken)
        {
            var paciente = Domain.Paciente.Crear(request.CI, request.Nombre, request.Apellido);
            
            await pacienteRepository.AddAsync(paciente);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return paciente.Id;
        }
    }
}