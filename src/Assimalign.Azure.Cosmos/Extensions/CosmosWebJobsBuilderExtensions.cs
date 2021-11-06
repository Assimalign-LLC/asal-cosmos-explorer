using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs
{
    using Assimalign.Azure.Cosmos;
    using Assimalign.Azure.Cosmos.Bindings;

    public static class CosmosWebJobsBuilderExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IWebJobsBuilder AddCosmosExtensions<T>(this IWebJobsBuilder builder, Action<CosmosOptions> configure)
            where T : class, new()
        {
            builder.Services.AddCosmosService<T>(configure);
            builder.AddExtension<CosmosBindingExtensionBindingProvider<T>>();
            return builder;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IWebJobsBuilder AddCosmosExtensions<T>(this IWebJobsBuilder builder, Action<IServiceProvider, CosmosOptions> configure)
           where T : class, new()
        {
            builder.Services.AddCosmosService<T>(configure);
            builder.AddExtension<CosmosBindingExtensionBindingProvider<T>>();
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IWebJobsBuilder AddCosmosExtensions<T>(this IWebJobsBuilder builder, Func<IServiceProvider, CosmosOptions> configure)
            where T : class, new()
        {
            builder.Services.AddCosmosService<T>(configure);
            builder.AddExtension<CosmosBindingExtensionBindingProvider<T>>();
            return builder;
        }
    }
}
