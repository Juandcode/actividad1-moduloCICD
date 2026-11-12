using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace GestionClinicaNutricional.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			_ = services.AddMediatR(config =>
			{
				_ = config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			return services;
		}
	}
}
