using System;
using System.Linq;
using System.Reflection;


using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.DependencyInjection;

using PawnArt.Logic;

namespace PawnArt.Application
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Registers services of the Application layer.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
		/// <returns>A reference to this instance after the operation has completed.</returns>
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			//BackgroundService
			services
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
			services.AddSingleton<MatchMakePoolSingleton>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
			//->

			return services;
		}
	}
}
