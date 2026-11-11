using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.Paciente;
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
    public class ConsultationControllerTests
    {
        private IMediator mediator;

        [SetUp]
        public void Setup()
        {
            mediator = A.Fake<IMediator>();
        }

        [Test]
        public async Task Consultation_ShouldSendCommandWithPacienteIdFromRoute()
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

            var controller = new ConsultationController(mediator);

            // Act
            var actionResult = await controller.Consultation(pacienteId, command);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));

            A.CallTo(() => mediator.Send(
                    A<CreateConsultaCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Consultations_ShouldReturnOkWithConsultasForPaciente()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var consultas = new List<ConsultaInicial> { A.Dummy<ConsultaInicial>() };
            Result<List<ConsultaInicial>> mediatorResult = consultas;

            A.CallTo(() => mediator.Send(
                    A<GetConsultasCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new ConsultationController(mediator);

            // Act
            var actionResult = await controller.Consultations(pacienteId);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }

        [Test]
        public async Task Approve_ShouldSendApproveConsultaCommandWithConsultaIdFromRoute()
        {
            // Arrange
            var consultaId = Guid.NewGuid();
            Result<Guid> mediatorResult = consultaId;

            A.CallTo(() => mediator.Send(
                    A<ApproveConsultaCommand>.That.Matches(c => c.ConsultaId == consultaId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new ConsultationController(mediator);

            // Act
            var actionResult = await controller.Approve(consultaId);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));
        }
    }
}
