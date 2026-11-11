using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests.Setup
{
    public static class ApiScenarios
    {
        public static async Task<Guid> CreatePacienteAsync(HttpClient client)
        {
            var response = await client.PostAsJsonAsync(
                "api/v1/Patient/CreatePaciente",
                new
                {
                    CI = Guid.NewGuid().ToString("N"),
                    Nombre = "Diego",
                    Apellido = "Cabrera"
                });

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>();
            return body!.Value;
        }

        public static async Task<Guid> CreatePlanAsync(HttpClient client, Guid pacienteId)
        {
            var plan = new
            {
                nombre = "Plan de prueba",
                descripcion = "Plan de prueba",
                duracionPlan = 15,
                fechaVencimiento = new DateTime(2026, 9, 15, 20, 21, 29),
                estadoPlan = 0,
                planComidas = new[]
                {
                    new
                    {
                        nombre = "Pollo",
                        descripcion = "Comida",
                        categoria = 0
                    }
                }
            };

            var response = await client.PostAsJsonAsync($"api/v1/Patient/{pacienteId}/Plan", plan);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>();
            return body!.Value;
        }

        public static async Task<Guid> CreateConsultaAsync(HttpClient client, Guid pacienteId)
        {
            var consulta = new
            {
                peso = 70,
                altura = 1.75,
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

            var response = await client.PostAsJsonAsync(
                $"api/v1/Consultation/{pacienteId}/Consultation",
                consulta);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<ResultResponse<Guid>>();
            return body!.Value;
        }
    }
}
