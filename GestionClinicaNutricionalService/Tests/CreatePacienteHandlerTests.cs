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
    public class CreatePacienteHandlerTests
    {
        private IPacienteRepository pacienteRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            pacienteRepository = A.Fake<IPacienteRepository>();
            unitOfWork = A.Fake<IUnitOfWork>();
        }

        [Test]
        public async Task CreatePaciente_ShouldCreatePacienteSuccessfully()
        {
            // Arrange
            var command = new CreatePacienteCommand
            {
                CI = "12345678",
                Nombre = "Juan",
                Apellido = "Perez"
            };

            var handler = new CreatePacienteHandler(pacienteRepository, unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => pacienteRepository.AddAsync(
                    A<Paciente>.That.Matches(p =>
                        p.CI == command.CI &&
                        p.Nombre == command.Nombre &&
                        p.Apellido == command.Apellido)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();

            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
