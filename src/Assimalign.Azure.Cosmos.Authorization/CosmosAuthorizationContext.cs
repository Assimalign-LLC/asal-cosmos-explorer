using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
    public class CosmosAuthorizationContext
    {

        private IEnumerable<ICosmosAuthorizationPolicy> _policies;

        public CosmosAuthorizationContext(IEnumerable<ICosmosAuthorizationPolicy> policies)
        {
            _policies = policies;
        }




    }

}
