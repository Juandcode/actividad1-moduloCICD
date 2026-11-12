using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Controllers
{
	public class ConsultationControllerTest(WebApplicationFactory factory)
		: IClassFixture<WebApplicationFactory>
	{
		[Fact]
		public async Task Consultation_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);

			var consulta = new
			{
				peso = 82,
				altura = 1.8,
				composicion = "Composicion de prueba",
				antecedentes = new[]
				{
					new { descripcion = "Antecedente de prueba", problema = 0 }
				},
				habitoAlimenticios = new[]
				{
					new { nombre = "Desayuno", descripcion = "Habito de prueba", categoria = 0 }
				}
			};

			HttpResponseMessage response = await client.PostAsJsonAsync(
				$"api/v1/Consultation/{pacienteId}/Consultation",
				consulta);

			_ = response.EnsureSuccessStatusCode();

			ResultResponse<Guid> body = (await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);

			using IServiceScope scope = factory.Services.CreateScope();
			IConsultaInicialRepository repository = scope.ServiceProvider.GetRequiredService<IConsultaInicialRepository>();
			ConsultaInicial created = (await repository.GetByIdAsync(body.Value))!;
			Assert.NotNull(created);
			Assert.Equal(82, created.Peso);
			Assert.Equal(1.8, created.Altura);
			Assert.Equal("Composicion de prueba", created.Composicion);
			Assert.Equal(pacienteId, created.PacienteId);
		}

		[Fact]
		public async Task Consultations_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);
			Guid consultaId = await ApiScenarios.CreateConsultaAsync(client, pacienteId);

			HttpResponseMessage response = await client.GetAsync($"api/v1/Consultation/{pacienteId}/Consultations");
			_ = response.EnsureSuccessStatusCode();

			ResultResponse<List<ConsultaInicial>> body = (await response.Content.ReadFromJsonAsync<ResultResponse<List<ConsultaInicial>>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);
			Assert.Contains(body.Value, c => c.Id == consultaId);
		}

		[Fact]
		public async Task Approve_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);
			Guid consultaId = await ApiScenarios.CreateConsultaAsync(client, pacienteId);

			HttpResponseMessage response = await client.PostAsync($"api/v1/Consultation/{consultaId}/Approve", null);
			_ = response.EnsureSuccessStatusCode();

			ResultResponse<Guid> body = (await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);
			Assert.Equal(consultaId, body.Value);

			using IServiceScope scope = factory.Services.CreateScope();
			IConsultaInicialRepository repository = scope.ServiceProvider.GetRequiredService<IConsultaInicialRepository>();
			ConsultaInicial approved = (await repository.GetByIdAsync(consultaId))!;
			Assert.True(approved.Estado);
		}
	}
}
