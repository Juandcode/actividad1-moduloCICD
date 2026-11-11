using System.Reflection;
using GestionClinicaNutricional.Application;
using GestionClinicaNutricional.Domain.Repositories;
using GestionClinicaNutricional.Infrastructure.Repositories;
using GestionClinicaNutricional.Infrastructure.Observability;
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
            services.AddApplication();
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' is missing or empty.");
            }

            services.AddDbContext<DatabaseContext>(
                context =>
                    context.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOutboxDatabase<DomainEvent>, UnitOfWork>().AddOutbox<DomainEvent>();
            services.AddScoped<IConsultaInicialRepository, ConsultaInicialRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IPlanAlimenticioRepository, PlanAlimenticioRepository>();
            
            services.AddMediatR(config =>
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

  