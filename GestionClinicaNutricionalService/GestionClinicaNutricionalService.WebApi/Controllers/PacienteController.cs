using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricionalService.Contracts;
using Joseco.DDD.Core.Results;
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
			Result<Guid> result = await mediator.Send(request);

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
					[.. request.Antecedentes.Select(antecedente => new Antecedente(antecedente.Descripcion, antecedente.Problema))],
				Composicion = request.Composicion,
				Peso = request.Peso,
				HabitoAlimenticios = [.. request.HabitoAlimenticios.Select(
						habito => new HabitoAlimenticio() { Nombre = habito.Nombre, Descripcion = habito.Descripcion })],
			};

			//var requestConId = request with { PacienteId = pacienteId };
			Result<Guid> result = await mediator.Send(requestHandler);

			return Ok(result);
		}

		[HttpPost("{planAlimenticioId}/" + nameof(CreateEvaluacion))]
		public async Task<IActionResult> CreateEvaluacion(
			[FromRoute] Guid planAlimenticioId,
			[FromBody] CreateEvaluacionCommand request)
		{
			CreateEvaluacionCommand requestConId = request with { PlanAlimenticioId = planAlimenticioId };
			Result<Guid> result = await mediator.Send(requestConId);

			return Ok(result);
		}

		[HttpGet("{planAlimenticioId}/" + nameof(Evaluaciones))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evaluacion>))]
		public async Task<IActionResult> Evaluaciones([FromRoute] Guid planAlimenticioId)
		{
			var request = new GetEvaluacionesCommand { PlanAlimenticioId = planAlimenticioId };
			Result<List<Evaluacion>> result = await mediator.Send(request);

			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPacienteById([FromRoute] Guid id)
		{
			var request = new GetPacienteByIdQuery { Id = id };
			Result<Paciente> result = await mediator.Send(request);

			return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
		}
	}
}
