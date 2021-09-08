using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Assimalign.Azure.Cosmos.Functions
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICosmosFunction
    {

        /// <summary>
        /// A method for chaining one function to the next
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool TryGetExpression(Expression parameter, out Expression expression);
    }
}
