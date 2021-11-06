using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICosmosClause
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        IOrderedQueryable<T> CreateQuery<T>(IQueryable<T> source);
    }
}
