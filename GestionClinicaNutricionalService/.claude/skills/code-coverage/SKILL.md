---
name: code-coverage
description: Corre los tests unitarios (Tests/) y de integración (IntegrationTests/) del proyecto GestionClinicaNutricionalService y genera UN solo reporte de cobertura combinado con ReportGenerator, resumiendo qué clases quedaron cubiertas, parcialmente cubiertas o pendientes. Úsalo cuando el usuario pida el reporte de cobertura de AMBOS proyectos juntos, "cobertura completa", o un reporte que incluya unitarios + integración. Para cobertura solo de unitarios usá test-coverage; para solo crear/verificar tests unitarios o de integración usá create-unit-tests/unit-test-verifier y create-integration-tests/integration-tests.
---

# Code Coverage (unitarios + integración)

Este skill genera el reporte de cobertura **combinando ambos proyectos** de tests del repo:

- `Tests` — tests unitarios (NUnit + FakeItEasy, sin BD): `dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings`.
- `IntegrationTests` — tests de integración (xUnit + `WebApplicationFactory`, contra SQL Server real): `dotnet test IntegrationTests/IntegrationTests.csproj --collect:"XPlat Code Coverage"`.

El objetivo es un **único** `coveragereport/` que mergee los dos `coverage.cobertura.xml` con `reportgenerator` y un solo `Summary.txt` con los ensamblados de ambos.

## 1. Prerrequisitos

- ReportGenerator instalado (una sola vez):

  ```bash
  dotnet tool install -g dotnet-reportgenerator-globaltool
  ```

- Para los tests de integración: SQL Server Express local (`localhost\SQLEXPRESS`, servicio `MSSQL$SQLEXPRESS`, `Database=ClinicaNutricional`). Si no está disponible, avisá que no se pudo correr la parte de integración y caé de nuevo en el flujo solo-unit del skill `test-coverage`.

## 2. Correr ambos proyectos con cobertura

Desde la raíz del repo:

```bash
# Unitarios (el runsettings excluye Infrastructure.Migrations.*)
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings

# Integración (requiere SQL Server local; MISMO runsettings para excluir las migraciones)
dotnet test IntegrationTests/IntegrationTests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

- `coverlet.runsettings` excluye `GestionClinicaNutricional.Infrastructure.Migrations.*` del cálculo — usalo en **ambos** proyectos, no solo en los unitarios, para que las migraciones (que se ejercitan en integración) no aparezcan en el reporte combinado.
- Si un proyecto falla por compilación, arreglalo antes de seguir — no interpretes cobertura de un build roto.
- Si algún test falla, reportá el nombre y el error antes de continuar con el reporte.
- Los resultados quedan en `Tests/TestResults/<guid>/coverage.cobertura.xml` y `IntegrationTests/TestResults/<guid>/coverage.cobertura.xml`. Para un reporte limpio, borrá `Tests/TestResults`, `IntegrationTests/TestResults` y `coveragereport` antes de correr.

## 3. Merge con ReportGenerator

```bash
rm -rf coveragereport
reportgenerator "-reports:Tests/TestResults/*/coverage.cobertura.xml;IntegrationTests/TestResults/*/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html;TextSummary"
```

- Usá `*/coverage.cobertura.xml` (una sola estrella) para cada proyecto, nunca `**`. Si además corrés con `--logger trx` junto a `--collect`, VSTest genera carpetas extra tipo `TestResults/<usuario>_<máquina>_<fecha>/In/<máquina>/coverage.cobertura.xml` con reportes parciales; `**` los recoge y contamina el merge (aparece "MultiReportParser" en vez de "CoberturaParser"). Con `*/coverage.cobertura.xml` solo se toman las carpetas con GUID de coverlet.
- Esto produce `coveragereport/index.html` y `coveragereport/Summary.txt`.

## 4. Resumir el resultado

Leé `coveragereport/Summary.txt` y reportá:

- Resultado de ambos proyectos (pasados/fallados por proyecto).
- Line coverage y method coverage globales del merge.
- Clases al 100% (cubiertas), indicando a qué ensamblado pertenecen.
- Clases entre 1-99% (parcialmente cubiertas) con su porcentaje.
- Clases en 0% (pendientes), agrupadas por ensamblado (Application / Domain / Infrastructure / WebApi).

Puntos clave del merge:

- `Infrastructure` (repositorios, EF) hoy solo se ejercita desde los tests de integración: en el merge combinado suele pasar de "pendiente" a "parcialmente cubierta". Es LA ganancia de combinar los dos proyectos.
- No trates una clase como "cubierta" solo porque aparece en el reporte — confirmá que su % sea 100.
- Advertí al usuario que estos números **no son comparables** con la sección de cobertura del README, que hoy está calculada solo con los tests unitarios.

## 5. (Opcional) Actualizar screenshot y README

Solo si el usuario lo pide explícitamente ("actualiza el screenshot/README con la cobertura combinada"):

1. Screenshot con Chrome headless (ajustá el `.exe` si es Edge):

   ```bash
   "/c/Program Files/Google/Chrome/Application/chrome.exe" --headless --disable-gpu \
     --screenshot="$PWD/docs/coverage-summary.png" --window-size=900,560 --hide-scrollbars \
     --force-device-scale-factor=1 "file:///<ruta-absoluta-con-%20-por-espacios>/coveragereport/index.html"
   ```

2. En `README.md` → `## Tests` → `### Estado actual de la cobertura`, actualizá porcentajes y listas de clases según el nuevo `Summary.txt`, y aclará que incluyen cobertura de integración.

## 6. Recordatorios

- `Tests/TestResults/`, `IntegrationTests/TestResults/` y `coveragereport/` están en `.gitignore` — no los agregues a git salvo pedido explícito.
- El pipeline de CI (`.github/workflows/tests.yml`) corre solo los tests unitarios (sin cobertura combinada). Si cambiás el flujo local, mantené el workflow en sync y no asumas que CI genera este reporte.
- Para flujos parciales: cobertura solo-unit → skill `test-coverage`; crear/verificar unit → `create-unit-tests` / `unit-test-verifier`; crear/verificar integración → `create-integration-tests` / `integration-tests`.