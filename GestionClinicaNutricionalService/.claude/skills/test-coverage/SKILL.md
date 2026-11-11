---
name: test-coverage
description: Corre y verifica los tests unitarios del proyecto GestionClinicaNutricionalService (Tests/ con NUnit + FakeItEasy) y genera el reporte de cobertura de los unitarios con ReportGenerator. Úsalo cuando el usuario pida correr los unitarios o ver la cobertura SOLO de unitarios. Para cobertura COMBINADA (unitarios + integración en un mismo reporte) usá el skill code-coverage; para los tests de integración sola usá el skill integration-tests.
---

# Test & Coverage

Este skill reproduce el flujo de testing unitario de este repo:

- `Tests` — tests unitarios (NUnit + FakeItEasy), cobertura con `coverlet.collector` + `ReportGenerator`.

> **¿Cobertura combinada?** Si el usuario quiere la cobertura de unitarios **y** de integración en un único reporte, usá el skill `code-coverage` en su lugar (corre ambos proyectos y mergea con `reportgenerator`).

## 1. Correr los tests unitarios

Desde la raíz del repo:

```bash
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

- `coverlet.runsettings` (en la raíz) excluye `GestionClinicaNutricional.Infrastructure.Migrations.*` del cálculo de cobertura — no lo quites salvo que el usuario lo pida explícitamente.
- Si `dotnet test` falla por errores de compilación, arreglalos antes de seguir — no interpretes cobertura de un build roto.
- El resultado queda en `Tests/TestResults/<guid>/coverage.cobertura.xml`. Si hay corridas viejas y querés un reporte limpio, borrá `Tests/TestResults` antes de correr.

## 2. Generar el reporte de cobertura

```bash
# Si no está instalado (una sola vez):
dotnet tool install -g dotnet-reportgenerator-globaltool

rm -rf coveragereport
reportgenerator "-reports:Tests/TestResults/*/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html;TextSummary"
```

Usá `*/coverage.cobertura.xml` (una sola estrella), no `**`. Si en algún momento corrés `dotnet test` con `--logger trx` a la vez que `--collect`, VSTest genera una carpeta extra tipo `TestResults/<usuario>_<máquina>_<fecha>/In/<máquina>/coverage.cobertura.xml` con un segundo reporte parcial; `**` la recoge y contamina el merge (aparece "MultiReportParser" en vez de "CoberturaParser" en el summary — señal de que se coló ese archivo de más). Con `*/coverage.cobertura.xml` solo se toma la carpeta con GUID que genera coverlet.

Esto produce `coveragereport/index.html` y `coveragereport/Summary.txt`.

## 3. Resumir el resultado

Leé `coveragereport/Summary.txt` y reportá al usuario, en texto (no hace falta abrir el HTML salvo que se pida):

- Resultado de los tests unitarios (pasados/fallados).
- Line coverage y method coverage globales.
- Clases al 100% (cubiertas).
- Clases entre 1-99% (parcialmente cubiertas) con su porcentaje.
- Clases en 0% (pendientes), agrupadas por ensamblado (Application / Domain / Infrastructure / WebApi).

No trates un handler o controller como "cubierto" solo porque el archivo aparece en el reporte — confirmá que su % sea 100 antes de decir que está cubierto.

## 4. (Opcional) Actualizar el screenshot y la sección de cobertura del README

Solo si el usuario lo pide explícitamente (por ejemplo "actualiza el screenshot de cobertura" o "actualiza el README con la cobertura actual"):

1. Tomar el screenshot del summary con Chrome headless (ajustá la ruta del `.exe` de Chrome/Edge si difiere):

   ```bash
   "/c/Program Files/Google/Chrome/Application/chrome.exe" --headless --disable-gpu \
     --screenshot="$PWD/docs/coverage-summary.png" --window-size=900,560 --hide-scrollbars \
     --force-device-scale-factor=1 "file:///<ruta-absoluta-con-%20-por-espacios>/coveragereport/index.html"
   ```

   La ruta del HTML debe ser absoluta y con `%20` en vez de espacios. El screenshot final vive en `docs/coverage-summary.png` (referenciado desde `README.md`).

2. Actualizar en `README.md`, dentro de la sección `## Tests` → `### Estado actual de la cobertura`:
   - Los porcentajes de line/method coverage.
   - Las listas de clases cubiertas / parciales / pendientes, según el nuevo `Summary.txt`.

## 5. Recordatorios

- `Tests/TestResults/` y `coveragereport/` están en `.gitignore` — son artefactos regenerables, no los agregues a git salvo pedido explícito.
- Si se agregan handlers o controllers nuevos sin test, avisá cuáles quedaron sin cubrir en vez de asumir que están cubiertos.
- El pipeline de CI (`.github/workflows/tests.yml`) corre el flujo de los tests unitarios en cada push/PR a `main`. Si cambiás el comando local, mantené el workflow en sync.
