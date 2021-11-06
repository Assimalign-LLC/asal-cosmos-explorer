using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Extensions.Options;

namespace Assimalign.Azure.Cosmos.Bindings
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Utilities;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Azure.WebJobs.Host.Bindings;

    internal sealed partial class CosmosBindingExtensionBindingProvider<T> : IExtensionConfigProvider
        where T : class, new()
    {
        private readonly CosmosOptions cosmosOptions;
        private readonly ICosmosRepository<T> cosmosRepository;
        private readonly IHttpContextAccessor httpContextAccessor;


        private readonly IDictionary<string, Type> fields;

        public CosmosBindingExtensionBindingProvider(
            IOptions<CosmosOptions> cosmosOptions,
            ICosmosRepository<T> cosmosRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.cosmosRepository = cosmosRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.cosmosOptions = cosmosOptions.Value;
            this.fields = GetCollectionBreakdown(typeof(T));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            BindCosmosQuery(context);
            BindCosmosRepository(context);
        }


        private IDictionary<string, Type> GetCollectionBreakdown(Type type)
        {
            var breakdown = new Dictionary<string, Type>();
            var properties = type.GetProperties().Where(x => x.CanRead && x.CanWrite);

            foreach (var property in properties)
            {
                var propertyName = property.Name;

                if (property.PropertyType.IsSystemValueType(out var valueType))
                {
                    breakdown.Add(propertyName, valueType);
                }
                else if (property.PropertyType.IsObjectType())
                {
                    foreach (var child in GetCollectionBreakdown(property.PropertyType))
                    { 
                        breakdown.Add($"{property.Name}.{child.Key}", child.Value);
                    }
                }
            }

            return breakdown;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void BindCosmosQuery(ExtensionConfigContext context) =>
            context.AddBindingRule<CosmosBindingAttribute>()
                .BindToInput<ICosmosQuery<T>>((attribute, context) =>
                {
                    return GetCosmosQueryAsync(attribute, context);
                });

   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void BindCosmosRepository(ExtensionConfigContext context) =>
             context.AddBindingRule<CosmosBindingAttribute>()
                .BindToInput<ICosmosRepository<T>>(builder =>
                {
                    return cosmosRepository;
                });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Task<ICosmosQuery<T>> GetCosmosQueryAsync(CosmosBindingAttribute attribute, ValueBindingContext context) =>
            cosmosOptions.QueryParser.ParseAsync<T>(httpContextAccessor.HttpContext, context);
    }
}
