using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionClinicaNutricionalService.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AtencionController(IMediator mediator) : ControllerBase
	{
		// [HttpPost("{consultaId}/" + nameof(CreatePlanAlimenticion))]
		// public async Task<IActionResult> CreatePlanAlimenticion(
		//     [FromRoute] Guid consultaId,
		//     [FromBody] CreatePlanAlimenticioCommand request)
		// {
		//     var requestConId = request with { ConsultaId = consultaId };
		//     var result = await mediator.Send(requestConId);
		//
		//     return Ok(result);
		// }
		[HttpPost("{pacienteId}/" + nameof(Consultas))]
		public async Task<IActionResult> Consultas([FromRoute] Guid pacienteId)
		{
			var requestConId = new GetConsultasCommand { PacienteId = pacienteId };
			Result<List<ConsultaInicial>> result = await mediator.Send(requestConId);

			return Ok(result);
		}
	}
}
