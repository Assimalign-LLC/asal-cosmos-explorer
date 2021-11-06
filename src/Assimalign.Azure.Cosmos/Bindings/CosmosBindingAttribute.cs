using System;
using Microsoft.Azure.WebJobs.Description;

namespace Assimalign.Azure.Cosmos.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CosmosBindingAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryParameter">The parameter name to search for in the HTTP Request Query Parameters.</param>
        public CosmosBindingAttribute(string queryParameter = "query") =>
            this.QueryParameter = queryParameter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useAuthorization"></param>
        /// <param name="policy"></param>
        [Obsolete("Not ready for public consumption.")]
        public CosmosBindingAttribute(bool useAuthorization, string policy)
        {
            this.UseAuthorization = useAuthorization;
            this.Policy = policy;
        }

        /// <summary>
        /// The Query Parameter used for parsing Url
        /// </summary>
        public string QueryParameter { get; set; }


        /// <summary>
        /// Specifies whether the pipeline shoud 
        /// </summary>
        public bool UseAuthorization { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Policy { get; set; }

    }
}
