using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.PlanAlimenticio
{
    internal class CreatePlanAlimenticioHandler(
        IPlanAlimenticioRepository planAlimenticioRepository,
        IConsultaInicialRepository consultaInicialRepository,
        IPacienteRepository pacienteRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreatePlanAlimenticioCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(
            CreatePlanAlimenticioCommand request,
            CancellationToken cancellationToken)
        {
            var planAlimenticio = new Domain.PlanAlimenticio()
            {
                Descripcion = request.Descripcion,
                Nombre = request.Nombre,
                DuracionPlan = request.DuracionPlan,
                EstadoPlan = request.EstadoPlan,
                FechaVencimiento = request.FechaVencimiento,

                PlanComidas = request.PlanComidas,
            };

            Domain.Paciente paciente = (await pacienteRepository.GetByIdAsync(request.PacienteId))!;
            planAlimenticio.PacienteId = paciente.Id;
            await planAlimenticioRepository.AddAsync(planAlimenticio);
            await unitOfWork.CommitAsync(cancellationToken);
            return planAlimenticio.Id;
        }
    }
}