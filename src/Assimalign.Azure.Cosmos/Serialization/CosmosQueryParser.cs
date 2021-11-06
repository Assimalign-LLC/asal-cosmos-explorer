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

namespace Assimalign.Azure.Cosmos.Serialization
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Utilities;
    using Microsoft.Azure.WebJobs.Host.Bindings;

    /// <summary>
    /// 
    /// </summary>
    public abstract class CosmosQueryParser
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public virtual Task<ICosmosQuery<T>> ParseAsync<T>(HttpContext httpContext, ValueBindingContext bindingContext)
            where T : class, new()
        {
            return Task.Run<ICosmosQuery<T>>(() => 
            { 
                var request = httpContext.Request;
                var response = httpContext.Response;
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
                        
                        using (StreamReader reader = new StreamReader(request.Body))
                        {
                            return JsonSerializer.Deserialize<CosmosQuery<T>>(reader.ReadToEnd(), options);
                        }
                    }
                    else
                    {
                        // TODO: Invalid Content Type. Only JSON Supported
                        throw new Exception("");
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
        }
    }
}
