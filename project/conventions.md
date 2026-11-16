# Code & Testing Conventions

## Code style

- Follow `.editorconfig`. The husky `pre-commit` hook runs `dotnet format` on the whole solution (excluding Migrations); if formatting fails, fix and commit again — never use `--no-verify`.
- No unnecessary comments, no dead code, no placeholder files like `UnitTest1.cs`.

## Domain language

The domain is in **Spanish** and must stay consistent: entities (`Paciente`, `PlanAlimenticio`, `ConsultaInicial`), methods (`Crear`, `Aprobar`). Controllers and HTTP contracts may keep the existing English mix — don't rename public API surfaces unless asked.

## Commands

Run from `GestionClinicaNutricionalService/` (use `workdir`, not `cd`):

| Action | Command |
|---|---|
| Build | `dotnet build GestionClinicaNutricionalService.sln` |
| Unit tests | `dotnet test Tests/Tests.csproj` |
| Tests + coverage | `dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings` |
| Integration tests | `dotnet test IntegrationTests/IntegrationTests.csproj` (needs local SQL Server Express `localhost\SQLEXPRESS`) |
| Format | `dotnet format GestionClinicaNutricionalService.sln --exclude GestionClinicaNutricional.Infrastructure/Migrations/ --no-restore` |

After any C# change: build + unit tests. A task is not done if tests fail.

## Testing rules

1. **One framework per project.** `Tests/` uses NUnit + FakeItEasy; `IntegrationTests/` uses xUnit. Never mix them.
2. **Unit tests** target Application handlers and WebApi controllers with fakes (FakeItEasy). No database, no HTTP.
3. **Integration tests** use the existing factory as `IClassFixture<WebApplicationFactory>`; do not create new factories or touch `ConfigureWebHost` from a test.
4. **Naming**: `Accion_Condicion` (e.g. `CreatePaciente_WithValidRequest`), one test per case.
5. **One test = one scenario**: each test creates its own `client` and shares no state.
6. **Real routes**: hit the routes the controllers actually expose; extract repeated paths to constants.
7. **DTOs, not `dynamic`**: deserialize with `ReadFromJsonAsync<T>()` using DTOs declared in `IntegrationTests/Setup/`.
8. **End-to-end assertions**: request → `EnsureSuccessStatusCode()` → deserialize → assert `IsSuccess`/`IsFailure` → verify persistence via the repository in a fresh scope (`factory.Services.CreateScope()`).
9. **Unique test data**: generate CIs/names with `Guid.NewGuid()`; no fixed values that can collide.
10. Coverage: `coverlet.runsettings` excludes `Infrastructure/Migrations`; generate the HTML report with ReportGenerator (see the solution README).
