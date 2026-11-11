using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Joseco.DDD.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GestionClinicaNutricional.Domain
{
    public class Evaluacion : Entity
    {
        [Key] public Guid Id { get; private set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        [JsonIgnore] public Guid PlanAlimenticioId { get; set; }
        [JsonIgnore] public virtual PlanAlimenticio PlanAlimenticio { get; set; }
    }
}