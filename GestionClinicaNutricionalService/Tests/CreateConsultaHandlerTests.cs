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
    public class CreateConsultaHandlerTests
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
        public async Task CreateConsulta_ShouldCreateConsultaSuccessfully()
        {
            // Arrange
            var habitoAlimenticios = new List<HabitoAlimenticio>
            {
                new HabitoAlimenticio
                {
                    Nombre = "Desayuno",
                    Descripcion = "Desayuno balanceado",
                    Categoria = CategoriaComida.Omnivora
                }
            };

            var command = new CreateConsultaCommand
            {
                Peso = 70,
                Altura = 1.75,
                Composicion = "Composicion de prueba",
                Antecedentes = new List<Antecedente>
                {
                    new Antecedente("Antecedente de prueba", Problema.SobrePeso)
                },
                HabitoAlimenticios = habitoAlimenticios,
                PacienteId = Guid.NewGuid()
            };

            var handler = new CreateConsultaHandler(consultaInicialRepository, unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => consultaInicialRepository.AddAsync(
                    A<ConsultaInicial>.That.Matches(c =>
                        c.Peso == command.Peso &&
                        c.Altura == command.Altura &&
                        c.Composicion == command.Composicion &&
                        c.PacienteId == command.PacienteId)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => consultaInicialRepository.AddHabitoAlimenticiosAsync(
                    A<ConsultaInicial>._,
                    command.HabitoAlimenticios))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();

            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
