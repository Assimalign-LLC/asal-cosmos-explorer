using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.OData.Edm;

namespace Assimalign.Azure.Cosmos.OData
{
    using Assimalign.Azure.Cosmos.Serialization;

    internal sealed class CosmosODataQueryParser : CosmosSerializer
    {

        private readonly CosmosODataOptions odataOptions;


        public CosmosODataQueryParser(CosmosODataOptions odataOptions)
        {
            this.odataOptions = odataOptions;
        }

        public override Task<ICosmosQuery<T>> ParseAsync<T>(HttpContext httpContext, ValueBindingContext bindingContext)
        {
            var functionName = bindingContext.FunctionContext.MethodName;
            var httpRequest = httpContext.Request;
            
            var temp = httpContext.Request.Query.ToString();


            if (odataOptions.BindingInfo.TryGetValue(functionName, out var bindingOptions)) 
            {
                //httpRequest.Method
                
            }

            return base.ParseAsync<T>(httpContext, bindingContext);
        }

    }
}
