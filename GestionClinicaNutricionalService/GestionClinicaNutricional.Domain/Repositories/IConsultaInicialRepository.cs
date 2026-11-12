using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Repositories
{
	public interface IConsultaInicialRepository : IRepository<ConsultaInicial>
	{
		public Task<ConsultaInicial> GetLastByPacienteId(Guid pacienteId);
		public Task UpdateAsync(ConsultaInicial consultaInicial, List<HabitoAlimenticio> habitoAlimenticios);
		public Task AddHabitoAlimenticiosAsync(ConsultaInicial consultaInicial, List<HabitoAlimenticio> habitoAlimenticios);
		public Task<List<ConsultaInicial>> AllConsultasAsync(Guid pacienteId);
		public Task ApproveConsulta(Guid consultaId);
	}
}
