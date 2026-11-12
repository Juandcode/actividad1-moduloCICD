using GestionClinicaNutricional.Application.Contracts;
using GestionClinicaNutricional.Domain.Events;
using Joseco.Communication.External.Contracts.Services;
using Joseco.Outbox.Contracts.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestionClinicaNutricional.Application.UseCase.DomainEventHandler
{
	/// <summary>
	/// Translates the outbox message that carries <see cref="PacienteCreated"/> into the
	/// external contract and publishes it to RabbitMQ.
	/// </summary>
	public class PublishIntegrationEventWhenPacienteCreated(
		ILogger<PublishIntegrationEventWhenPacienteCreated> logger,
		IExternalPublisher publisher)
				: INotificationHandler<OutboxMessage<PacienteCreated>>
	{
		public const string Destination = "paciente-published";
		public const string Source = "gestion-clinica-nutricional";

		private readonly ILogger<PublishIntegrationEventWhenPacienteCreated> _logger = logger;
		private readonly IExternalPublisher _publisher = publisher;

		public async Task Handle(OutboxMessage<PacienteCreated> notification, CancellationToken cancellationToken = default)
		{
			PacienteCreated pacienteCreated = notification.Content;

			var message = new PacientePublished(notification.CorrelationId, Source)
			{
				PacienteId = pacienteCreated.Id,
				CI = pacienteCreated.CI,
				Nombre = pacienteCreated.Nombre,
				Apellido = pacienteCreated.Apellido
			};

			_logger.LogInformation(
				"Publishing {EventType} for paciente {PacienteId}: {Nombre} {Apellido}",
				notification.Content.GetType().Name, pacienteCreated.PacienteId, pacienteCreated.Nombre, pacienteCreated.Apellido);

			await _publisher.PublishAsync(message, Destination, declareDestination: true);
		}
	}
}
