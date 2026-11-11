using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricionalService.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionClinicaNutricionalService.WebApi.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ConsultationController(IMediator mediator) : ControllerBase
	{
		[HttpPost("{pacienteId}/" + nameof(Consultation))]
		public async Task<IActionResult> Consultation(
			[FromRoute] Guid pacienteId,
			[FromBody] CreateConsultaRequest request)
		{
			var requestHandler = new CreateConsultaCommand
			{
				PacienteId = pacienteId,
				Altura = request.Altura,
				Antecedentes =
					request.Antecedentes
						.Select(antecedente => new Antecedente(antecedente.Descripcion, antecedente.Problema)).ToList(),
				Composicion = request.Composicion,
				Peso = request.Peso,
				HabitoAlimenticios = request.HabitoAlimenticios.Select(
						habito => new HabitoAlimenticio() { Nombre = habito.Nombre, Descripcion = habito.Descripcion })
					.ToList(),
			};
			//var requestConId = request with { PacienteId = pacienteId };
			var result = await mediator.Send(requestHandler);

			return Ok(result);
		}

		[HttpGet("{pacienteId}/" + nameof(Consultations))]
		public async Task<IActionResult> Consultations([FromRoute] Guid pacienteId)
		{
			var requestConId = new GetConsultasCommand { PacienteId = pacienteId };
			var result = await mediator.Send(requestConId);

			return Ok(result);
		}

		[HttpPost("{consultaId}/" + nameof(Approve))]
		public async Task<IActionResult> Approve([FromRoute] Guid consultaId)
		{
			var requestConId = new ApproveConsultaCommand { ConsultaId = consultaId };
			var result = await mediator.Send(requestConId);

			return Ok(result);
		}
	}
}
