using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Edm;

namespace Assimalign.Azure.Cosmos.OData
{
    internal sealed class CosmosODataOptions
    {
        internal readonly IDictionary<string, CosmosODataBindingOptions> bindingInfo;

        public CosmosODataOptions()
        {
            bindingInfo = new Dictionary<string, CosmosODataBindingOptions>();
        }


        public IReadOnlyDictionary<string, CosmosODataBindingOptions> BindingInfo => 
            bindingInfo.ToImmutableDictionary();


        internal void Add(string functionName, CosmosODataBindingOptions options)
        {
            bindingInfo.Add(functionName, options);
        }
    }
}
