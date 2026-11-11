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
    public class AtencionControllerTests
    {
        private IMediator mediator;

        [SetUp]
        public void Setup()
        {
            mediator = A.Fake<IMediator>();
        }

        [Test]
        public async Task Consultas_ShouldReturnOkWithConsultasForPaciente()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var consultas = new List<ConsultaInicial> { A.Dummy<ConsultaInicial>() };
            Result<List<ConsultaInicial>> mediatorResult = consultas;

            A.CallTo(() => mediator.Send(
                    A<GetConsultasCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .Returns(mediatorResult);

            var controller = new AtencionController(mediator);

            // Act
            var actionResult = await controller.Consultas(pacienteId);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mediatorResult));

            A.CallTo(() => mediator.Send(
                    A<GetConsultasCommand>.That.Matches(c => c.PacienteId == pacienteId),
                    A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}
