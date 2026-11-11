using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricionalService.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionClinicaNutricionalService.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PacienteController(IMediator mediator) : ControllerBase
	{
		[HttpPost(nameof(CreatePaciente))]
		public async Task<IActionResult> CreatePaciente([FromBody] CreatePacienteCommand request)
		{
			var result = await mediator.Send(request);

			return Ok(result);
		}

		[HttpPost("{pacienteId}/" + nameof(CreateConsulta))]
		public async Task<IActionResult> CreateConsulta(
			[FromRoute] Guid pacienteId,
			[FromBody] CreateConsultaRequest request)
		{
			var requestHandler = new CreateConsultaCommand
			{
				PacienteId = pacienteId,
				Altura = request.Altura,
				Antecedentes =
					request.Antecedentes.Select(antecedente => new Antecedente(antecedente.Descripcion, antecedente.Problema))
						.ToList(),
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

		[HttpPost("{planAlimenticioId}/" + nameof(CreateEvaluacion))]
		public async Task<IActionResult> CreateEvaluacion(
			[FromRoute] Guid planAlimenticioId,
			[FromBody] CreateEvaluacionCommand request)
		{
			var requestConId = request with { PlanAlimenticioId = planAlimenticioId };
			var result = await mediator.Send(requestConId);

			return Ok(result);
		}

		[HttpGet("{planAlimenticioId}/" + nameof(Evaluaciones))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evaluacion>))]
		public async Task<IActionResult> Evaluaciones([FromRoute] Guid planAlimenticioId)
		{
			var request = new GetEvaluacionesCommand { PlanAlimenticioId = planAlimenticioId };
			var result = await mediator.Send(request);

			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPacienteById([FromRoute] Guid id)
		{
			var request = new GetPacienteByIdQuery { Id = id };
			var result = await mediator.Send(request);

			if (result.IsFailure)
			{
				return NotFound(result.Error);
			}

			return Ok(result.Value);
		}
	}
}
