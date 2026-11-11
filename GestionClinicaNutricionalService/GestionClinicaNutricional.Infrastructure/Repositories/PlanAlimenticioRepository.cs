using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestionClinicaNutricional.Infrastructure.Repositories
{
    public class PlanAlimenticioRepository(DatabaseContext databaseContext): IPlanAlimenticioRepository
    {
        public async Task<PlanAlimenticio?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            PlanAlimenticio? planAlimenticio;
            if (readOnly)
            {
                planAlimenticio = await databaseContext.PlanAlimenticio.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                if (planAlimenticio == null) throw new NullReferenceException("Paciente no encontrado.");
                await databaseContext.Entry(planAlimenticio).Collection(p => p.Evaluaciones).LoadAsync();
            }
            else
            {
                planAlimenticio = await databaseContext.PlanAlimenticio.FirstOrDefaultAsync(i => i.Id == id);
                if (planAlimenticio == null) throw new NullReferenceException("Paciente no encontrado.");
                await databaseContext.Entry(planAlimenticio).Collection(p => p.Evaluaciones).LoadAsync();
            }

            return planAlimenticio;
        }

        public async Task AddAsync(PlanAlimenticio entity)
        {
            await databaseContext.PlanAlimenticio.AddAsync(entity);
        }

        public Task UpdateAsync(PlanAlimenticio paciente)
        {
            // var added = paciente.DomainEvents.Where(e => e is TransactionItemAdded)
            //     .Select(e => (TransactionItemAdded)e)
            //     .ToList();
            // foreach (var e in added)
            // {
            //     var itemToAdd = transaction.Items.First(i => i.ItemId == e.ItemId);
            //     _dbContext.TransactionItem.Add(itemToAdd);
            // }


            databaseContext.PlanAlimenticio.Update(paciente);
            return Task.CompletedTask;
        }

        public async Task AddEvaluacion(PlanAlimenticio planAlimenticio, Evaluacion evaluacion)
        {
            evaluacion.PlanAlimenticioId = planAlimenticio.Id;
            await databaseContext.Evaluacion.AddAsync(evaluacion);
            //databaseContext.PlanAlimenticio.Update(planAlimenticio);
        }

        public Task<List<Evaluacion>> GetEvaluaciones(Guid planAlimenticioId)
        {
            List<Evaluacion> evaluaciones = databaseContext.Evaluacion.Where(e => e.PlanAlimenticioId == planAlimenticioId).ToList();
            return Task.FromResult(evaluaciones);
        }

        public async Task ProgramarProximoControl(Guid planAlimenticioId, DateTime fecha)
        {
            var planAlimenticio = await databaseContext.PlanAlimenticio.FirstOrDefaultAsync(p => p.Id == planAlimenticioId);
            planAlimenticio.FechaSiguienteControl = fecha;
            databaseContext.PlanAlimenticio.Update(planAlimenticio);
        }
    }
}