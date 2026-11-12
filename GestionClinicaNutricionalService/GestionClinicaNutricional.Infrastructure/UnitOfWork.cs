using System.Collections.Immutable;
using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.Contracts.Model;
using Joseco.Outbox.EFCore.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestionClinicaNutricional.Infrastructure
{
	internal class UnitOfWork(DatabaseContext dbContext, IMediator mediator) : IUnitOfWork, IOutboxDatabase<DomainEvent>
	{
		private readonly DatabaseContext _dbContext = dbContext;
		private readonly IMediator _mediator = mediator;

		public DbSet<OutboxMessage<DomainEvent>> GetOutboxMessages()
		{
			return _dbContext.OutboxMessages;
		}

		public async Task CommitAsync(CancellationToken cancellationToken = default)
		{

			//Get domain events
			var domainEvents = _dbContext.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents.Count != 0)
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
			foreach (DomainEvent? domainEvent in domainEvents)
			{
				await _mediator.Publish(domainEvent, cancellationToken);
			}


			_ = await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
