using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;


namespace Microsoft.Extensions.DependencyInjection
{
    using Assimalign.Azure.Cosmos;

    public static class CosmosServiceCollectionExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosService<T>(this IServiceCollection services, Action<CosmosOptions> configure)
            where T : class, new()
        {
            return services.AddSingleton<ICosmosRepository<T>, CosmosRepository<T>>(serviceProvider =>
            {
                var options = new CosmosOptions();
                configure.Invoke(options);
                return new CosmosRepositoryDefault<T>(options);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosService<T>(this IServiceCollection services, Action<IServiceProvider, CosmosOptions> configure)
            where T : class, new()
        {
            return services.AddSingleton<ICosmosRepository<T>, CosmosRepository<T>>(serviceProvider =>
            {
                var options = new CosmosOptions();
                configure.Invoke(serviceProvider, options);
                return new CosmosRepositoryDefault<T>(options);
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosService<T>(this IServiceCollection services, Func<IServiceProvider, CosmosOptions> configure)
            where T : class, new()
        {
            return services.AddSingleton<ICosmosRepository<T>, CosmosRepository<T>>(serviceProvider =>
            {
                var options = configure.Invoke(serviceProvider);
                return new CosmosRepositoryDefault<T>(options);
            });
        }


        internal sealed class CosmosRepositoryDefault<T> : CosmosRepository<T>
            where T : class, new()
        {
            public CosmosRepositoryDefault(CosmosOptions options) 
                : base(options) { }
        }
    }
}
