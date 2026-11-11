using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    internal class CreateConsultaHandler(IConsultaInicialRepository consultaInicialRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateConsultaCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateConsultaCommand request, CancellationToken cancellationToken)
        {
            Domain.ConsultaInicial consulta = new Domain.ConsultaInicial()
            {
                Altura = request.Altura,
                Antecedentes = request.Antecedentes,
                Composicion = request.Composicion,
                Peso = request.Peso,
                HabitoAlimenticios = request.HabitoAlimenticios,
                PacienteId = request.PacienteId,
            };
            await consultaInicialRepository.AddAsync(consulta);
            await consultaInicialRepository.AddHabitoAlimenticiosAsync(consulta, request.HabitoAlimenticios);
            await unitOfWork.CommitAsync(cancellationToken);
            return consulta.Id;
        }
    }
}