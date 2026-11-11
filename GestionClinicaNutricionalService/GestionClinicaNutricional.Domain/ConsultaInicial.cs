using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain
{
    public class ConsultaInicial : AggregateRoot
    {
        [Key] public Guid Id { get; set; }
        public int Peso { get; set; }
        public double Altura { get; set; }
        public string Composicion { get; set; }
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public List<Antecedente> Antecedentes { get; set; } = new List<Antecedente>();
        //public IReadOnlyCollection<Antecedente> Antecedentes => _antecedentes.AsReadOnly();

        public List<HabitoAlimenticio> HabitoAlimenticios { get; set; }
        //public IReadOnlyCollection<HabitoAlimenticio> HabitoAlimenticios => _habitoAlimenticios.AsReadOnly();

        [JsonIgnore] public Guid PacienteId { get; set; }
        [JsonIgnore] public Paciente Paciente { get; set; }
        //public virtual Paciente Paciente { get; set; }

        //[JsonIgnore] public virtual ICollection<PlanAlimenticio> PlanAlimenticios { get; set; }
    }
}