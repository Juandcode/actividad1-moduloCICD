using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.Contracts.Model;
using Joseco.Outbox.Contracts.Service;
using MediatR;
using GestionClinicaNutricional.Domain.Events;

namespace GestionClinicaNutricional.Application.UseCase.DomainEventHandler
{
    public class SaveInOutboxWhenPacienteCreated : INotificationHandler<PacienteCreated>
    {
        private readonly IOutboxService<DomainEvent> _outboxService;

        public SaveInOutboxWhenPacienteCreated(IOutboxService<DomainEvent> outboxService)
        {
            _outboxService = outboxService;
        }

        public async Task Handle(PacienteCreated notification, CancellationToken cancellationToken = default)
        {
            await _outboxService.AddAsync(new OutboxMessage<DomainEvent>(notification));
        }
    }
}