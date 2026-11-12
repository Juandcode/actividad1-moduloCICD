using GestionClinicaNutricional.Domain.Events;
using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.Contracts.Model;
using Joseco.Outbox.Contracts.Service;
using MediatR;

namespace GestionClinicaNutricional.Application.UseCase.DomainEventHandler
{
	public class SaveInOutboxWhenPacienteCreated(IOutboxService<DomainEvent> outboxService) : INotificationHandler<PacienteCreated>
	{
		private readonly IOutboxService<DomainEvent> _outboxService = outboxService;

		public async Task Handle(PacienteCreated notification, CancellationToken cancellationToken = default)
		{
			await _outboxService.AddAsync(new OutboxMessage<DomainEvent>(notification));
		}
	}
}
