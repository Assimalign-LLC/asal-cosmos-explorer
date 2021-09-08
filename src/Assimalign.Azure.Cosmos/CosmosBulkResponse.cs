using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos
{
    public sealed class CosmosBulkResponse
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
        public CosmosExecutionStats Stats { get; set; } = new CosmosExecutionStats();
    }
}
