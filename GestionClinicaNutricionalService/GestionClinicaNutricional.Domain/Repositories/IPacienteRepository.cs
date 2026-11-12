using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Repositories
{
	public interface IPacienteRepository : IRepository<Paciente>
	{
		public Task UpdateAsync(Paciente paciente);
	}
}
