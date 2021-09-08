using Assimalign.Azure.Cosmos.Exceptions;
using Assimalign.Azure.Cosmos.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Functions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosStringIn : ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("values")]
        public string[] Values { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("isSensitive")]
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            expression = null;

            // Might need to revisit how querying for values with IN
            if (parameter is MemberExpression member && member.Type == typeof(string))
            {
                foreach(var value in Values)
                {
                    var comparison = CaseSensitive ?
                        StringComparison.InvariantCulture :
                        StringComparison.InvariantCultureIgnoreCase;
                    var arguments = CosmosUtility.GetArgumentExpressions(value, comparison);
                    var methodinfo = CosmosUtility.GetStringEqualsMethod();
                    var method = CosmosUtility.GetMethodExpression(member, methodinfo, arguments);

                    if (expression is null)
                    {
                        expression = method;
                    }
                    else
                    {
                        expression = Expression.OrElse(expression, method);
                    }
                }
            }
            else
            {
                throw new CosmosInvalidMethodRequestException(
                     message: "The request under '$where' or '$filter' is invalid.",
                     source: "The method '$stringIn' cannot be used for the requested member or method.");
            }

            if (expression is null)
                return false;

            return true;

        }
    }
}
