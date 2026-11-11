using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;

namespace Tests
{
  using FakeItEasy;
using NUnit.Framework;

[TestFixture]
public class PlanAlimenticioTests
{
    private IPlanAlimenticioRepository planAlimenticioRepository;
    private IConsultaInicialRepository consultaInicialRepository;
    private IPacienteRepository pacienteRepository;
    private IUnitOfWork unitOfWork;

    [SetUp]
    public void Setup()
    {
        planAlimenticioRepository = A.Fake<IPlanAlimenticioRepository>();
        consultaInicialRepository = A.Fake<IConsultaInicialRepository>();
        pacienteRepository = A.Fake<IPacienteRepository>();
        unitOfWork = A.Fake<IUnitOfWork>();
    }

    [Test]
    public async Task PlanAlimenticio_ShouldCreatePlanAlimenticioSuccessfully()
    {
        // Arrange
        var paciente = A.Dummy<Paciente>();

        var command = new CreatePlanAlimenticioCommand
        {
            Descripcion = "Plan de prueba",
            Nombre = "Plan Simulado",
            DuracionPlan = DuracionPlan.Mes,
            EstadoPlan = EstadoPlan.Vigente,
            FechaVencimiento = DateTime.Now.AddDays(30),
            PlanComidas = new List<PlanComida>(),
            PacienteId = Guid.NewGuid()
        };

        A.CallTo(() => pacienteRepository.GetByIdAsync(A<Guid>._,A<bool>._))
            .Returns(paciente);

        var handler = new CreatePlanAlimenticioHandler(
            planAlimenticioRepository,
            consultaInicialRepository,
            pacienteRepository,
            unitOfWork);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        A.CallTo(() => planAlimenticioRepository.AddAsync(
                A<PlanAlimenticio>.That.Matches(p =>
                    p.Descripcion == command.Descripcion &&
                    p.Nombre == command.Nombre &&
                    p.PacienteId == paciente.Id)))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => unitOfWork.CommitAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        Assert.That(result.IsSuccess, Is.True);
    }
}
}