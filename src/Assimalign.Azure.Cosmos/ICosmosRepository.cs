using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Utilities;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICosmosRepository<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        CosmosClient Client { get; }

        /// <summary>
        /// Get the current container for this implementation
        /// </summary>
        Container Container { get;  }

        /// <summary>
        /// 
        /// </summary>
        IOrderedQueryable<T> Queryable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partition">The Partition in which to query item.</param>
        /// <returns></returns>
        Task<CosmosResponse<T>> GetItemAsync(string id, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<CosmosResponse<T>> GetItemsAsync(ICosmosQuery<T> query = null, CancellationToken cancellation = default);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <returns></returns>
        Task<CosmosResponse<T>> CreateItemAsync(T item, Expression<Func<T, object>> partition = null, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="partition"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosBulkResponse> CreateItemsAsync(IEnumerable<T> items, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partition"></param>
        /// <returns></returns>
        Task<CosmosResponse<T>> DeleteItemAsync(string id, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="partition"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosBulkResponse> DeleteItemsAsync(IReadOnlyList<string> ids, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosResponse<T>> UpsertItemAsync(T item, object partition = null, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosResponse<T>> PatchItemAsync(T item, object partition, string id, CosmosPatcherOptions options = null, CancellationToken cancellation = default);
    }
}
