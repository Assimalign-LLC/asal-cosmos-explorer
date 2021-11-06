using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.OData.ModelBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs
{
    using Assimalign.Azure.Cosmos;
    using Assimalign.Azure.Cosmos.Bindings;
    using Assimalign.Azure.Cosmos.OData;
    

    public static class CosmosODataWebJobsExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebJobsBuilder AddCosmosODataExtensions<T>(this IWebJobsBuilder builder, Action<CosmosOptions> configure)
            where T : class, new()
        {
            builder.Services.AddHttpContextAccessor();

            var options = new CosmosODataOptions();

            // Let's query all functions in the APP domain
            var functions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes
                    .SelectMany(b => b.DeclaredMethods)
                    .Where(x => 
                    {
                        bool isValid = x.CustomAttributes.Any(a => a.AttributeType == typeof(FunctionNameAttribute)) &&
                                       x.GetParameters().Any(x => x.CustomAttributes.FirstOrDefault()?.AttributeType == typeof(HttpTriggerAttribute));

                        return isValid;
                    }));


            // Checks Required:
            // 1. 'CosmosODataQueryAttribute' must be registered with an HTTP function that has a 'GET' method
            // 2. 'CosmosODataQueryAttribute' 
            foreach (var function in functions)
            {
                var modelBuilder = new ODataConventionModelBuilder();

                // Let's get the function name
                var functionName = (string)function.CustomAttributes
                    .FirstOrDefault(x=>x.AttributeType == typeof(FunctionNameAttribute))
                    .ConstructorArguments
                    .First()
                    .Value;
                //
                var parameters = function.GetParameters();

                var httpODataBindingParameters = parameters
                    .Where(x => x.CustomAttributes.Any(x => x.AttributeType == typeof(CosmosBindingAttribute)));

                if (httpODataBindingParameters.Any())
                {
                    // Get's the HttpTrigger Attribute for the current function
                    var httpTriggerAttribute = parameters
                        .Select(x => x.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(HttpTriggerAttribute)))
                        .First();

                    // Get's the HTTP Methods: 'get', 'post', 'put', etc.,
                    var httpODataAttributeMethod = (httpTriggerAttribute.ConstructorArguments
                        .FirstOrDefault(x => x.ArgumentType == typeof(string[])).Value as IEnumerable)
                        .Cast<CustomAttributeTypedArgument>()
                        .Select(x => x.Value as string);

                    // Get's the Route of the API
                    var httpODataAttributeRoute = (string)httpTriggerAttribute.NamedArguments
                        .FirstOrDefault(x => x.MemberName == "Route").TypedValue.Value;

                    //var httpTriggerAttributeRoute = httpTriggerAttribute.CustomAttributes.FirstOrDefault(x=>x.AttributeType.))
                    var httpODataQueryAttribute = parameters
                        .Select(x => x.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(HttpTriggerAttribute)))
                        .First();

                    options.Add(functionName, new CosmosODataBindingOptions()
                    {
                        Method = httpODataAttributeMethod.First(),
                        Route = httpODataAttributeRoute
                    });
                }
                else
                {
                    // Let's not bother reflecting on methods that do not contain the Cosmos Binding Attributes
                    continue;
                }
            }

            return builder;
        }
    }
}
