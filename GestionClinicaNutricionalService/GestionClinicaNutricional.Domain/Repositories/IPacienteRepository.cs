using System.Transactions;
using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Repositories
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        Task UpdateAsync(Paciente paciente);
    }
}