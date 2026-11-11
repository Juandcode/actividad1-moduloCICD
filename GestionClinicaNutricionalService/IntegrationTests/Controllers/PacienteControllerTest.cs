using System;
using System.Collections.Generic;
using System.Linq;
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
            var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync(
                "api/v1/Patient/CreatePaciente",
                new { CI = "343243", Nombre = "Diego", Apellido = "Cabrera" });

            response.EnsureSuccessStatusCode();

            var body = (await response.Content.ReadFromJsonAsync<PacienteCreateResponse>())!;
            Assert.NotNull(body);
            Assert.True(body.IsSuccess);
            Assert.False(body.IsFailure);

            using IServiceScope scope = factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
            var paciente = (await repository.GetByIdAsync(body.Value))!;
            Assert.NotNull(paciente);
            Assert.Equal("Diego", paciente.Nombre);
            Assert.Equal("Cabrera", paciente.Apellido);
        }

        [Fact]
        public async Task CreatePlan_WithValidRequest()
        {
            var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync(
                "api/v1/Patient/CreatePaciente",
                new { CI = "343243", Nombre = "Diego", Apellido = "Cabrera" });

            response.EnsureSuccessStatusCode();

            var body = (await response.Content.ReadFromJsonAsync<PacienteCreateResponse>())!;
            Assert.NotNull(body);
            Assert.True(body.IsSuccess);
            Assert.False(body.IsFailure);

            using IServiceScope scope = factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
            var paciente = (await repository.GetByIdAsync(body.Value))!;
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

            var responsePlan = await client.PostAsJsonAsync($"api/v1/Patient/{paciente.Id}/Plan", plan);
            
            responsePlan.EnsureSuccessStatusCode();

            var bodyPlan = (await responsePlan.Content.ReadFromJsonAsync<PacienteCreateResponse>())!;
            var repositoryPlan = scope.ServiceProvider.GetRequiredService<IPlanAlimenticioRepository>();
            var planAlimenticio = (await repositoryPlan.GetByIdAsync(bodyPlan.Value))!;
            
            Assert.NotNull(planAlimenticio);
            Assert.Equal(plan.descripcion, planAlimenticio.Descripcion);
            Assert.Equal(plan.nombre, planAlimenticio.Nombre);
        }

        [Fact]
        public async Task SchedulePlan_WithValidRequest()
        {
            var client = factory.CreateClient();
            var pacienteId = await ApiScenarios.CreatePacienteAsync(client);
            var planId = await ApiScenarios.CreatePlanAsync(client, pacienteId);

            var proximaFecha = new DateTime(2026, 10, 1, 8, 30, 0);
            var response = await client.PatchAsJsonAsync(
                $"api/v1/Patient/{planId}/SchedulePlan",
                new { proximaFecha });

            response.EnsureSuccessStatusCode();

            var body = (await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>())!;
            Assert.NotNull(body);
            Assert.True(body.IsSuccess);
            Assert.False(body.IsFailure);
            Assert.Equal(planId, body.Value);

            using IServiceScope scope = factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPlanAlimenticioRepository>();
            var plan = (await repository.GetByIdAsync(planId))!;
            Assert.Equal(proximaFecha, plan.FechaSiguienteControl);
        }

        [Fact]
        public async Task CreateControlEvaluacion_WithValidRequest()
        {
            var client = factory.CreateClient();
            var pacienteId = await ApiScenarios.CreatePacienteAsync(client);
            var planId = await ApiScenarios.CreatePlanAsync(client, pacienteId);

            var descripcion = $"Evaluacion {Guid.NewGuid():N}";
            var response = await client.PostAsJsonAsync(
                $"api/v1/Patient/{planId}/CreateControlEvaluacion",
                new { descripcion });

            response.EnsureSuccessStatusCode();

            var body = (await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>())!;
            Assert.NotNull(body);
            Assert.True(body.IsSuccess);
            Assert.False(body.IsFailure);

            using IServiceScope scope = factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPlanAlimenticioRepository>();
            var evaluaciones = await repository.GetEvaluaciones(planId);
            var evaluacion = Assert.Single(evaluaciones, e => e.Id == body.Value);
            Assert.Equal(descripcion, evaluacion.Descripcion);
        }

        [Fact]
        public async Task ControlEvaluationHistory_WithValidRequest()
        {
            var client = factory.CreateClient();
            var pacienteId = await ApiScenarios.CreatePacienteAsync(client);
            var planId = await ApiScenarios.CreatePlanAsync(client, pacienteId);

            var descripcion = $"Evaluacion {Guid.NewGuid():N}";
            var createResponse = await client.PostAsJsonAsync(
                $"api/v1/Patient/{planId}/CreateControlEvaluacion",
                new { descripcion });
            createResponse.EnsureSuccessStatusCode();

            var response = await client.GetAsync($"api/v1/Patient/{planId}/ControlEvaluationHistory");
            response.EnsureSuccessStatusCode();

            var body = (await response.Content.ReadFromJsonAsync<ResultResponse<List<Evaluacion>>>())!;
            Assert.NotNull(body);
            Assert.True(body.IsSuccess);
            Assert.Contains(body.Value, e => e.Descripcion == descripcion);
        }
    }
}