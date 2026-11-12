using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class ApproveConsultaHandlerTests
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
		public async Task ApproveConsulta_ShouldApproveConsultaSuccessfully()
		{
			// Arrange
			var command = new ApproveConsultaCommand { ConsultaId = Guid.NewGuid() };

			var handler = new ApproveConsultaHandler(consultaInicialRepository, unitOfWork);

			// Act
			Result<Guid> result = await handler.Handle(command, CancellationToken.None);

			// Assert
			_ = A.CallTo(() => consultaInicialRepository.ApproveConsulta(command.ConsultaId))
				.MustHaveHappenedOnceExactly();

			_ = A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
				.MustHaveHappenedOnceExactly();

			Assert.That(result.IsSuccess, Is.True);
			Assert.That(result.Value, Is.EqualTo(command.ConsultaId));
		}
	}
}
