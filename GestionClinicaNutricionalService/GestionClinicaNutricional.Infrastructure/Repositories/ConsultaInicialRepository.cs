using GestionClinicaNutricional.Domain;
using GestionClinicaNutricional.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestionClinicaNutricional.Infrastructure.Repositories
{
    public class ConsultaInicialRepository(DatabaseContext databaseContext) : IConsultaInicialRepository
    {
        public async Task<ConsultaInicial> GetLastByPacienteId(Guid pacienteId)
        {
            ConsultaInicial? consultaInicial = await databaseContext.ConsultaInicial
                .Where(i => i.PacienteId == pacienteId)
                .OrderByDescending(i => i.Fecha)
                .FirstOrDefaultAsync();
            if (consultaInicial == null) throw new NullReferenceException("Consulta no encontrado.");
            return consultaInicial;
        }

        public async Task<ConsultaInicial?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            ConsultaInicial? consultaInicial;
            if (readOnly)
            {
                consultaInicial = await databaseContext.ConsultaInicial.AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == id);
                if (consultaInicial == null) throw new NullReferenceException("Consulta no encontrado.");
                await databaseContext.Entry(consultaInicial).Collection(p => p.HabitoAlimenticios).LoadAsync();
            }
            else
            {
                consultaInicial = await databaseContext.ConsultaInicial.FirstOrDefaultAsync(i => i.Id == id);
                if (consultaInicial == null) throw new NullReferenceException("Consulta no encontrado.");
                await databaseContext.Entry(consultaInicial).Collection(p => p.HabitoAlimenticios).LoadAsync();
            }

            return consultaInicial;
        }

        public async Task AddAsync(ConsultaInicial entity)
        {
            await databaseContext.ConsultaInicial.AddAsync(entity);
        }

        public Task UpdateAsync(ConsultaInicial consultaInicial, List<HabitoAlimenticio> habitoAlimenticios)
        {
            consultaInicial.HabitoAlimenticios.AddRange(habitoAlimenticios);
            return Task.CompletedTask;
        }

        public Task AddHabitoAlimenticiosAsync(
            ConsultaInicial consultaInicial,
            List<HabitoAlimenticio> habitoAlimenticios)
        {
            foreach (var habitoAlimenticio in habitoAlimenticios)
            {
                habitoAlimenticio.ConsultaInicialId = consultaInicial.Id;
            }

            databaseContext.HabitoAlimenticio.AddRange(habitoAlimenticios);
            //databaseContext.ConsultaInicial.Update(consultaInicial);
            return Task.CompletedTask;
        }

        public async Task<List<ConsultaInicial>> AllConsultasAsync(Guid pacienteId)
        {
            var consultas = await databaseContext.ConsultaInicial.Where(c => c.PacienteId == pacienteId)
                .Include(c => c.HabitoAlimenticios)
                .OrderByDescending(c => c.Fecha).ToListAsync();
            return consultas;
        }

        public async Task ApproveConsulta(Guid consultaId)
        {
            var consulta = await databaseContext.ConsultaInicial.FirstOrDefaultAsync(c => c.Id == consultaId);
            consulta.Estado = true;
            databaseContext.ConsultaInicial.Update(consulta);
        }
    }
}