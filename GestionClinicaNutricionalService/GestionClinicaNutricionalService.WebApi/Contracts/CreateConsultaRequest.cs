using System.ComponentModel.DataAnnotations;
using GestionClinicaNutricional.Domain;

namespace GestionClinicaNutricionalService.Contracts
{
    public record CreateConsultaRequest
    {
        [Required] public int Peso { get; init; }
        [Required] public double Altura { get; init; }
        [Required] public string Composicion { get; init; }
        [Required] public IReadOnlyList<AntecedenteRequest> Antecedentes { get; init; }
        [Required] public IReadOnlyList<HabitoAlimenticioRequest> HabitoAlimenticios { get; init; }
    }

    public record AntecedenteRequest(
        [Required] string Descripcion,
        [Required] Problema Problema);

    public record HabitoAlimenticioRequest(
        [Required] string Nombre,
        [Required] string Descripcion,
        [Required] CategoriaComida Categoria);
}