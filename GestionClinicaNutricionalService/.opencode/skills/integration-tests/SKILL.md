---
name: integration-tests
description: Corre y verifica los tests de integración del proyecto GestionClinicaNutricionalService (IntegrationTests/ con xUnit + WebApplicationFactory contra una base de datos SQL Server real). Úsalo cuando el usuario pida correr o verificar los tests de integración, o incluir su cobertura en el reporte. Para escribir/crear tests de integración nuevos usá el skill create-integration-tests.
---

# Integration Tests

Este skill cubre los tests de integración de este repo. El proyecto `IntegrationTests` levanta la app real (`IntegrationTests.Setup.WebApplicationFactory`) y pega contra una base de datos SQL Server real.

## 1. Verificar los tests de integración

```bash
dotnet test IntegrationTests/IntegrationTests.csproj
```

- Requiere una instancia local de SQL Server Express (`localhost\SQLEXPRESS`, servicio `MSSQL$SQLEXPRESS`) — la misma que usa el entorno de desarrollo (`Database=ClinicaNutricional`). Si no está disponible:
  - **No** saltees la verificación en silencio ni la marques como exitosa.
  - Avisá al usuario que los tests de integración no se pudieron correr y por qué.
- Tienen que pasar **todos**. Si alguno falla, reportá el nombre del test y el error antes de seguir con el reporte de cobertura.
- Los tests crean datos reales en `ClinicaNutricional`; usan datos únicos (Guid) para no pisarse entre corridas. No agregues datos fijos.

## 2. (Opcional) Incluir la cobertura de integración

Para que el reporte de cobertura también refleje lo cubierto por los tests de integración (por ejemplo los repositorios de `Infrastructure`, que solo se ejercitan acá), corré también este proyecto con `--collect` y pasá los dos reportes a `reportgenerator`:

```bash
dotnet test IntegrationTests/IntegrationTests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings

reportgenerator "-reports:Tests/TestResults/*/coverage.cobertura.xml;IntegrationTests/TestResults/*/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html;TextSummary"
```

- Usá `--settings coverlet.runsettings` también acá, no solo en los unitarios: excluye `GestionClinicaNutricional.Infrastructure.Migrations.*` del cálculo, y las migraciones se ejercitan justamente en integración. Sin ese flag se cuelan en el reporte combinado.
- El flujo completo de cobertura combinada (correr ambos proyectos + merge + resumen) lo cubre el skill `code-coverage`; usalo en lugar de replicar los comandos acá.
- Aclará al usuario que, si hacés esto, los porcentajes del reporte ya no son comparables con los de la sección de cobertura del README (que hoy son solo de los tests unitarios).

## 3. Recordatorios

- Para **crear** tests de integración nuevos (no correrlos), usá el skill `create-integration-tests`, que documenta los patrones de `IntegrationTests/`.
- `IntegrationTests/TestResults/` está en `.gitignore` — son artefactos regenerables, no los agregues a git salvo pedido explícito.
- El pipeline de CI (`.github/workflows/tests.yml`) corre solo los tests unitarios; los tests de integración todavía **no** están en el pipeline (dependen de SQL Server local). Si cambiás el comando local, mantené el workflow en sync.