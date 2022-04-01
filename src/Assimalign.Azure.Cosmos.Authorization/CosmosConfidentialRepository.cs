using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
    public sealed class CosmosConfidentialRepository<T> : CosmosRepository<T>, ICosmosConfidentialRepository<T>
        where T : class, new()
    {
        private readonly CosmosAuthorizationOptions<T> _securityOptions;


        public CosmosConfidentialRepository(CosmosAuthorizationOptions<T> options) 
            : base (new CosmosRepositoryOptions()
            {
                Database = options.Database,
                Container = options.Container,
                Connection = options.Connection
            })
        {
            
            


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public Task<CosmosCollectionResponse<T>> GetAuthorizedItemsAsync(CosmosQuery<T>? query, ClaimsPrincipal claimsPrincipal)
        {
            return Task.Run(async () =>
            {
                return new CosmosCollectionResponse<T>();
            });
        }
    }
}
