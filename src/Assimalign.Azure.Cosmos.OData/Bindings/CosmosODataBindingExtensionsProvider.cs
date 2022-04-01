
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.UriParser;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Assimalign.Azure.Cosmos.OData.Bindings
{
    [Extension("CosmosODataExtension")]
    internal sealed class CosmosODataBindingExtensionsProvider<T> : IExtensionConfigProvider
        where T : class, new()
    {
        private readonly CosmosODataBindingOptions<T> bindingOptions;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICosmosRepository<T> repository;

        public CosmosODataBindingExtensionsProvider(
            CosmosODataBindingOptions<T> bindingOptions,
            ICosmosRepository<T> repository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.bindingOptions = bindingOptions;
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }



        public void Initialize(ExtensionConfigContext context)
        {
            BindCosmosODataQuery(context);
            BindCosmosODataRepository(context);
        }

        private void BindCosmosODataQuery(ExtensionConfigContext context) =>
            context.AddBindingRule<CosmosODataQueryAttribute>()
                .BindToInput<ICosmosQuery<T>>(attribute =>
                {
                    var request = httpContextAccessor.HttpContext.Request;

                    if (request.Method.Equals("GET"))
                    {
                        var query = new CosmosQuery<T>();
                        var root = new Uri(request.PathBase.Value);
                        var path = new Uri(request.Path.Value);

                        if (!request.Path.Value.Equals(bindingOptions.Route))
                        {
                            throw new Exception("OData Path does not match route");
                        }

                        var parser = new ODataUriParser(bindingOptions.Model, root, path);
                        var orderBy = parser.ParseOrderBy();


                        return query;

                    }
                    else
                    {
                        throw new Exception("Invalid Request");
                    }
                });

        private void BindCosmosODataRepository(ExtensionConfigContext context) =>
            context.AddBindingRule<CosmosODataRepositoryAttribute>()
                .BindToInput<ICosmosRepository<T>>(attribute =>
                {
                    return repository;
                });
    }
}
