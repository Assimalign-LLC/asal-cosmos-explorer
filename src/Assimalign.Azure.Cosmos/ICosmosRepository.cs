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
        Task<CosmosItemResponse<T>> GetItemAsync(string id, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosItemResponse<T>> CreateItemAsync(T item, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <returns></returns>
        Task<CosmosItemResponse<T>> CreateItemAsync(T item, Func<T, object> partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partition"></param>
        /// <returns></returns>
        Task<CosmosItemResponse<T>> DeleteItemAsync(string id, object partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosItemResponse<T>> UpsertItemAsync(T item, object partition = null, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosItemResponse<T>> UpsertItemAsync(T item, Func<T, object> partition, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        //Task<CosmosCollectionResponse<T>> PatchItemAsync(T item, object partition, string id, CosmosPatcherOptions options = null, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosBulkResponse<T>> BatchItemsAsync(CosmosBulkRequest<T> requests, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CosmosCollectionResponse<T>> QueryItemsAsync(ICosmosQuery<T> query = null, CancellationToken cancellation = default);
    }
}
