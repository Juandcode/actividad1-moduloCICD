Diagrama actualizado:
![Diagrama de arquitectura](Untitled Diagram2.drawio.png)

## Patrones Implementados
- **DDD & Clean Architecture**: Separación clara en capas de Dominio, Aplicación e Infraestructura.
- **CQRS**: Uso de MediatR para separar comandos (escritura) de queries (lectura).
- **Transactional Outbox**: Garantiza la entrega de eventos hacia RabbitMQ.
    - Los eventos de dominio se persisten en la tabla `OutboxMessages` dentro de la misma transacción de negocio.
    - Un `WorkerService` actúa como procesador de fondo: monitorea la tabla de Outbox, recupera los mensajes no procesados y los publica en el exchange de RabbitMQ, asegurando la consistencia eventual.
- **CPM (Central Package Management)**: Gestión de versiones de NuGet centralizada en `Directory.Packages.props`.

## Endpoints de la API
...
...

### MS1 - Clínica

| # | Cliente | Método | Endpoint | Descripción |
|:-:|---|:-:|---|---|
| 1 | Paciente | `POST` | `/api/v1/consultation` | Crea una solicitud de atención inicial |
| 2 | Recepcionista | `PATCH` | `/api/v1/consultation/{patientId}/approve` | Aprueba la consulta y cambia su estado |
| 3 | Nutricionista | `POST` | `/api/v1/patient/{patientId}/plan` | Asigna un plan alimenticio al paciente |
| 4 | Nutricionista | `PATCH` | `/api/v1/patient/{planAlimenticioId}/schedulePlan` | Agenda la próxima cita de evaluación |
| 5 | Nutricionista | `POST` | `/api/v1/patient/{planAlimenticioId}/CreateControlEvaluation` | Agrega una evaluación de control al plan |
| 6 | Nutricionista | `GET` | `/api/v1/patient/{planAlimenticioId}/controlEvaluationHistory` | Devuelve el historial de evaluaciones |
| 7 | Nutricionista | `GET` | `/api/v1/consultations/{patientId}/consultations` | Devuelve el historial de consultas del paciente |
| 8 | Paciente | `GET` | `/api/v1/patient/{id}/evolutions` | El paciente visualiza la evolución de sus mediciones |

### MS2 - Catálogo

Consumido internamente por MS1 (no expuesto al front-end):

| # | Método | Endpoint | Descripción |
|:-:|:-:|---|---|
| 3.1 | `GET` | `/api/v1/catalog/plans/{id}` | MS1 valida que el Plan ID exista antes de asignarlo |

## Tests

El proyecto `Tests` contiene tests unitarios (NUnit + FakeItEasy) sobre los handlers de `GestionClinicaNutricional.Application` y sobre los controllers de `GestionClinicaNutricionalService.WebApi`.

### Ejecutar los tests

```bash
dotnet test Tests/Tests.csproj
```

### Ejecutar los tests con cobertura de código

Se usa `coverlet.collector` a través de `dotnet test`. El archivo `coverlet.runsettings` (en la raíz del repo) excluye del cálculo de cobertura la carpeta `GestionClinicaNutricional.Infrastructure/Migrations` (código autogenerado por EF Core, sin valor de testear).

```bash
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

Esto genera `Tests/TestResults/<guid>/coverage.cobertura.xml`.

### Generar el reporte HTML

```bash
# Una sola vez, si no está instalado:
dotnet tool install -g dotnet-reportgenerator-globaltool

reportgenerator -reports:"Tests/TestResults/*/coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:Html
```

Luego abrir `coveragereport/index.html` en el navegador.

### Automatización

- **Claude Code:** este flujo completo (correr los tests unitarios, verificar los tests de integración, generar el reporte de cobertura y resumir el resultado) está empaquetado en el skill [`test-coverage`](.claude/skills/test-coverage), invocable con `/test-coverage` dentro de una sesión de Claude Code.
- **CI (GitHub Actions):** [`.github/workflows/tests.yml`](.github/workflows/tests.yml) corre este mismo flujo en cada push y pull request a `main`:
  - Restaura dependencias y corre los tests en `Release` con `--collect:"XPlat Code Coverage"` y `coverlet.runsettings`.
  - Genera el reporte de cobertura (HTML, Markdown, badges) con ReportGenerator y publica el resumen en `GITHUB_STEP_SUMMARY` (visible directamente en la pestaña *Actions* del run).
  - Sube dos artifacts descargables: `test-results` (.trx) y `coverage-report` (HTML), con 15 días de retención.
  - Los steps de reporte corren con `if: always()`, así que el reporte se genera incluso si algún test falla.

### Tests de integración

El proyecto `IntegrationTests` contiene tests de integración (xUnit + `Microsoft.AspNetCore.Mvc.Testing`). Levantan la app real vía `IntegrationTests.Setup.WebApplicationFactory` (`WebApplicationFactory<Program>`), pegan a los endpoints HTTP de los controllers y verifican el resultado contra la base de datos real.

#### Ejecutar los tests de integración

Requieren una instancia local de SQL Server Express (`localhost\SQLEXPRESS`), hoy la misma que usa el entorno de desarrollo (`Database=ClinicaNutricional`).

```bash
dotnet test IntegrationTests/IntegrationTests.csproj
```

#### Reglas

1. **Un framework por proyecto.** xUnit es solo para `IntegrationTests/`; `Tests/` usa NUnit + FakeItEasy. No mezclarlos.
2. **Usar la factory existente.** Todos los tests usan `IClassFixture<WebApplicationFactory>`; no crear factories nuevas ni tocar `ConfigureWebHost` desde un test.
3. **Un test = un escenario.** Cada test crea su propio `client` con `factory.CreateClient()` y no comparte estado con otros tests.
4. **Rutas reales.** Pegar a las rutas que exponen los controllers (ej. `api/v1/Patient/CreatePaciente`), no inventar rutas; si se repiten, extraerlas a una constante.
5. **Deserializar con DTO.** Usar `response.Content.ReadFromJsonAsync<T>()` con un DTO declarado en `IntegrationTests/Setup/` (ej. `PacienteCreateResponse`), nunca `dynamic` ni `JsonDocument`.
6. **Flujo end-to-end.** `client.PostAsJsonAsync(...)` → `response.EnsureSuccessStatusCode()` → deserializar → asertar `IsSuccess`/`IsFailure` → resolver el repositorio en un scope propio (`factory.Services.CreateScope()`) y verificar la persistencia con `GetByIdAsync`.
7. **Datos de prueba únicos.** Generar datos únicos (ej. CI con `Guid.NewGuid()`) para evitar colisiones; no usar valores fijos como `CI = "343243"`.
8. **Sin placeholders.** No dejar archivos `UnitTest1.cs` vacíos.
9. **Nombres.** Mantener el patrón `Accion_Condicion` (ej. `CreatePaciente_WithValidRequest`), un test por caso.

#### Deuda técnica

- `IntegrationTests.Setup.WebApplicationFactory` tiene el mismo nombre que `Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory`: conviene renombrarlo (ej. `IntegrationTestFactory`).
- La factory apunta a la misma BD que desarrollo (`ClinicaNutricional`) y no limpia nada: el campo `_dbName` ya existe pero no se usa, y `DisposeAsync` es un no-op. Debería usar una base única por corrida y dropearla (o limpiar tablas) al terminar.
- El CI (`.github/workflows/tests.yml`) solo corre `Tests/`; `IntegrationTests` no está en el pipeline.

### Estado actual de la cobertura

![Resumen de cobertura de código](docs/coverage-summary.png)

- **Line coverage:** 48.6% (216/444)
- **Method coverage:** 61.9% (91/147)

#### Clases cubiertas (100%)

**Application — Handlers**
- `Paciente.ApproveConsultaHandler`
- `Paciente.CreateConsultaHandler`
- `Paciente.CreatePacienteHandler`
- `Paciente.GetConsultasHandler`
- `PlanAlimenticio.CreateEvaluacionHandler`
- `PlanAlimenticio.CreatePlanAlimenticioHandler`
- `PlanAlimenticio.GetEvaluacionesHandler`
- `PlanAlimenticio.ProximaEvaluacionHandler`

**WebApi — Controllers**
- `AtencionController`
- `ConsultationController`
- `PacienteController`
- `PatientController`

**Application — Commands** (records, cubiertos vía uso en los tests de arriba)
- `CreateConsultaCommand`, `GetConsultasCommand`, `CreateEvaluacionCommand`, `CreatePlanAlimenticioCommand`, `ProximaEvaluacionCommand`

#### Clases parcialmente cubiertas

- `ApproveConsultaCommand` (50%), `CreatePacienteCommand` (75%), `GetEvaluacionesCommand` (50%) — falta ejercitar algunos miembros generados por el record (`with`, `Equals`, etc.).
- Entidades de dominio (`Antecedente`, `ConsultaInicial`, `Evaluacion`, `HabitoAlimenticio`, `Paciente`, `PlanAlimenticio`) — cubiertas indirectamente por los tests de handlers/controllers, pero no todas sus propiedades/ramas se ejercitan.

#### Clases sin cubrir (pendientes)

- `Application.Consultas.CreateConsultaInicialCommand` — no tiene handler propio todavía.
- `Application.DependencyInjection` / `Infrastructure.DependencyInjection` — registro de servicios, se prueba mejor con un test de integración de arranque.
- `Infrastructure.Repositories.ConsultaInicialRepository`, `PacienteRepository`, `PlanAlimenticioRepository`, `Infrastructure.UnitOfWork` — requieren tests de integración contra una base de datos (in-memory o SQL real), no unitarios.
- `Domain.PlanComida`, `Domain.TipoComida` — records simples, sin lógica propia.
- `GestionClinicaNutricionalService.ExcludeHabitoAlimenticioPropertySchemaProcessor` — procesador de esquema de NSwag, se prueba mejor generando el swagger.json.
- `Program` (WebApi) — bootstrap de la app, requiere un test de integración (`WebApplicationFactory`).
- `Infrastructure.Migrations.*` — excluido intencionalmente del reporte (código autogenerado por EF Core).
- `Infrastructure.DatabaseContext` — excluido intencionalmente con `[ExcludeFromCodeCoverage]` (requiere una base de datos real).