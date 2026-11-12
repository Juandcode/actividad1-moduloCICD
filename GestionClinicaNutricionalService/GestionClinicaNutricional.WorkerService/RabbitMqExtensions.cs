using Joseco.Communication.External.RabbitMQ;
using Joseco.Communication.External.RabbitMQ.Services;

namespace GestionClinicaNutricional.WorkerService
{
	public static class RabbitMqExtensions
	{
		public static IServiceCollection AddGestionClinicaRabbitMq(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			RabbitMqSettings settings = configuration
				.GetSection("RabbitMqSettings")
				.Get<RabbitMqSettings>() ?? throw new InvalidOperationException(
					"Configuration section 'RabbitMqSettings' is missing or empty.");
			_ = services.AddSingleton(settings);

			return services.AddRabbitMQ(settings);
		}
	}
}
