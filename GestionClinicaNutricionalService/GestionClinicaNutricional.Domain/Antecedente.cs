namespace GestionClinicaNutricional.Domain
{
    public record Antecedente
    {
        public string Descripcion { get; init; }
        public Problema Problema { get; init; }
        
        public Antecedente(string descripcion, Problema problema)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Descripción requerida");

            if (!Enum.IsDefined(typeof(Problema), problema))
                throw new ArgumentException("Problema inválido");

            Descripcion = descripcion;
            Problema = problema;
        }
    }
}