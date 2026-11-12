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
	public class PacienteControllerTest(WebApplicationFactory factory)
		: IClassFixture<WebApplicationFactory>
	{
		[Fact]
		public async Task CreatePaciente_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			HttpResponseMessage response = await client.PostAsJsonAsync(
				"api/v1/Patient/CreatePaciente",
				new { CI = "343243", Nombre = "Diego", Apellido = "Cabrera" });

			_ = response.EnsureSuccessStatusCode();

			PacienteCreateResponse body = (await response.Content.ReadFromJsonAsync<PacienteCreateResponse>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);

			using IServiceScope scope = factory.Services.CreateScope();
			IPacienteRepository repository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
			Paciente paciente = (await repository.GetByIdAsync(body.Value))!;
			Assert.NotNull(paciente);
			Assert.Equal("Diego", paciente.Nombre);
			Assert.Equal("Cabrera", paciente.Apellido);
		}

		[Fact]
		public async Task CreatePlan_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			HttpResponseMessage response = await client.PostAsJsonAsync(
				"api/v1/Patient/CreatePaciente",
				new { CI = "343243", Nombre = "Diego", Apellido = "Cabrera" });

			_ = response.EnsureSuccessStatusCode();

			PacienteCreateResponse body = (await response.Content.ReadFromJsonAsync<PacienteCreateResponse>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);

			using IServiceScope scope = factory.Services.CreateScope();
			IPacienteRepository repository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
			Paciente paciente = (await repository.GetByIdAsync(body.Value))!;
			Assert.NotNull(paciente);
			Assert.Equal("Diego", paciente.Nombre);
			Assert.Equal("Cabrera", paciente.Apellido);

			var plan = new
			{
				nombre = "string",
				descripcion = "string",
				duracionPlan = 15,
				fechaVencimiento = DateTime.Parse("2026-09-15T20:21:29.082Z"),
				estadoPlan = 0,
				PlanComidas = new[]
				{
					new
					{
						nombre = "Pollo",
						descripcion = "Comida",
						categoria = 0
					}
				}
			};

			HttpResponseMessage responsePlan = await client.PostAsJsonAsync($"api/v1/Patient/{paciente.Id}/Plan", plan);

			_ = responsePlan.EnsureSuccessStatusCode();

			PacienteCreateResponse bodyPlan = (await responsePlan.Content.ReadFromJsonAsync<PacienteCreateResponse>())!;
			IPlanAlimenticioRepository repositoryPlan = scope.ServiceProvider.GetRequiredService<IPlanAlimenticioRepository>();
			PlanAlimenticio planAlimenticio = (await repositoryPlan.GetByIdAsync(bodyPlan.Value))!;

			Assert.NotNull(planAlimenticio);
			Assert.Equal(plan.descripcion, planAlimenticio.Descripcion);
			Assert.Equal(plan.nombre, planAlimenticio.Nombre);
		}

		[Fact]
		public async Task SchedulePlan_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);
			Guid planId = await ApiScenarios.CreatePlanAsync(client, pacienteId);

			var proximaFecha = new DateTime(2026, 10, 1, 8, 30, 0);
			HttpResponseMessage response = await client.PatchAsJsonAsync(
				$"api/v1/Patient/{planId}/SchedulePlan",
				new { proximaFecha });

			_ = response.EnsureSuccessStatusCode();

			ResultResponse<Guid> body = (await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);
			Assert.Equal(planId, body.Value);

			using IServiceScope scope = factory.Services.CreateScope();
			IPlanAlimenticioRepository repository = scope.ServiceProvider.GetRequiredService<IPlanAlimenticioRepository>();
			PlanAlimenticio plan = (await repository.GetByIdAsync(planId))!;
			Assert.Equal(proximaFecha, plan.FechaSiguienteControl);
		}

		[Fact]
		public async Task CreateControlEvaluacion_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);
			Guid planId = await ApiScenarios.CreatePlanAsync(client, pacienteId);

			var descripcion = $"Evaluacion {Guid.NewGuid():N}";
			HttpResponseMessage response = await client.PostAsJsonAsync(
				$"api/v1/Patient/{planId}/CreateControlEvaluacion",
				new { descripcion });

			_ = response.EnsureSuccessStatusCode();

			ResultResponse<Guid> body = (await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.False(body.IsFailure);

			using IServiceScope scope = factory.Services.CreateScope();
			IPlanAlimenticioRepository repository = scope.ServiceProvider.GetRequiredService<IPlanAlimenticioRepository>();
			List<Evaluacion> evaluaciones = await repository.GetEvaluaciones(planId);
			Evaluacion evaluacion = Assert.Single(evaluaciones, e => e.Id == body.Value);
			Assert.Equal(descripcion, evaluacion.Descripcion);
		}

		[Fact]
		public async Task ControlEvaluationHistory_WithValidRequest()
		{
			HttpClient client = factory.CreateClient();
			Guid pacienteId = await ApiScenarios.CreatePacienteAsync(client);
			Guid planId = await ApiScenarios.CreatePlanAsync(client, pacienteId);

			var descripcion = $"Evaluacion {Guid.NewGuid():N}";
			HttpResponseMessage createResponse = await client.PostAsJsonAsync(
				$"api/v1/Patient/{planId}/CreateControlEvaluacion",
				new { descripcion });
			_ = createResponse.EnsureSuccessStatusCode();

			HttpResponseMessage response = await client.GetAsync($"api/v1/Patient/{planId}/ControlEvaluationHistory");
			_ = response.EnsureSuccessStatusCode();

			ResultResponse<List<Evaluacion>> body = (await response.Content.ReadFromJsonAsync<ResultResponse<List<Evaluacion>>>())!;
			Assert.NotNull(body);
			Assert.True(body.IsSuccess);
			Assert.Contains(body.Value, e => e.Descripcion == descripcion);
		}
	}
}
