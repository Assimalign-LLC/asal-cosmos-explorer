using System;
using Azure.Core;
using Microsoft.Azure.Cosmos;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosOptions
    {
        private TokenCredential cosmosTokenCredential;
        private CosmosClientOptions cosmosClientOptions;

        /// <summary>
        /// The name of the database to connect to.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// The name of the container within the database to connect to.
        /// </summary>
        public string Container { get; set; }

        /// <summary>
        /// The Connection string to the Cosmos DB instance.
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// The Cosmos DB Endpoint.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Set the Azure Credentials to be used for negotiating authenticated connection to 
        /// Cosmos DB instance.
        /// </summary>
        public TokenCredential Credentials => cosmosTokenCredential;

        /// <summary>
        /// CosmosClient Options to be passed to the CosmosRepository
        /// </summary>
        public CosmosClientOptions ClientOptions => cosmosClientOptions;

        /// <summary>
        /// Parses the query request for incoming queries.
        /// </summary>
        public CosmosQueryParser QueryParser { get; set; }

        /// <summary>
        /// Configure the underlying CosmosClient options.
        /// </summary>
        /// <param name="configure"></param>
        public void AddClientOptions(Action<CosmosClientOptions> configure)
        {
            cosmosClientOptions = new CosmosClientOptions();
            configure.Invoke(cosmosClientOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        public void AddTokenCredentials(TokenCredential credentials) =>
            cosmosTokenCredential = credentials;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CosmosOptions RegisterResolver(ICosmosQueryResolver resolver)
        {
            throw new NotImplementedException("");
        }
    }
}
