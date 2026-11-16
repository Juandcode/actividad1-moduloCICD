# Gestión Clínica Nutricional — Actividad 1 (Módulo CI/CD)

Microservicio para la gestión clínica nutricional de pacientes: consultas iniciales, planes alimenticios y evaluaciones de control. Construido con .NET (Clean Architecture + DDD + CQRS), con eventos de integración hacia RabbitMQ vía Transactional Outbox y CI con GitHub Actions.

> La documentación detallada de la API, tests y cobertura vive en [`GestionClinicaNutricionalService/README.md`](GestionClinicaNutricionalService/README.md).

## Stack

- **.NET 10** (ASP.NET Core WebApi, Worker Service)
- **Entity Framework Core** (SQL Server)
- **MediatR** (CQRS) · **Joseco DDD / Outbox** (DDD + Transactional Outbox)
- **RabbitMQ** (publicación de eventos de integración)
- **NUnit + FakeItEasy** (tests unitarios) · **xUnit + WebApplicationFactory** (tests de integración)
- **GitHub Actions** (build + tests + cobertura) · **husky** (`dotnet format` en `pre-commit`)
- **Docker** (`docker-compose.yml` para levantar la API)

## Estructura del repo

```
actividad1-modulocicd/
├── GestionClinicaNutricionalService/        # Solución .NET (todo el código)
│   ├── GestionClinicaNutricional.Domain/            # Entidades, eventos de dominio, interfaces de repositorios
│   ├── GestionClinicaNutricional.Application/       # Casos de uso (CQRS con MediatR)
│   ├── GestionClinicaNutricional.Infrastructure/    # EF Core, repositorios, outbox
│   ├── GestionClinicaNutricional.WorkerService/     # Procesador de outbox → RabbitMQ
│   ├── GestionClinicaNutricionalService.WebApi/     # Controllers y contratos HTTP
│   ├── Tests/                                       # Tests unitarios (NUnit + FakeItEasy)
│   ├── IntegrationTests/                            # Tests de integración (xUnit)
│   ├── Directory.Packages.props                     # Versiones NuGet centralizadas (CPM)
│   └── .github/workflows/tests.yml                  # CI
├── .husky/pre-commit                        # dotnet format antes de cada commit
└── package.json                             # Solo tooling Node (husky)
```

## Prerrequisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Node.js (solo para husky)
- SQL Server Express local (`localhost\SQLEXPRESS`) para desarrollo e integración
- (Opcional) RabbitMQ si se quiere probar el flujo de eventos end-to-end

## Primeros pasos

```bash
# 1. Clonar
git clone https://github.com/Juandcode/actividad1-moduloCICD.git
cd actividad1-moduloCICD

# 2. Instalar hooks de git (husky)
npm install

# 3. Restaurar la solución
dotnet restore GestionClinicaNutricionalService/GestionClinicaNutricionalService.sln
```

La cadena de conexión se configura en `GestionClinicaNutricionalService/GestionClinicaNutricionalService.WebApi/appsettings.Development.json` (no commitear credenciales; puede sobrescribirse por variables de entorno).

## Correr la API

```bash
dotnet run --project GestionClinicaNutricionalService/GestionClinicaNutricionalService.WebApi
```

Swagger: `http://localhost:<puerto>/swagger`.

Alternativa con Docker (imagen publicada en Docker Hub):

```bash
cd GestionClinicaNutricionalService
docker compose up
# API en http://localhost:3001
```

## Comandos útiles (desde `GestionClinicaNutricionalService/`)

| Acción | Comando |
|---|---|
| Build | `dotnet build GestionClinicaNutricionalService.sln` |
| Tests unitarios | `dotnet test Tests/Tests.csproj` |
| Tests + cobertura | `dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings` |
| Tests de integración | `dotnet test IntegrationTests/IntegrationTests.csproj` |
| Formato | `dotnet format GestionClinicaNutricionalService.sln --exclude GestionClinicaNutricional.Infrastructure/Migrations/ --no-restore` |

Reporte HTML de cobertura:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"Tests/TestResults/*/coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:Html
# abrir coveragereport/index.html
```

## CI/CD

- **`.github/workflows/tests.yml`**: en cada push/PR a `main`, corre los tests unitarios en `Release` con cobertura, genera el reporte con ReportGenerator, publica el resumen en el job y sube los artifacts (`test-results`, `coverage-report`).
- **husky `pre-commit`**: ejecuta `dotnet format` sobre la solución (excluyendo Migrations). Si el formato falla, el commit se detiene.

## Convenciones

- Arquitectura en capas con dependencias hacia adentro: `Domain` ← `Application` ← `Infrastructure/WebApi`. Las reglas completas del proyecto viven en la carpeta [`project/`](project/): arquitectura, convenciones y flujo de git.
- **Trunk-Based Development**: `main` es el único trunk (siempre publicable). Las ramas son de vida corta (≤ 1–2 días) y se integran con squash merge; nada de `develop` ni `release/*`; lo incompleto va detrás de feature flags. Ver [`project/trunk-based-development.md`](project/trunk-based-development.md).
- **Una tarea = un worktree**: cada tarea/feature se desarrolla en su propio git worktree con su propia rama (`feature/<nombre>`); nunca se trabaja directo en `main`. Ver [`project/git-workflow.md`](project/git-workflow.md).
- Lenguaje del dominio en español (`Paciente`, `ConsultaInicial`, `PlanAlimenticio`).
- Commits cortos e imperativos. Nunca commitear secretos ni cadenas de conexión.
- Versiones de paquetes NuGet centralizadas en `Directory.Packages.props`.

## Documentación adicional

- [Reglas del proyecto (arquitectura, convenciones, git)](project/README.md)
- [API, endpoints y tests](GestionClinicaNutricionalService/README.md)
- [Reglas para agentes de IA](AGENTS.md)
