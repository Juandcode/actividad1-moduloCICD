using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain.Repositories
{
    public interface IConsultaInicialRepository: IRepository<ConsultaInicial>
    {
        Task<ConsultaInicial> GetLastByPacienteId(Guid pacienteId);
        Task UpdateAsync(ConsultaInicial consultaInicial, List<HabitoAlimenticio> habitoAlimenticios);
        Task AddHabitoAlimenticiosAsync(ConsultaInicial consultaInicial, List<HabitoAlimenticio> habitoAlimenticios);
        Task<List<ConsultaInicial>> AllConsultasAsync(Guid pacienteId);
        Task ApproveConsulta(Guid consultaId);
    }
}