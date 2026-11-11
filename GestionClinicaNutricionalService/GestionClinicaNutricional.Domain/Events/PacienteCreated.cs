using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Events
{
    public record PacienteCreated : DomainEvent
    {
        public Guid PacienteId { get; }
        public string CI { get; }
        public string Nombre { get; }
        public string Apellido { get; }

        /// <remarks>
        /// This is the ONLY constructor on purpose. The outbox persists this event through a
        /// Newtonsoft value converter with <c>TypeNameHandling.All</c>, and Newtonsoft prefers a
        /// public parameterless constructor over a parameterized one when both exist. Because every
        /// payload property above is get-only, that constructor produced an event whose payload was
        /// never written — <c>PacienteId</c> came back <c>Guid.Empty</c>, with no error raised. With a single fully parameterized constructor
        /// Newtonsoft matches the JSON members to the parameters by name and rebuilds the event
        /// correctly. Do NOT add a parameterless constructor back: it reintroduces the silent
        /// truncation.
        /// </remarks>
        public PacienteCreated(Guid pacienteId, string ci, string nombre, string apellido)
        {
            OccuredOn = DateTime.UtcNow;
            PacienteId = pacienteId;
            CI = ci;
            Nombre = nombre;
            Apellido = apellido;
        }
    }
}