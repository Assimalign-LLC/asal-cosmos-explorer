using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos
{
    internal sealed class CosmosRepositoryDefault<T> : CosmosRepository<T>
        where T : class, new()
    { 
        public CosmosRepositoryDefault(CosmosOptions options) : base(options) {  }


    }
}
