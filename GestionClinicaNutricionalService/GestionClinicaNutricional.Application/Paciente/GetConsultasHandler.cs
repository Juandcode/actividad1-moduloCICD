using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class GetConsultasHandler(IConsultaInicialRepository consultaInicialRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<GetConsultasCommand, Result<List<ConsultaInicial>>>
    {
        public async Task<Result<List<ConsultaInicial>>> Handle(GetConsultasCommand request, CancellationToken cancellationToken)
        {
            var consultas = await consultaInicialRepository.AllConsultasAsync(request.PacienteId);
            return consultas;
        }
    }
}