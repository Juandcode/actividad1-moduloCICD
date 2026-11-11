using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricionalService.Contracts;
using GestionClinicaNutricionalService.WebApi.Controllers;
using Joseco.DDD.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PacienteControllerTests
    {
        private IMediator mediator;

        [SetUp]
        public void Setup()
        {
            mediator = A.Fake<IMediator>();
        }

        [Test]
        public async Task CreatePaciente_ShouldReturnOkWithCreatedPacienteId()
        {
            // Arrange
            var command = new CreatePacienteCommand { CI = "12345678", Nombre = "Juan", Apellido = "Perez" };
            Result<Guid> mediatorResult = Guid.NewGuid();

            A.CallTo(() => mediator.Send(command, A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PacienteController(mediator);

            // Act
            var actionResult = await controller.CreatePaciente(command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task CreateConsulta_ShouldSendCommandWithPacienteIdFromRoute()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var command = new CreateConsultaRequest
            {
                Peso = 70,
                Altura = 1.75,
                Composicion = "Composicion de prueba",
                Antecedentes = new List<AntecedenteRequest>(),
                HabitoAlimenticios = new List<HabitoAlimenticioRequest>()
            };
            Result<Guid> mediatorResult = Guid.NewGuid();

            A.CallTo(() => mediator.Send(
                    A<CreateConsultaCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PacienteController(mediator);

            // Act
            var actionResult = await controller.CreateConsulta(pacienteId, command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task CreateEvaluacion_ShouldSendCommandWithPlanAlimenticioIdFromRoute()
        {
            // Arrange
            var planAlimenticioId = Guid.NewGuid();
            var command = new CreateEvaluacionCommand { Descripcion = "Evaluacion de control" };
            Result<Guid> mediatorResult = Guid.NewGuid();

            A.CallTo(() => mediator.Send(
                    A<CreateEvaluacionCommand>.That.Matches(c => c.PlanAlimenticioId == planAlimenticioId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PacienteController(mediator);

            // Act
            var actionResult = await controller.CreateEvaluacion(planAlimenticioId, command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task Evaluaciones_ShouldReturnOkWithEvaluacionesForPlan()
        {
            // Arrange
            var planAlimenticioId = Guid.NewGuid();
            var evaluaciones = new List<Evaluacion> { A.Dummy<Evaluacion>() };
            Result<List<Evaluacion>> mediatorResult = evaluaciones;

            A.CallTo(() => mediator.Send(
                    A<GetEvaluacionesCommand>.That.Matches(c => c.PlanAlimenticioId == planAlimenticioId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PacienteController(mediator);

            // Act
            var actionResult = await controller.Evaluaciones(planAlimenticioId);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }
    }
}
