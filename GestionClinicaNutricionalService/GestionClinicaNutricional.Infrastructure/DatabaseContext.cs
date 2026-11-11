using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GestionClinicaNutricional.Domain;
using Joseco.DDD.Core.Abstractions;
using Joseco.Outbox.Contracts.Model;
using Joseco.Outbox.EFCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionClinicaNutricional.Infrastructure;

[ExcludeFromCodeCoverage]
public class DatabaseContext: DbContext
{
    public DbSet<ConsultaInicial> ConsultaInicial { get; set; }
    public DbSet<Paciente> Paciente { get; set; }
    public DbSet<PlanAlimenticio> PlanAlimenticio { get; set; }
    public DbSet<Evaluacion> Evaluacion { get; set; }
    public DbSet<HabitoAlimenticio> HabitoAlimenticio { get; set; }
    
    public DbSet<OutboxMessage<DomainEvent>> OutboxMessages { get; set; } = null!;
    
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.AddOutboxModel<DomainEvent>();

        base.OnModelCreating(modelBuilder);
        
        // modelBuilder.Entity<HabitoAlimenticio>()
        //     .OwnsOne(ha => ha.TipoComida);
        
        modelBuilder.Entity<ConsultaInicial>()
            .OwnsMany(p => p.Antecedentes, a => a.ToJson());
        
        
        // modelBuilder.Entity<PlanAlimenticio>()
        //     .HasOne(p => p.Paciente)
        //     .WithOne(pa => pa.PlanAlimenticio)
        //     .HasForeignKey<Paciente>(pa => pa.PlanAlimenticioId);
        
        // modelBuilder.Entity<ConsultaInicial>()
        //     .HasOne(c => c.Paciente)
        //     .WithOne(pa => pa.ConsultaInicial)
        //     .HasForeignKey<Paciente>(pa => pa.ConsultaInicialId);

        modelBuilder.Ignore<DomainEvent>();
    }
}