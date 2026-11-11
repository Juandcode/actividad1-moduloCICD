---
name: create-integration-tests
description: Crea tests de integración nuevos en el proyecto GestionClinicaNutricionalService siguiendo los patrones ya existentes en IntegrationTests/ (xUnit + WebApplicationFactory contra SQL Server real). Úsalo cuando el usuario pida escribir/crear/agregar un test de integración para un controller o un endpoint nuevo. Para correr/verificar los tests de integración usá el skill integration-tests.
---

# Crear tests de integración

Este skill guía la creación de tests de integración nuevos en `IntegrationTests/`, imitando los tests ya existentes (`PacienteControllerTest`, `ConsultationControllerTest`, `AtencionControllerTest`). Para correr o verificar los tests usá el skill `integration-tests`.

## 1. Contexto del proyecto

- El proyecto `IntegrationTests` es xUnit + `Microsoft.AspNetCore.Mvc.Testing` (net10.0) y referencia el proyecto `WebApi`: `IntegrationTests/IntegrationTests.csproj`.
- Levanta la app real (`IntegrationTests.Setup.WebApplicationFactory`) contra una SQL Server local en la base `ClinicaNutricional`. No se moca nada: los requests pegan en la API real y se verifican contra los repositorios reales.
- `AssemblyInfo.cs` desactiva la paralelización de tests (`CollectionBehavior(DisableTestParallelization = true)`) porque comparten la base de datos.

## 2. Convenciones de los tests existentes

Un archivo de test nuevo vive en `IntegrationTests/Controllers/<Controller>Test.cs` y sigue este molde (`ConsultationControllerTest.cs` es el ejemplo más simple):

```csharp
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Controllers
{
    public class XControllerTest(WebApplicationFactory factory)
        : IClassFixture<WebApplicationFactory>
    {
        [Fact]
        public async Task Metodo_WithValidRequest()
        {
            var client = factory.CreateClient();

            using IServiceScope scope = factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IXRepository>();
            var resultado = // ... crear/consultar y verificar contra el repo
        }
    }
}
```

Reglas que siguen los tests actuales:

- **Clase**: constructor primario que recibe `WebApplicationFactory factory`, implementa `IClassFixture<WebApplicationFactory>`. Una instancia de `factory` por clase.
- **Nombre de tests**: `NombreDelMetodo_WithValidRequest` (o `_WithValidRequest` como sufijo del escenario happy path).
- **Cliente**: `factory.CreateClient()`.
- **Respuesta HTTP**: `response.EnsureSuccessStatusCode()` y deserializar el body.
- **DTOs de respuesta**: en la carpeta `Setup` hay `ResultResponse<T>` (con `Value`, `IsSuccess`, `IsFailure`) y `PacienteCreateResponse`. Deserializá con `ReadFromJsonAsync`. No inventes un DTO nuevo salvo que la respuesta del endpoint no matchee estos.
- **Verificación de persistencia**: abrí un `IServiceScope` sobre `factory.Services` y pedí el repositorio del dominio (`IPacienteRepository`, `IPlanAlimenticioRepository`, `IConsultaInicialRepository`, etc.), luego consultá la entidad creada y afirmá sobre sus propiedades. No te quedes solo con el `IsSuccess` del response.
- **Datos únicos**: los datos que se crean usan `Guid.NewGuid()` (por ejemplo `CI = Guid.NewGuid().ToString("N")` o descripciones con Guid) para no chocar entre corridas. **Nunca** usen datos fijos/repetidos.
- **Namespaces**: el archivo usa `IntegrationTests.Setup` para la factory y los helpers.

## 3. Helpers compartidos: `Setup/ApiScenarios.cs`

Para setup reutilizá los helpers de `ApiScenarios` en vez de repetir el POST de creación:

- `await ApiScenarios.CreatePacienteAsync(client)` → `Guid` del paciente creado.
- `await ApiScenarios.CreatePlanAsync(client, pacienteId)` → `Guid` del plan del paciente.
- `await ApiScenarios.CreateConsultaAsync(client, pacienteId)` → `Guid` de la consulta del paciente.

Si el endpoint nuevo necesita crear otra entidad de contexto (por ejemplo un plan, una consulta), agregá un helper nuevo acá con el mismo estilo.

## 4. Cómo armar un test para un endpoint nuevo

1. Abrí el controller en `GestionClinicaNutricionalService.WebApi/Controllers/` y leé la ruta (`Route`) y los atributos HTTP (Get/Post/Patch/Delete). Las rutas se construyen con `nameof(Endpoint)` y `[Route("[controller]")]` o `[Route("api/v1/[controller]")]`, así que la URL efectiva derivá de ahí.
2. Mirá el `Command`/`Request` que recibe el endpoint en `GestionClinicaNutricional.Application/` para armar el body JSON correcto (propiedades, tipos).
3. Escribí el test happy path siguiendo el molde de la sección 2.
4. Si la verificación busca una colección (p. ej. `ControlEvaluationHistory`), deserializá con `ResultResponse<List<T>>` y afirmá con `Assert.Contains(...)`.
5. Para endpoints que no van por `api/v1/...` (como `Atencion/{pacienteId}/Consultas`), usá la ruta exacta del controller.

## 5. Recordatorios

- No cambies `WebApplicationFactory.cs` ni la config de la DB de un test para otro.
- No descomentes los bloques comentados en los controllers (`PacienteController`, `PatientController`, `AtencionController`) para "probar más" — los tests cubren lo publicado, no lo comentado.
- Después de crear, corri los tests con el skill `integration-tests` y reportá al usuario si algún test falla con el nombre y el error.