using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Joseco.DDD.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GestionClinicaNutricional.Domain
{
    public class HabitoAlimenticio: Entity
    {
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        
        public CategoriaComida Categoria { get; init; }
        //public TipoComida TipoComida { get; set; }
        
        [JsonIgnore]
        [ValidateNever]
        public ConsultaInicial ConsultaInicial { get; set; }
        [JsonIgnore]
        public Guid ConsultaInicialId { get; set; }
    }
}