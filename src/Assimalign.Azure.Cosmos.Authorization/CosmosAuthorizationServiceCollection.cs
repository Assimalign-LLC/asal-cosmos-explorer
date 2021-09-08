using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
	using Assimalign.Azure.Cosmos.Authorization;

    public static class CosmosAuthorizationServiceCollection
    {

		public static IServiceCollection AddConfidentialCosmosServices<T>(this IServiceCollection services, Action<CosmosAuthorizationOptions<T>> options)
			where T : class
		{

			//services.AddSingleton<ICosmosConfidentialRepository<T>, CosmosConfidentialRepository<T>(ServiceProvider =>
			//{
			//	var settings = new CosmosAuthorizationOptions<T>();
			//	options.Invoke(settings);


			//	return new CosmosConfidentialRepository<T>()

			//});




			return services;
		}
	}
}
