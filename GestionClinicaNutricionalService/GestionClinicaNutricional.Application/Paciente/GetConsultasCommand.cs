using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class GetConsultasCommand : IRequest<Result<List<ConsultaInicial>>>
    {
        public Guid PacienteId { get; set; }
    }
}