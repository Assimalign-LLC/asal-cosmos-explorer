using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
    public interface ICosmosConfidentialRepository<T> : ICosmosRepository<T>
        where T : class, new()
    {



        Task<CosmosCollectionResponse<T>> GetAuthorizedItemsAsync(CosmosQuery<T>? query, ClaimsPrincipal claimsPrincipal);



    }
}
