using System.Text.Json.Serialization;
using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    public record CreateConsultaCommand : IRequest<Result<Guid>>
    {
        public int Peso { get; init; }
        public double Altura { get; init; }
        public string Composicion { get; init; }
        public List<Antecedente> Antecedentes { get; init; }
        public List<HabitoAlimenticio> HabitoAlimenticios { get; init; }
        public Guid PacienteId { get; init; }
    }
}