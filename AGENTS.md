# AGENTS.md — Rules for AI agents in this repository

Mandatory instructions for any code assistant (opencode, Claude Code, Copilot, etc.) working in this repo. Read them before generating or modifying code. The canonical, detailed rules live in the [`project/`](project/) folder:

| Document | Content |
|---|---|
| [`project/architecture.md`](project/architecture.md) | Layers, CQRS, Outbox, CPM |
| [`project/conventions.md`](project/conventions.md) | Code style, domain language, testing rules |
| [`project/git-conventions.md`](project/git-conventions.md) | Branch naming, commit messages, PRs |
| [`project/git-workflow.md`](project/git-workflow.md) | Trunk workflow, feature worktrees, CI |
| [`project/trunk-based-development.md`](project/trunk-based-development.md) | TBD model, branch lifetime, feature flags |

## Repository structure

```
actividad1-modulocicd/                     ← root (Node tooling + husky)
└── GestionClinicaNutricionalService/      ← the entire .NET solution lives here
    ├── GestionClinicaNutricional.Domain/
    ├── GestionClinicaNutricional.Application/
    ├── GestionClinicaNutricional.Infrastructure/
    ├── GestionClinicaNutricional.WorkerService/
    ├── GestionClinicaNutricionalService.WebApi/
    ├── Tests/                             ← unit tests (NUnit + FakeItEasy)
    ├── IntegrationTests/                  ← integration tests (xUnit + WebApplicationFactory)
    ├── Directory.Packages.props           ← centralized NuGet versions (CPM)
    ├── coverlet.runsettings
    ├── docker-compose.yml
    └── .github/workflows/tests.yml
```

- The root `package.json` exists only for **husky** (git hooks). Do not add solution dependencies or scripts there.
- Never modify `GestionClinicaNutricional.Infrastructure/Migrations/` (auto-generated EF Core code) or `bin/`, `obj/`, `publish/`, `coveragereport/` folders.

## Commands

Always run from `GestionClinicaNutricionalService/` (use `workdir`, not `cd`):

| Action | Command |
|---|---|
| Build | `dotnet build GestionClinicaNutricionalService.sln` |
| Unit tests | `dotnet test Tests/Tests.csproj` |
| Tests with coverage | `dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings` |
| Integration tests | `dotnet test IntegrationTests/IntegrationTests.csproj` (requires local SQL Server Express `localhost\SQLEXPRESS`) |
| Format | `dotnet format GestionClinicaNutricionalService.sln --exclude GestionClinicaNutricional.Infrastructure/Migrations/ --no-restore` |

After any C# code change, run build + unit tests. Do not consider a task done if tests fail.

## Architecture (non-negotiable)

- **Clean Architecture / DDD**: dependencies point inward. `Domain` references no other project; `Application` only `Domain`; `Infrastructure` and `WebApi` the previous ones. Never introduce reverse references.
- **CQRS with MediatR**: one handler per use case (`CreatePacienteHandler`, `ApproveConsultaHandler`, …). Commands/queries are records in `Application/<Feature>/`.
- **Repositories**: interfaces live in `Domain/Repositories/`; implementations in `Infrastructure/Repositories/`.
- **Transactional Outbox**: domain events are persisted to `OutboxMessages` within the same transaction; the `WorkerService` publishes them to RabbitMQ. Never publish to RabbitMQ directly from a handler.
- **NuGet packages**: with CPM active, versions are declared ONLY in `Directory.Packages.props`; reference packages in `.csproj` files without `Version=`.

## Code conventions

- Follow `.editorconfig`. The husky `pre-commit` hook runs `dotnet format`; if formatting fails, fix and commit again (do not use `--no-verify`).
- Domain language: **Spanish** (entities `Paciente`, `PlanAlimenticio`, methods `Crear`). Keep that consistency; controllers and contracts may mix English as already existing.
- One test framework per project: **NUnit + FakeItEasy** in `Tests/`, **xUnit** in `IntegrationTests/`. Do not mix them.
- Integration tests: use the existing factory as `IClassFixture`, `Accion_Condicion` naming, unique test data (no fixed CIs), deserialize to DTOs in `IntegrationTests/Setup/`, no `dynamic`.
- No unnecessary comments, no dead code, no placeholder files like `UnitTest1.cs`.

## Git

- **Trunk-Based Development.** `main` is the single, always-releasable trunk. No `develop`/`release/*` branches; releases are tags on `main`.
- **One task = one worktree.** Develop every task in its own git worktree with its own short-lived branch (`feature/<short-name>`, lifetime ≤ 1–2 days), created from `origin/main` and squash-merged back. Never work directly on `main`. Full procedure in [`project/git-workflow.md`](project/git-workflow.md).
- **Incomplete features go behind feature flags** — trunk must stay safe, no half-broken work.
- Never commit secrets, connection strings or `.env`. Secrets go in `appsettings.Development.json` (environment-level `.gitignore`) or environment variables.
- Short, imperative commit messages, in the domain language (Spanish) or plain English, without mixing unrelated changes.
- CI: `.github/workflows/tests.yml` runs build + tests + coverage on every push/PR to `main`. If you touch the solution, the pipeline must stay green; if trunk is red, fixing it has priority.

## Scope

- Do not add new microservices, projects, packages or CI workflows unless explicitly requested.
- Prefer editing existing code over creating new files.
