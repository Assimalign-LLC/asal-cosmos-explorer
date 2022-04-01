using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.OData.Bindings
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CosmosODataQueryAttribute : Attribute
    {
        public CosmosODataQueryAttribute()
        {

        }
    }
}
