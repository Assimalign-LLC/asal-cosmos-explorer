using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Clauses;

    /// <summary>
    /// Represents a query object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CosmosQuery<T> : ICosmosQuery<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosQuery()
        {
            QueryId = Guid.NewGuid();
        }


        /// <summary>
        /// A unique identifier for the query request.
        /// </summary>
        public Guid QueryId { get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$select")]
        public CosmosSelect Select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$where")]
        public CosmosWhere Where { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$orderBy")]
        public CosmosOrderBy OrderBy { get; set; }

        /// <summary>
        /// A object representing the paging request for the 
        /// </summary>
        [JsonPropertyName("$paging")]
        public CosmosPaging Paging { get; set; } = new CosmosPaging();


        /// <summary>
        /// Builds the query collection
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> BuildQuery(IOrderedQueryable<T> source)
        {
            if (this.Where is not null)
            {
                source = this.Where.CreateQuery<T>(source);
            }
            if (this.OrderBy is not null)
            {
                source = this.OrderBy.CreateQuery<T>(source);
            }
            if (this.Paging is not null)
            {
                source = this.Paging.CreateQuery<T>(source);
            }
            if (this.Select is not null)
            {
                source = this.Select.CreateQuery<T>(source);
            }

            return source;
        }
    }
}