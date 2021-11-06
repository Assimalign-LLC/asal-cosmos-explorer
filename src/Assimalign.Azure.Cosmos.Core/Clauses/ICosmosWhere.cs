using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICosmosWhere : ICosmosField, ICosmosClause
    {
        /// <summary>
        /// 
        /// </summary>
        OperatorType Operator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ValueTypeFunctions Function { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TWhere"></typeparam>
    public interface ICosmosWhere<TWhere> : ICosmosWhere
        where TWhere : ICosmosWhere
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<TWhere> Or { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<TWhere> And { get; set; }
    }
}
