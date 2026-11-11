using System.Collections.Immutable;
using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.Contracts.Model;
using Joseco.Outbox.EFCore.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionClinicaNutricional.Infrastructure
{
    internal class UnitOfWork : IUnitOfWork, IOutboxDatabase<DomainEvent>
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMediator _mediator;

        public UnitOfWork(DatabaseContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public DbSet<OutboxMessage<DomainEvent>> GetOutboxMessages() => _dbContext.OutboxMessages;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {

            //Get domain events
            var domainEvents = _dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(entityEntry =>
                {
                    var domainEvents = entityEntry.Entity
                        .DomainEvents
                        .ToImmutableArray();
                    entityEntry.Entity.ClearDomainEvents();

                    return domainEvents;
                })
                .SelectMany(domainEvents => domainEvents)
                .ToList();

            //[[e1, e2], [e3]] => [e1, e2, e3]

            //Publish Domain Events
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }


            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}