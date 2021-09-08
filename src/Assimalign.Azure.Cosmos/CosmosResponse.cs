using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Assimalign.Azure.Cosmos
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CosmosResponse<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        internal CosmosQuery<T> Query { get; set; }

        /// <summary>
        /// The defined status code for HTTP.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The item count returned from query request.
        /// </summary>
        public long Count => Items.LongCount();

        /// <summary>
        /// The collection of items for a particular container.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// The SQL query conversion used to query the collection.
        /// </summary>
        public string ItemsQuery { get; set; }


        /// <summary>
        /// The execution stats for the query request.
        /// </summary>
        public CosmosExecutionStats Stats { get; set; }
    }
}
