using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Repositories
{
    public interface IPlanAlimenticioRepository: IRepository<PlanAlimenticio>
    {
        Task UpdateAsync(PlanAlimenticio planAlimenticio);
        Task AddEvaluacion(PlanAlimenticio planAlimenticio, Evaluacion evaluacion);
        Task<List<Evaluacion>> GetEvaluaciones(Guid planAlimenticioId);
        Task ProgramarProximoControl(Guid planAlimenticioId, DateTime fecha);
    }
    
}