using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class EvaluacionTests
	{
		private IPlanAlimenticioRepository planAlimenticioRepository;
		private IUnitOfWork unitOfWork;
		[SetUp]
		public void SetUp()
		{
			// Arrange: Se ejecuta antes de cada método de prueba
			planAlimenticioRepository = A.Fake<IPlanAlimenticioRepository>();
			unitOfWork = A.Fake<IUnitOfWork>();
		}

		[Test]
		public async Task Evaluacion_ShouldCreateAnEvaluacionSuccessfully()
		{
			PlanAlimenticio planAlimenticio = A.Dummy<PlanAlimenticio>();
			var simulatedOrder = new CreateEvaluacionCommand { Descripcion = "Simulated Order", PlanAlimenticioId = Guid.NewGuid() };

			_ = A.CallTo(() => planAlimenticioRepository.GetByIdAsync(A<Guid>._, A<bool>._))
				.Returns(planAlimenticio);

			var handler = new CreateEvaluacionHandler(planAlimenticioRepository, unitOfWork);
			// var command = new CreateEvaluacionCommand
			// {
			//     PlanAlimenticioId = planAlimenticio.Id,
			//     Descripcion = "Test"
			// };

			Result<Guid> result = await handler.Handle(simulatedOrder, CancellationToken.None);

			_ = A.CallTo(() => planAlimenticioRepository.AddEvaluacion(planAlimenticio, A<Evaluacion>._))
				.MustHaveHappenedOnceExactly();

			_ = A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
				.MustHaveHappenedOnceExactly();

			Assert.That(result.IsSuccess, Is.True);

		}
	}
}
