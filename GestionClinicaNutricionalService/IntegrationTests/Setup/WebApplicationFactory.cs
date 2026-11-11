using System;
using System.Linq;
using System.Threading.Tasks;
using GestionClinicaNutricional.Domain.Repositories;
using GestionClinicaNutricional.Infrastructure;
using GestionClinicaNutricionalService.WebApi.Controllers;
using Joseco.DDD.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace IntegrationTests.Setup
{
    public class WebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly string _dbName = $"IntTests_Item_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(
                services =>
                {
                    // var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPacienteRepository));
                    // if (descriptor != null)
                    // {
                    //     services.Remove(descriptor);
                    // }
                    //
                    // var unitDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUnitOfWork));
                    // if (unitDescriptor != null)
                    // {
                    //     services.Remove(unitDescriptor);
                    // }
                    // services.AddSingleton<IPacienteRepository, InMemoryItemRepository>();
                    // services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();

                    // Calling AddDbContext<PersistenceDbContext> again would ADD to, not replace, the
                    // Npgsql configuration already registered by Program.cs, causing EF Core to see two
                    // providers on the same context. Register an already-built DbContextOptions instance
                    // instead, bypassing that additive pipeline entirely.
                    services.RemoveAll<DbContextOptions<DatabaseContext>>();
                    services.AddSingleton(
                        new DbContextOptionsBuilder<DatabaseContext>()
                            .UseSqlServer(
                                "Server=localhost\\SQLEXPRESS;Database=ClinicaNutricional;MultipleActiveResultSets=True;Trusted_Connection=True;TrustServerCertificate=True")
                            .Options);
                });
        }

        public async Task InitializeAsync()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await db.Database.MigrateAsync();
        }

        public new Task DisposeAsync() => Task.CompletedTask;// no hay contenedor que tirar
    }
}