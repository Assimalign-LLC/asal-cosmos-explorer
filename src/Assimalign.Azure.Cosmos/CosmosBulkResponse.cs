using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos
{
    public sealed class CosmosBulkResponse<T>
        where T : class, new()
    {

        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The number of effected items within the the bulk operation.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CosmosBulkTransaction<T>> Responses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CosmosExecutionStats Stats { get; set; } = new CosmosExecutionStats();
    }
}
