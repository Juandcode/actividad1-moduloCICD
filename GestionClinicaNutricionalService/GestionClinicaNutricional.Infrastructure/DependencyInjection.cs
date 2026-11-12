using System.Reflection;
using GestionClinicaNutricional.Application;
using GestionClinicaNutricional.Domain.Repositories;
using GestionClinicaNutricional.Infrastructure.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.EFCore;
using Joseco.Outbox.EFCore.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionClinicaNutricional.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			_ = services.AddApplication();

			var connectionString = configuration.GetConnectionString("DefaultConnection");

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new InvalidOperationException(
					"Connection string 'DefaultConnection' is missing or empty.");
			}

			_ = services.AddDbContext<DatabaseContext>(
				context =>
					context.UseSqlServer(connectionString));

			_ = services.AddScoped<IUnitOfWork, UnitOfWork>();
			_ = services.AddScoped<IOutboxDatabase<DomainEvent>, UnitOfWork>().AddOutbox<DomainEvent>();
			_ = services.AddScoped<IConsultaInicialRepository, ConsultaInicialRepository>();
			_ = services.AddScoped<IPacienteRepository, PacienteRepository>();
			_ = services.AddScoped<IPlanAlimenticioRepository, PlanAlimenticioRepository>();

			_ = services.AddMediatR(config =>
				config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
			);

			// // Observability
			// services.AddObservability();
			//
			// // Outbox & RabbitMQ
			// services.AddJosecoOutboxEFCore(configuration);
			// services.AddJosecoRabbitMqExternalPublisher(configuration);

			return services;
		}
	}
}

