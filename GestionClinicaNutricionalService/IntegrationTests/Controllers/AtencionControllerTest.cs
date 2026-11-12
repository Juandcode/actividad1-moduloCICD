using System;
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
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);
			Guid consultaId = await ApiScenarios.CreateConsultaAsync(client, pacienteId);

			HttpResponseMessage response = await client.PostAsync($"Atencion/{pacienteId}/Consultas", null);
			_ = response.EnsureSuccessStatusCode();

			ResultResponse<List<ConsultaInicial>> body = (await response.Content.ReadFromJsonAsync<ResultResponse<List<ConsultaInicial>>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);
			Assert.Contains(body.Value, c => c.Id == consultaId);
		}
	}
}
