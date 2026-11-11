---
name: unit-test-verifier
description: Corre y verifica los tests unitarios del proyecto GestionClinicaNutricionalService (Tests/ con NUnit + FakeItEasy, sin base de datos). Úsalo cuando el usuario pida correr o verificar los tests unitarios, o confirmar que todos pasan. Para escribir/crear tests unitarios nuevos usá el skill create-unit-tests.
---

# Unit Test Verifier

Este skill cubre la verificación de los tests unitarios de este repo. El proyecto `Tests` es NUnit + FakeItEasy y no requiere base de datos ni servicios externos.

## 1. Verificar los tests unitarios

```bash
dotnet test Tests/Tests.csproj
```

- No depende de SQL Server: corre contra mocks (FakeItEasy). Si falla por errores de compilación, arreglalos primero y volvé a correr — no reportes como exitoso un build roto.
- Tienen que pasar **todos**. Si alguno falla, reportá el nombre del test y el error (línea del assert o excepción) antes de seguir.
- Verificá también que el proyecto compile correctamente (si `dotnet test` levanta errores de compilación, es señal de que arregló/refactorizó algo que rompió el build).

## 2. (Opcional) Incluir cobertura de los tests unitarios

Solo si el usuario la pide (por ejemplo "verifica los tests y la cobertura", o "actualiza el reporte de cobertura"):

```bash
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

- `coverlet.runsettings` (en la raíz) excluye `GestionClinicaNutricional.Infrastructure.Migrations.*` del cálculo — no lo quites salvo pedido explícito.
- El resultado queda en `Tests/TestResults/<guid>/coverage.cobertura.xml`.
- El merge, el reporte (`reportgenerator`) y el resumen final por clases los cubre el skill `test-coverage`; este comando solo genera el reporte crudo de `Tests` para quien lo consuma.

## 3. (Opcional) Correr también los de integración

Los tests de integración son otro proyecto (`IntegrationTests/`, xUnit + `WebApplicationFactory`, contra SQL Server real) y **no** corren con este skill:

- Para correrlos/verificarlos, usá el skill `integration-tests` (requiere `localhost\SQLEXPRESS`).
- Si el usuario pidió "verificar que todos los tests corren" y menciona los de integración, corré ambos: `dotnet test Tests/Tests.csproj` (este skill) y `dotnet test IntegrationTests/IntegrationTests.csproj` (skill `integration-tests`).

## 4. Resumir el resultado

- Cantidad de tests corridos, pasados y fallados.
- Si alguno falló: nombre del test entre corchetes (ej. `Failed Tests/CreateXHandlerTests.Metodo_ShouldX`) y el mensaje de error.
- No digas "todo pasa" si hubo un test fallido o un build roto.

## 5. Recordatorios

- `Tests/TestResults/` está en `.gitignore` — son artefactos regenerables, no los agregues a git salvo pedido explícito.
- Para **crear** tests unitarios nuevos (no correrlos), usá el skill `create-unit-tests`, que documenta los patrones de `Tests/`.
- El pipeline de CI (`.github/workflows/tests.yml`) corre el flujo de los tests unitarios en cada push/PR a `main`. Si cambiás el comando local de verificación, mantené el workflow en sync.