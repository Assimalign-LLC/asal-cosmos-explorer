using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICosmosPaging : ICosmosClause
    {
        /// <summary>
        /// 
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int Count { get; set; }
    }
}
