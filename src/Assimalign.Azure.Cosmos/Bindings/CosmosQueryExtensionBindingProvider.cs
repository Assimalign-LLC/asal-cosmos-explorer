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


namespace Assimalign.Azure.Cosmos.Bindings
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Utilities;

    internal sealed class CosmosQueryExtensionBindingProvider<T> : IExtensionConfigProvider
        where T : class, new()
    {
        private readonly ICosmosRepository<T> repository;
        private readonly IHttpContextAccessor httpContextAccessor;


        public CosmosQueryExtensionBindingProvider(
            ICosmosRepository<T> repository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
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


        //private IDictionary<string, Type> GetCollectionBreakdown(Type type)
        //{
        //    var breakdown = new Dictionary<string, Type>();
        //    var properties = type.GetProperties().Where(x=>x.CanRead && x.CanWrite);

        //    foreach(var property in properties)
        //    {
        //        var propertyName = property.Name;

        //        if (property.PropertyType.IsSystemValueType(out var valueType))
        //        {
        //            breakdown.Add(propertyName, valueType)
        //        }
        //        else if (property.PropertyType.IsObjectType())
        //        {
        //            foreach(var child in )
        //        }

        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void BindCosmosQuery(ExtensionConfigContext context) =>
            context.AddBindingRule<CosmosQueryAttribute>()
                .BindToInput(builder =>
                {
                    // Need to figure out how to return a response without
                    // exception or internal nuget package errors hitting function
                    var request = httpContextAccessor.HttpContext.Request;
                    var response = httpContextAccessor.HttpContext.Response;
                    var options = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        AllowTrailingCommas = true,
                        IgnoreNullValues = true,
                    };

                    try
                    {
                        if (request.ContentType.Contains("application/json"))
                        {
                            // TODO: Invalid Content Type. Only JSON Supported
                        }
                        if (request.Query.TryGetValue(builder.QueryParameter, out var json))
                        {
                            return JsonSerializer.Deserialize<CosmosQuery<T>>(json, options);
                        }
                        else
                        {
                            using (StreamReader reader = new StreamReader(request.Body))
                            {
                                return JsonSerializer.Deserialize<CosmosQuery<T>>(reader.ReadToEnd(), options);
                            }
                        }
                    }
                    catch (CosmosQueryException exception)
                    {
                        if (response.Body.CanWrite)
                        {
                            response.ContentType = "application/json";
                            response.StatusCode = 400;

                            JsonSerializer.SerializeAsync(response.Body, new 
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Errors = exception.GetErrors()
                            }, options).GetAwaiter().GetResult();
                        }
                        throw exception;
                    }
                    catch (JsonException exception)
                    {
                        if (response.Body.CanWrite)
                        {
                            var error = new CosmosInvalidQueryRequestException(
                                message: "JSON Deserialization Error",
                                source: "The HTTP request body was invalid.");

                            response.ContentType = "application/json";
                            response.StatusCode = 400;

                            JsonSerializer.SerializeAsync(response.Body, new
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Errors = error.GetErrors()
                            }, options).GetAwaiter().GetResult();
                        }
                        throw exception;
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                });


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void BindCosmosRepository(ExtensionConfigContext context) =>
             context.AddBindingRule<CosmosQueryAttribute>()
                .BindToInput<ICosmosRepository<T>>(builder => repository);
    }
}
