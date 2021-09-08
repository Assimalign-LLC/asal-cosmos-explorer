using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CosmosAuthorizationPolicy
    {
        /// <summary>
        /// Don't want to allow access to dirty rats trying to access good Ole honest people's data.
        /// </summary>
        protected CosmosAuthorizationPolicy() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        public CosmosAuthorizationPolicy(string policy)
        {
            this.Policy = policy;
        }


        /// <summary>
        /// A Unique name for the policy.
        /// </summary>
        /// <remarks>
        /// If the policy name is the same as another policy then an exception will occur. Policies 
        /// should be unique to permission that are being accessed.
        /// </remarks>
        public string Policy { get; }

    }
}
