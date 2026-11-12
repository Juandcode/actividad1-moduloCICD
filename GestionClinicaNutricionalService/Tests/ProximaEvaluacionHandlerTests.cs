using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class ProximaEvaluacionHandlerTests
	{
		private IPlanAlimenticioRepository planAlimenticioRepository;
		private IUnitOfWork unitOfWork;

		[SetUp]
		public void Setup()
		{
			planAlimenticioRepository = A.Fake<IPlanAlimenticioRepository>();
			unitOfWork = A.Fake<IUnitOfWork>();
		}

		[Test]
		public async Task ProximaEvaluacion_ShouldProgramarProximoControlSuccessfully()
		{
			// Arrange
			var command = new ProximaEvaluacionCommand
			{
				PlanAlimenticioId = Guid.NewGuid(),
				ProximaFecha = DateTime.Now.AddDays(15)
			};

			var handler = new ProximaEvaluacionHandler(planAlimenticioRepository, unitOfWork);

			// Act
			Result<Guid> result = await handler.Handle(command, CancellationToken.None);

			// Assert
			_ = A.CallTo(() => planAlimenticioRepository.ProgramarProximoControl(
					command.PlanAlimenticioId,
					command.ProximaFecha))
				.MustHaveHappenedOnceExactly();

			_ = A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
				.MustHaveHappenedOnceExactly();

			Assert.That(result.IsSuccess, Is.True);
			Assert.That(result.Value, Is.EqualTo(command.PlanAlimenticioId));
		}
	}
}
