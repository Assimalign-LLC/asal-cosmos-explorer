using System;
using Azure.Core;
using Microsoft.Azure.Cosmos;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosRepositoryOptions
    {
        private TokenCredential cosmosClientCredential;
        private CosmosClientOptions cosmosClientOptions;

        /// <summary>
        /// 
        /// </summary>
        public CosmosRepositoryOptions()
        {
            // Let's set default options for smoother use
            cosmosClientOptions = new CosmosClientOptions()
            {
                AllowBulkExecution = true,
                SerializerOptions = new CosmosSerializationOptions()
                {
                     PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
        }

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
        public TokenCredential ClientCredentials => cosmosClientCredential;

        /// <summary>
        /// CosmosClient Options to be passed to the CosmosRepository
        /// </summary>
        public CosmosClientOptions ClientOptions => cosmosClientOptions;

        ///// <summary>
        ///// Parses the query request for incoming queries.
        ///// </summary>
        //public CosmosSerializer Serializer { get; set; }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public void AddCosmosClientOptions(CosmosClientOptions options) =>
            cosmosClientOptions = options;

        /// <summary>
        /// Configure the underlying CosmosClient options.
        /// </summary>
        /// <param name="configure"></param>
        public void AddCosmosClientOptions(Action<CosmosClientOptions> configure) =>
             configure.Invoke(cosmosClientOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        public void AddCosmosTokenCredentials(TokenCredential credentials) =>
            cosmosClientCredential = credentials;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CosmosRepositoryOptions RegisterResolver(ICosmosQueryResolver resolver)
        {
            throw new NotImplementedException("");
        }
    }
}
