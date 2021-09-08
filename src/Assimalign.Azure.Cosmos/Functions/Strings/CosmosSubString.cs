using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosSubString : CosmosFunctions, ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosSubString() {  }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("start")]
        public int StringStart { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("length")]
        public int StringLength { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            expression = null;

            var arguments = CosmosUtility.GetArgumentExpressions(this.StringStart, this.StringLength);
            var methodinfo = CosmosUtility.GetSubStringMethod();

            // Let's Check if the a property, field, or method expression is being passed through
            // and is of type string
            if (parameter is MemberExpression member && member.Type == typeof(string))
            {
                expression = CosmosUtility.GetMethodExpression(parameter, methodinfo, arguments);
            }
            else if (parameter is MethodCallExpression method && method.Method.ReturnType == typeof(string))
            {
                expression = CosmosUtility.GetMethodExpression(parameter, methodinfo, arguments);
            }
            else
            {
                throw new CosmosInvalidMethodRequestException(
                     message: "The request under '$where' or '$filter' is invalid.",
                     source: "The method '$substring' cannot be used for the requested member or method.");
            }
            if (base.IsFunctionAvailable && base.CurrentFunction.TryGetExpression(expression, out var child))
            {
                expression = child;
            }

            if (null == expression)
                return false;

            return true;
        }
    }
}
