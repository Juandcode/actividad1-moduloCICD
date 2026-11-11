using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GetEvaluacionesHandlerTests
    {
        private IPlanAlimenticioRepository planAlimenticioRepository;

        [SetUp]
        public void Setup()
        {
            planAlimenticioRepository = A.Fake<IPlanAlimenticioRepository>();
        }

        [Test]
        public async Task GetEvaluaciones_ShouldReturnEvaluacionesForPlanAlimenticio()
        {
            // Arrange
            var planAlimenticioId = Guid.NewGuid();
            var evaluaciones = new List<Evaluacion>
            {
                A.Dummy<Evaluacion>(),
                A.Dummy<Evaluacion>()
            };

            var command = new GetEvaluacionesCommand { PlanAlimenticioId = planAlimenticioId };

            A.CallTo(() => planAlimenticioRepository.GetEvaluaciones(planAlimenticioId))
                .Returns(evaluaciones);

            var handler = new GetEvaluacionesHandler(planAlimenticioRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => planAlimenticioRepository.GetEvaluaciones(planAlimenticioId))
                .MustHaveHappenedOnceExactly();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(evaluaciones));
        }
    }
}
