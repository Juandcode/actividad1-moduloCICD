using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GestionClinicaNutricional.Domain;
using IntegrationTests.Setup;
using Xunit;

namespace IntegrationTests.Controllers
{
    public class AtencionControllerTest(WebApplicationFactory factory)
        : IClassFixture<WebApplicationFactory>
    {
        [Fact]
        public async Task Consultas_WithValidRequest()
        {
            var client = factory.CreateClient();
            var pacienteId = await ApiScenarios.CreatePacienteAsync(client);
            var consultaId = await ApiScenarios.CreateConsultaAsync(client, pacienteId);

            var response = await client.PostAsync($"Atencion/{pacienteId}/Consultas", null);
            response.EnsureSuccessStatusCode();

            var body = (await response.Content.ReadFromJsonAsync<ResultResponse<List<ConsultaInicial>>>())!;
            Assert.NotNull(body);
            Assert.True(body.IsSuccess);
            Assert.False(body.IsFailure);
            Assert.Contains(body.Value, c => c.Id == consultaId);
        }
    }
}
