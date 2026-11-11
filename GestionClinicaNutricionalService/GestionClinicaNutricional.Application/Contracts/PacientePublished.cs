using Joseco.Communication.External.Contracts.Message;

namespace GestionClinicaNutricional.Application.Contracts
{
    public record PacientePublished : IntegrationMessage
    {
        public Guid PacienteId { get; set; }
        public string CI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public PacientePublished(string correlationId, string source) 
            : base(correlationId, source)
        {
        }
    }
}