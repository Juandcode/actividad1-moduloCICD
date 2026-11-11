using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestionClinicaNutricional.Infrastructure.Repositories
{
    public class PacienteRepository(DatabaseContext databaseContext) : IPacienteRepository
    {
        public async Task<Paciente?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            Paciente? paciente;
            if (readOnly)
            {
                paciente = await databaseContext.Paciente.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                if (paciente == null) throw new NullReferenceException("Paciente no encontrado.");
                await databaseContext.Entry(paciente).Collection(p => p.ConsultaIniciales).LoadAsync();
            }
            else
            {
                paciente = await databaseContext.Paciente.FirstOrDefaultAsync(i => i.Id == id);
                if (paciente == null) throw new NullReferenceException("Paciente no encontrado.");
                await databaseContext.Entry(paciente).Collection(p => p.ConsultaIniciales).LoadAsync();
            }

            return paciente;
        }

        public async Task AddAsync(Paciente entity)
        {
            await databaseContext.Paciente.AddAsync(entity);
        }

        public Task UpdateAsync(Paciente paciente)
        {
            // var added = paciente.DomainEvents.Where(e => e is TransactionItemAdded)
            //     .Select(e => (TransactionItemAdded)e)
            //     .ToList();
            // foreach (var e in added)
            // {
            //     var itemToAdd = transaction.Items.First(i => i.ItemId == e.ItemId);
            //     _dbContext.TransactionItem.Add(itemToAdd);
            // }


            databaseContext.Paciente.Update(paciente);
            return Task.CompletedTask;
        }
    }
}