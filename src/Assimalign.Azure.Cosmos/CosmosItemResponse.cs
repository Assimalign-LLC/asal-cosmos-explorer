using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos
{
    public sealed class CosmosItemResponse<T>
        where T : class, new()
    {
        /// <summary>
        /// The defined status code for HTTP.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The collection of items for a particular container.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// The execution stats for the query request.
        /// </summary>
        public CosmosExecutionStats Stats { get; set; }
    }
}
