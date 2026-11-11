---
name: create-unit-tests
description: Crea tests unitarios nuevos en el proyecto GestionClinicaNutricionalService siguiendo los patrones ya existentes en Tests/ (NUnit + FakeItEasy contra mocks, sin base de datos). Úsalo cuando el usuario pida escribir/crear/agregar un test unitario para un controller o un handler. Para correr/verificar los tests unitarios usá el skill unit-test-verifier.
---

# Crear tests unitarios

Este skill guía la creación de tests unitarios nuevos en `Tests/`, imitando los tests ya existentes (`AtencionControllerTests`, `PacienteControllerTests`, `CreatePacienteHandlerTests`, `GetConsultasHandlerTests`, etc.). Para correr o verificar los tests usá el skill `unit-test-verifier`.

## 1. Contexto del proyecto

- El proyecto `Tests` es NUnit + FakeItEasy (net10.0) y referencia `Application`, `Domain` y `WebApi`: `Tests/Tests.csproj`.
- **No** levanta la app ni pega contra una base de datos: los controllers se prueban con un `IMediator` falso y los handlers con repositorios y `IUnitOfWork` falsos vía FakeItEasy.
- Paquetes disponibles: `FakeItEasy` 9.0.1, `NUnit` 3.13.2, `Microsoft.NET.Test.Sdk`, `NUnit3TestAdapter` 4.0.0, `coverlet.collector` (para cobertura opcional).

## 2. Dos tipos de tests y sus moldes

### Controllers (ejemplo: `AtencionControllerTests.cs`)

Para probar un controller, se fakea `IMediator` y se llama al método del controller con los parámetros de la ruta/body; el resultado se verifica como `ActionResult`:

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricionalService.WebApi.Controllers;
using Joseco.DDD.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class XControllerTests
    {
        private IMediator mediator;

        [SetUp]
        public void Setup()
        {
            mediator = A.Fake<IMediator>();
        }

        [Test]
        public async Task Metodo_ShouldReturnOkWithXxx()
        {
            // Arrange
            var id = Guid.NewGuid();
            var items = new List<Entity> { A.Dummy<Entity>() };
            Result<List<Entity>> mediatorResult = items;

            A.CallTo(() => mediator.Send(
                    A<SomeCommand>.That.Matches(c => c.Id == id),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new XController(mediator);

            // Act
            var actionResult = await controller.Metodo(id);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));

            A.CallTo(() => mediator.Send(
                    A<SomeCommand>.That.Matches(c => c.Id == id),
                    A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}
```

### Handlers (ejemplo: `CreatePacienteHandlerTests.cs`)

Para probar un handler de MediatR, se fakean las dependencias del constructor (repositorios, `IUnitOfWork`) y se verifica que el handler llame al repo/UnitOfWork esperado y devuelva el `Result` correcto:

```csharp
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class XHandlerTests
    {
        private IAlgunRepository repositorio;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            repositorio = A.Fake<IAlgunRepository>();
            unitOfWork = A.Fake<IUnitOfWork>();
        }

        [Test]
        public async Task Metodo_ShouldMetodoSuccessfully()
        {
            // Arrange
            var command = new SomeCommand { Id = Guid.NewGuid() };

            var handler = new SomeHandler(repositorio, unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => repositorio.AlgunMetodo(command.Id))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(command.Id));
        }
    }
}
```

## 3. Convenciones que siguen los tests actuales

- **Archivo**: `Tests/<Sujeto>Tests.cs` (ej. `AtencionControllerTests.cs`, `CreatePacienteHandlerTests.cs`).
- **Clase**: `public class XControllerTests` / `XHandlerTests` con `[TestFixture]`, dentro de `namespace Tests`.
- **Nombre de tests**: `NombreDelMetodo_ShouldResultadoEsperado()` (ej. `CreatePaciente_ShouldCreatePacienteSuccessfully`, `Consultas_ShouldReturnOkWithConsultasForPaciente`).
- **Setup**: una clase de fake por dependencia en campo privado, inicializada en `[SetUp]` con `A.Fake<TCualquiera>()`.
- **Estructura**: siempre `// Arrange`, `// Act`, `// Assert`.
- **Mocks**: `A.Fake<T>()` para interfaces, `A.Dummy<T>()` para datos de relleno, `A.CallTo(...)...MustHaveHappenedOnceExactly()` para verificar llamadas.
- **Filtros de argumentos**: `A<T>.That.Matches(x => x.Propiedad == valor)` cuando el handler recibe datos que se completan desde la ruta (controller) o cuando querés verificar que se pasó el comando correcto.
- **Mediador**: `A<CancellationToken>._` como segundo argumento de `mediator.Send(...)`.
- **Result del dominio**: `Result<T>` viene de `Joseco.DDD.Core.Results` (para controllers) y de `Joseco.DDD.Core.Abstractions` (para handlers + `IUnitOfWork`). Implícitamente convertible desde `T` (`Result<List<Entity>> mediatorResult = items;`) o desde `Guid`.
- **Idempotente**: no dependas de datos compartidos ni de `static`; usá `Guid.NewGuid()` para ids.
- **Verificación fuerte**: para controllers no te quedes solo con `Is.Not.Null`/`Is.EqualTo` del `OkObjectResult` — añadí el `MustHaveHappenedOnceExactly()` del `mediator.Send` cuando el test lo amerite (como en `PatientControllerTests`/`AtencionControllerTests`).

## 4. Cómo armar un test para un controller o handler nuevo

1. Abrí el controller en `GestionClinicaNutricionalService.WebApi/Controllers/` para ver los métodos públicos y qué comandos envía (mirá si construye el comando con `request with { Id = ... }` desde la ruta, o si le pasa el body tal cual).
2. Si el `Send` completa un id desde la ruta (patrón `command with { PacienteId = id }`), usá `A<T>.That.Matches(c => c.Propiedad == id)` para el arreglo del fake.
3. Abrí el handler correspondiente en `GestionClinicaNutricional.Application/<Carpeta>/` para ver su constructor y qué métodos llama de cada repositorio; fakeá exactamente esos métodos en el `A.CallTo`.
4. Escribí el test siguiendo el molde de la sección 2 (controller o handler).
5. Si el handler devuelve un `Result` con `.Value`, verificá `Assert.That(result.Value, Is.EqualTo(...))`.
6. Después de crear, corré los tests con el skill `unit-test-verifier` y reportá si alguno falla con el nombre y el error.

## 5. Recordatorios

- No escribas tests que dependan de una base de datos real ni de servicios externos: estos son unitarios, todo se mockea.
- No cambies `Tests.csproj` (proyecto compartido) para un test individual.
- `UnitTest1.cs` es el archivo plantilla de ejemplo que dejó la creación del proyecto; si el usuario no lo necesita, no lo modifiques ni lo borres sin pedirlo.
- Mantené el estilo de `Assert.That(...)` de NUnit (no `Assert.AreEqual`) ya que es el que usa el repo.
- Después de crear, verificá con `unit-test-verifier` que **todos** los tests pasen, no solo el nuevo.