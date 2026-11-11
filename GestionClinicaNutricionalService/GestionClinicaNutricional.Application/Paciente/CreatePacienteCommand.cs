using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public class CreatePacienteCommand: IRequest<Result<Guid>>
    {
        public string CI { get; init; }
        public string Nombre { get; init; }
        public string Apellido { get; init; }
    }
}