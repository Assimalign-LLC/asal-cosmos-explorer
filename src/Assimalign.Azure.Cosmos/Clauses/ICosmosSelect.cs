using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICosmosSelect : ICosmosField, ICosmosClause
    {
        /// <summary>
        /// 
        /// </summary>
        CosmosSelectFilter FilterBy { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSelect"></typeparam>
    public interface ICosmosSelect<TSelect> : ICosmosSelect
        where TSelect : ICosmosSelect
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<TSelect> Properties { get; set; }
    }

}
