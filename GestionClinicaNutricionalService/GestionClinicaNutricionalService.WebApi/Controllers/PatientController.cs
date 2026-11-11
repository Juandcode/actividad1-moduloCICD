using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using GestionClinicaNutricionalService.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionClinicaNutricionalService.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PatientController(IMediator mediator) : ControllerBase
    {
     
        
        [HttpPost(nameof(CreatePaciente))]
        public async Task<IActionResult> CreatePaciente([FromBody] CreatePacienteCommand request)
        {
            var result = await mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("{pacienteId}/" + nameof(Plan))]
        public async Task<IActionResult> Plan(
            [FromRoute] Guid pacienteId,
            [FromBody] CreatePlanAlimenticioRequest request)
        {
            var command = new CreatePlanAlimenticioCommand()
            {
                PacienteId = pacienteId,
                DuracionPlan = request.DuracionPlan,
                EstadoPlan = request.EstadoPlan,
                PlanComidas = request.PlanComidas.Select(
                    planComida => new PlanComida()
                    {
                        Nombre = planComida.Nombre, Descripcion = planComida.Descripcion,
                        Categoria = planComida.Categoria
                    }).ToList(),
                Descripcion = request.Descripcion,
                Nombre = request.Nombre,
                FechaVencimiento = request.FechaVencimiento,
            };
            //var requestConId = request with { PacienteId = pacienteId };
            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPatch("{planAlimenticioId}/" + nameof(SchedulePlan))]
        public async Task<IActionResult> SchedulePlan(
            [FromRoute] Guid planAlimenticioId,
            [FromBody] ProximaEvaluacionCommand request)
        {
            var requestConId = request with { PlanAlimenticioId = planAlimenticioId };
            var result = await mediator.Send(requestConId);

            return Ok(result);
        }

        [HttpGet("{planAlimenticioId}/" + nameof(ControlEvaluationHistory))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evaluacion>))]
        public async Task<IActionResult> ControlEvaluationHistory([FromRoute] Guid planAlimenticioId)
        {
            var request = new GetEvaluacionesCommand { PlanAlimenticioId = planAlimenticioId };
            var result = await mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("{planAlimenticioId}/" + nameof(CreateControlEvaluacion))]
        public async Task<IActionResult> CreateControlEvaluacion(
            [FromRoute] Guid planAlimenticioId,
            [FromBody] CreateEvaluacionCommand request)
        {
            var requestConId = request with { PlanAlimenticioId = planAlimenticioId };
            var result = await mediator.Send(requestConId);

            return Ok(result);
        }

        // [HttpGet("{pacienteId}/" + nameof(Consultations))]
        // public async Task<IActionResult> Consultations(
        //     [FromRoute] Guid pacienteId)
        // {
        //     var requestConId = new GetConsultasCommand {PacienteId = pacienteId};
        //     var result = await mediator.Send(requestConId);
        //
        //     return Ok(result);
        // }

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