using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Consultas
{
    public class CreateConsultaInicialCommand: IRequest<Result<Guid>>
    {
        public int Peso { get; set; }
        public double Altura { get; set; }
        public string Composicion { get; set; }
        public Guid PacienteId { get; set; }
        public List<Antecedente> Antecedentes { get; set; }
        public List<HabitoAlimenticio> HabitoAlimenticios { get; set; }
    }
}