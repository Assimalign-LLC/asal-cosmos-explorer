using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.OData.Bindings
{
    [Binding]
    [ConnectionProvider(typeof(CosmosODataAccountAttribute))]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CosmosODataRepositoryAttribute : Attribute, IConnectionProvider
    {
        public CosmosODataRepositoryAttribute() { }

        public CosmosODataRepositoryAttribute(string database, string container)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Connection { get; set; }
    }
}
