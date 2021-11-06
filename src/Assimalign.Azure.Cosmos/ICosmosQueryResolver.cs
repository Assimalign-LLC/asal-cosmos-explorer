using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos
{
    /// <summary>
    /// Resolves child fields for repository
    /// </summary>
    public interface ICosmosQueryResolver
    {
        
    }

    public interface ICosmosQueryResolver<in TParent, TChild> : ICosmosQueryResolver
    {



        public Task<TChild> ResolveAsync(TParent parent, ICosmosQuery<TChild> query);
    }
}
