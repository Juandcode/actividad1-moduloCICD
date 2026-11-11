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
    public class PatientControllerTests
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

            var controller = new PatientController(mediator);

            // Act
            var actionResult = await controller.CreatePaciente(command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task Plan_ShouldSendCommandWithPacienteIdFromRoute()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var command = new CreatePlanAlimenticioRequest
            {
                Nombre = "Plan",
                Descripcion = "Descripcion",
                DuracionPlan = DuracionPlan.Mes,
                EstadoPlan = EstadoPlan.Vigente,
                FechaVencimiento = DateTime.Now.AddDays(30),
                PlanComidas = new List<PlanComidaRequest>()
            };
            Result<Guid> mediatorResult = Guid.NewGuid();

            A.CallTo(() => mediator.Send(
                    A<CreatePlanAlimenticioCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PatientController(mediator);

            // Act
            var actionResult = await controller.Plan(pacienteId, command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));

            A.CallTo(() => mediator.Send(
                    A<CreatePlanAlimenticioCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task SchedulePlan_ShouldSendCommandWithPlanAlimenticioIdFromRoute()
        {
            // Arrange
            var planAlimenticioId = Guid.NewGuid();
            var command = new ProximaEvaluacionCommand { ProximaFecha = DateTime.Now.AddDays(15) };
            Result<Guid> mediatorResult = planAlimenticioId;

            A.CallTo(() => mediator.Send(
                    A<ProximaEvaluacionCommand>.That.Matches(c => c.PlanAlimenticioId == planAlimenticioId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PatientController(mediator);

            // Act
            var actionResult = await controller.SchedulePlan(planAlimenticioId, command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task ControlEvaluationHistory_ShouldReturnOkWithEvaluacionesForPlan()
        {
            // Arrange
            var planAlimenticioId = Guid.NewGuid();
            var evaluaciones = new List<Evaluacion> { A.Dummy<Evaluacion>() };
            Result<List<Evaluacion>> mediatorResult = evaluaciones;

            A.CallTo(() => mediator.Send(
                    A<GetEvaluacionesCommand>.That.Matches(c => c.PlanAlimenticioId == planAlimenticioId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PatientController(mediator);

            // Act
            var actionResult = await controller.ControlEvaluationHistory(planAlimenticioId);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task CreateControlEvaluacion_ShouldSendCommandWithPlanAlimenticioIdFromRoute()
        {
            // Arrange
            var planAlimenticioId = Guid.NewGuid();
            var command = new CreateEvaluacionCommand { Descripcion = "Evaluacion de control" };
            Result<Guid> mediatorResult = Guid.NewGuid();

            A.CallTo(() => mediator.Send(
                    A<CreateEvaluacionCommand>.That.Matches(c => c.PlanAlimenticioId == planAlimenticioId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new PatientController(mediator);

            // Act
            var actionResult = await controller.CreateControlEvaluacion(planAlimenticioId, command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }
    }
}
