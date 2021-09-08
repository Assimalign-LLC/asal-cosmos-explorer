using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Clauses;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICosmosQuery<T> 
    {
        /// <summary>
        /// A unique identifier for the query request.
        /// </summary>
        Guid QueryId { get;  }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$select")]
        CosmosSelect Select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$where")]
        CosmosWhere Where { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$orderBy")]
        CosmosOrderBy OrderBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$paging")]
        CosmosPaging Paging { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IOrderedQueryable<T> BuildQuery(IOrderedQueryable<T> source);
    }
}
