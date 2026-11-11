using Joseco.Communication.External.RabbitMQ;
using Joseco.Communication.External.RabbitMQ.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionClinicaNutricional.WorkerService
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddGestionClinicaRabbitMq(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = configuration
                .GetSection("RabbitMqSettings")
                .Get<RabbitMqSettings>();

            if (settings is null)
            {
                throw new InvalidOperationException(
                    "Configuration section 'RabbitMqSettings' is missing or empty.");
            }

            services.AddSingleton(settings);

            return services.AddRabbitMQ(settings);
        }
    }
}