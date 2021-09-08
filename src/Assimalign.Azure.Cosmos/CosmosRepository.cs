using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Exceptions;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CosmosRepository<T> : ICosmosRepository<T>
        where T : class, new()
    {
        private readonly CosmosClient client;
        private readonly Container container;
        private readonly CosmosOptions options;
        private static ConcurrentDictionary<string, IQueryable<T>> queryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public CosmosRepository(CosmosOptions options)
        {
            this.options = options;

            if (this.options.Credentials is null)
            {
                this.client = new CosmosClient(options.Connection, options.ClientOptions);
            }
            else
            {
                this.client = new CosmosClient(options.Uri, options.Credentials, options.ClientOptions);
            }

            this.container = this.client.GetContainer(options.Database, options.Container);
            queryCache = new ConcurrentDictionary<string, IQueryable<T>>();
        }

        /// <summary>
        /// Get's an instance of the current client
        /// </summary>
        public CosmosClient Client => this.client;

        /// <summary>
        /// 
        /// </summary>
        public Container Container => this.container;

        /// <summary>
        /// Gets an Item by id within a particular partition
        /// </summary>
        /// <param name="id">The items unique id. (NOTE: Not uncommon to have the same unique ids in different partitions.)</param>
        /// <param name="partition">The Partition in which to query the unique item.</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual Task<CosmosResponse<T>> GetItemAsync(string id, object partition, CancellationToken cancellation = default)
        {
            var partitionKey = GetPartitionKey(partition) ?? 
                throw new ArgumentNullException("Partition Key is Required when reading item by id within Cosmos Db.");

            return Task.Run(async () =>
            {
                var itemResponse = await container.ReadItemAsync<T>(id, partitionKey, null, cancellation);
                var cosmosResponse = new CosmosResponse<T>()
                {
                    StatusCode = itemResponse.StatusCode,
                    Items = new[] { itemResponse.Resource },
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)itemResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds
                    }
                };
                return cosmosResponse;
            });
        }

        /// <summary>
        /// Get items based on requested query. If query is null all items will be returned
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual Task<CosmosResponse<T>> GetItemsAsync(ICosmosQuery<T> query = null, CancellationToken cancellation = default)
        {
            query ??= new CosmosQuery<T>();

            return Task.Run(async () =>
            {
                long elapsedTime = 0;
                var resources = new List<T>();
                var linqQuery = container.GetItemLinqQueryable<T>().CreateQuery(query as CosmosQuery<T>);
                var linqDefinition = linqQuery.ToQueryDefinition();

                using (FeedIterator<T> iterator = linqQuery.ToFeedIterator())
                {
                    while (iterator.HasMoreResults)
                    {
                        var iteratorResponse = await iterator.ReadNextAsync(cancellation);
                        foreach (var resource in iteratorResponse)
                        {
                            resources.Add(resource);
                        }
                        elapsedTime += (long)iteratorResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds;
                    }
                }

                var cosmosResponse = new CosmosResponse<T>()
                {
                    Items = resources,
                    ItemsQuery = linqDefinition.QueryText.Replace('"', ' ').Replace("[ ", "['").Replace(" ]", "']"),
                    StatusCode = HttpStatusCode.OK,
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = elapsedTime
                    }
                };

                return cosmosResponse;
            });
        }

        /// <summary>
        /// Creates a single item within the specified container.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual Task<CosmosResponse<T>> CreateItemAsync(T item, Expression<Func<T, object>> partition = null, CancellationToken cancellation = default)
        {
            var value = partition.Compile().Invoke(item);
            var partitionKey = GetPartitionKey(value);

            return Task.Run(async () =>
            {
                var itemResponse = await container.CreateItemAsync(item, partitionKey, null, cancellation);
                var cosmosResponse = new CosmosResponse<T>()
                {
                    Items = new[] { itemResponse.Resource },
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)itemResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds
                    }
                };

                return cosmosResponse;
            });
        }

        /// <summary>
        /// Creates a collection of items as a batch request within the specified partition key.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="partition">The separation of items within the collection.</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual Task<CosmosBulkResponse> CreateItemsAsync(IEnumerable<T> items, object partition, CancellationToken cancellation = default)
        {
            var partitionKey = GetPartitionKey(partition) ??
                    throw new ArgumentNullException("Partition Key must be provided when creating multiple items in Cosmos Db.");

            return Task.Run(async () =>
            {
                var transaction = container.CreateTransactionalBatch(partitionKey);

                foreach(var item in items)
                {
                    transaction.CreateItem<T>(item);
                }

                var transactionResponse = await transaction.ExecuteAsync(cancellation);

                return new CosmosBulkResponse()
                {
                    StatusCode = transactionResponse.StatusCode,
                    ErrorMessage = transactionResponse.ErrorMessage,
                    Count = transactionResponse.Count,
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)transactionResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds
                    }
                };
            });
        }

        /// <summary>
        /// Deletes an item from the collection within the specified partition
        /// </summary>
        /// <param name="id">The items unique id. (NOTE: Not uncommon to have the same unique ids in different partitions.)</param>
        /// <param name="partition">The separation of items within the collection.</param>
        /// <returns></returns>
        public virtual Task<CosmosResponse<T>> DeleteItemAsync(string id, object partition, CancellationToken cancellation = default)
        {
            var partitionKey = GetPartitionKey(partition) ?? 
                throw new ArgumentNullException("Partition must be provided when Deleting an item from Cosmos Db.");

            return Task.Run(async () =>
            {
                var itemResponse = await container.DeleteItemAsync<T>(id, partitionKey,null, cancellation);
                return new CosmosResponse<T>()
                {
                    Items = new[] { itemResponse.Resource },
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)itemResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds
                    }
                };
            });
        }

        /// <summary>
        /// Deletes a collection of items within the specified container for the requested partition.
        /// </summary>
        /// <returns></returns>
        public virtual Task<CosmosBulkResponse> DeleteItemsAsync(IReadOnlyList<string> ids, object partition, CancellationToken cancellation = default)
        {
            var partitionKey = GetPartitionKey(partition) ??
                throw new ArgumentNullException("Partition must be provided when Deleting an item from Cosmos Db.");

            return Task.Run(async () =>
            {
                var transaction = container.CreateTransactionalBatch(partitionKey);
                foreach(var id in ids)
                {
                    transaction.DeleteItem(id);
                }

                var transactionResponse = await transaction.ExecuteAsync(cancellation);
                return new CosmosBulkResponse()
                {
                    StatusCode = transactionResponse.StatusCode,
                    ErrorMessage = transactionResponse.ErrorMessage,
                    Count = transactionResponse.Count,
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)transactionResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds
                    }
                };
            });
        }

        /// <summary>
        /// Creates or Updates an existing item within cosmos container.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="partition">The separation of items within the collection.</param>
        /// <returns></returns>
        public virtual Task<CosmosResponse<T>> UpsertItemAsync(T item, object partition = null, CancellationToken cancellation = default)
        {
            var partitionKey = GetPartitionKey(partition);
            return Task.Run(async () =>
            {
                var itemResponse = await container.UpsertItemAsync(item, partitionKey, null, cancellation);
                var cosmosResponse = new CosmosResponse<T>()
                {
                    Items = new[] { itemResponse.Resource },
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)itemResponse.Diagnostics
                            .GetClientElapsedTime()
                            .TotalMilliseconds
                    }
                };

                return cosmosResponse;
            });
        }


        /// <summary>
        /// Will update the properties of a given object without effecting any others
        /// </summary>
        /// <remarks>
        /// Patching is a very helpful method that reduces client side requests to update data, but 
        /// should be used CAREFULLY as it can cause irreparable effects on the data if not understood what
        /// is being updated.
        /// </remarks>
        /// <param name="item"></param>
        /// <param name="partition"></param>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual Task<CosmosResponse<T>> PatchItemAsync(T item, object partition, string id, CosmosPatcherOptions options = null, CancellationToken cancellation = default)
        {
            var partitionKey = GetPartitionKey(partition) ??
                throw new ArgumentNullException("Partition Key is Required when reading item by id within Cosmos Db.");

            return Task.Run(async () =>
            {
                var itemCurrent = await container.ReadItemAsync<T>(id, partitionKey, null, cancellation);
                var itemMerge = CosmosPatcher.Merge<T>(itemCurrent.Resource, item, options);
                var itemPatch = await container.UpsertItemAsync(itemMerge, partitionKey);

                var cosmosResponse = new CosmosResponse<T>()
                {
                    Items = new[] { itemPatch.Resource },
                    Stats = new CosmosExecutionStats()
                    {
                        EllapsedMilliseconds = (long)itemPatch.Diagnostics
                           .GetClientElapsedTime()
                           .TotalMilliseconds
                    }
                };

                return cosmosResponse;

            });
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="required"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private PartitionKey? GetPartitionKey(object partition)
        {
            if (partition != null)
            {
                if (partition is string partitionString)
                {
                    return new PartitionKey(partitionString);
                }
                if (partition is bool partitionBoolean)
                {
                    return new PartitionKey(partitionBoolean);
                }
                if (partition is double partitionDouble)
                {
                    return new PartitionKey(partitionDouble);
                }
                else
                {
                    throw new CosmosInvalidPartitionKeyException();
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            queryCache.Clear();
            client.Dispose();
        }        
    }
}
