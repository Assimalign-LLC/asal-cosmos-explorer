using System;
using System.Linq;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Clauses;

    internal static class CosmosQueryExtensions
    {

        /// <summary>
        /// Builds the Linq Expression from the Query passed through the CosmosQuery object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
		public static IOrderedQueryable<T> CreateQuery<T>(this IOrderedQueryable<T> source, CosmosQuery<T> query) =>
            query.BuildQuery(source);

    }
}
