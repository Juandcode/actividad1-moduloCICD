using System.Reflection;
using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.EFCore;
using GestionClinicaNutricional.Application;
using GestionClinicaNutricional.Application.UseCase.DomainEventHandler;
using GestionClinicaNutricional.Infrastructure;
//using GestionClinicaNutricional.Infrastructure.Observability;
using GestionClinicaNutricional.WorkerService;
//using Nur.Tricenter.Observability.Tracing;
using Serilog;
using Serilog.Enrichers.Span;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, configuration) => configuration
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithSpan()
    //.Enrich.WithEnvironmentName()
    //.Enrich.WithProcessId()
    .Enrich.WithProperty("ServiceName", "gestion-clinica-nutricional-service")
    .WriteTo.Console());


builder.Services.AddInfrastructure(builder.Configuration);

// builder.Services.AddObservability(
//     "gestion-clinica-nutricional-service",
//     shouldIncludeHttpInstrumentation: false);

builder.Services.AddGestionClinicaRabbitMq(builder.Configuration);

builder.Services.AddOutboxBackgroundService<DomainEvent>();

builder.Services.AddHostedService<DefaultQueueRegistrar>();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

var host = builder.Build();
host.Run();