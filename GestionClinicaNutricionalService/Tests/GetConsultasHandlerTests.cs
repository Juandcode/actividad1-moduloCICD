using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GetConsultasHandlerTests
    {
        private IConsultaInicialRepository consultaInicialRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            consultaInicialRepository = A.Fake<IConsultaInicialRepository>();
            unitOfWork = A.Fake<IUnitOfWork>();
        }

        [Test]
        public async Task GetConsultas_ShouldReturnConsultasForPaciente()
        {
            // Arrange
            var pacienteId = Guid.NewGuid();
            var consultas = new List<ConsultaInicial>
            {
                A.Dummy<ConsultaInicial>(),
                A.Dummy<ConsultaInicial>()
            };

            var command = new GetConsultasCommand { PacienteId = pacienteId };

            A.CallTo(() => consultaInicialRepository.AllConsultasAsync(pacienteId))
                .Returns(consultas);

            var handler = new GetConsultasHandler(consultaInicialRepository, unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => consultaInicialRepository.AllConsultasAsync(pacienteId))
                .MustHaveHappenedOnceExactly();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(consultas));
        }
    }
}
