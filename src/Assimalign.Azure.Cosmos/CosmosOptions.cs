using System;
using Azure.Core;
using Microsoft.Azure.Cosmos;

namespace Assimalign.Azure.Cosmos
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosOptions
    {

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
        /// 
        /// </summary>
        [Obsolete("Future State: Allow users to be able to create their own query syntax.")]
        public CosmosQueryParser QueryParser { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TokenCredential Credentials { get; set; }


        /// <summary>
        /// CosmosClient Options to be passed to the CosmosRepository
        /// </summary>
        public CosmosClientOptions ClientOptions { get; set; }

        /// <summary>
        /// Configure the underlying CosmosClient options.
        /// </summary>
        /// <param name="configure"></param>
        public void AddClientOptions(Action<CosmosClientOptions> configure)
        {
            ClientOptions = new CosmosClientOptions();
            configure.Invoke(ClientOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        public void AddTokenCredentials(TokenCredential credentials) =>
            Credentials = credentials;
    }
}
