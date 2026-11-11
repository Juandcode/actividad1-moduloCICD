using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Joseco.DDD.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GestionClinicaNutricional.Domain
{
    public class PlanComida: Entity
    {
        [JsonIgnore]
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public CategoriaComida Categoria { get; init; }
        
        [JsonIgnore]
        public Guid PlanAlimenticioId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual PlanAlimenticio PlanAlimenticio { get; set; }
    }
}