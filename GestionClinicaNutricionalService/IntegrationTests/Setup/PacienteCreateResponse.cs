using System;

namespace IntegrationTests.Setup
{
    public class PacienteCreateResponse
    {
        public Guid Value { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
    }
}