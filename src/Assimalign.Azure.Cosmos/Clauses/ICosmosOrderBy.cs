using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICosmosOrderBy : ICosmosField, ICosmosClause
    {
        /// <summary>
        /// 
        /// </summary>
        SortType Sort { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOrderBy"></typeparam>
    public interface ICosmosOrderBy<TOrderBy> : ICosmosOrderBy 
        where TOrderBy : ICosmosOrderBy
    {
        /// <summary>
        /// 
        /// </summary>
        TOrderBy ThenBy { get; set; }
    }
}
