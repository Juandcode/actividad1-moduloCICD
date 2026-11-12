using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Repositories
{
	public interface IPlanAlimenticioRepository : IRepository<PlanAlimenticio>
	{
		public Task UpdateAsync(PlanAlimenticio planAlimenticio);
		public Task AddEvaluacion(PlanAlimenticio planAlimenticio, Evaluacion evaluacion);
		public Task<List<Evaluacion>> GetEvaluaciones(Guid planAlimenticioId);
		public Task ProgramarProximoControl(Guid planAlimenticioId, DateTime fecha);
	}

}
