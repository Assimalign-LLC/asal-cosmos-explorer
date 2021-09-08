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
    public sealed class CosmosTrimEnd : CosmosFunctions, ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosTrimEnd() { }


        /// <summary>
        /// The Character to trim from the end of the String Value.
        /// </summary>
        [JsonPropertyName("trimchar")]
        [Obsolete("Not Currently Supported")]
        public char Character { get; set; } = ' ';


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            expression = null;

            var methodinfo = CosmosUtility.GetTrimEndMethod();

            // Let's Check if the a property, field, or method expression is being passed through
            // and is of type string
            if (parameter is MemberExpression member && member.Type == typeof(string))
            {
                expression = CosmosUtility.GetMethodExpression(parameter, methodinfo);
            }
            else if (parameter is MethodCallExpression method && method.Method.ReturnType == typeof(string))
            {
                expression = CosmosUtility.GetMethodExpression(parameter, methodinfo);
            }
            else
            {
                throw new CosmosInvalidMethodRequestException(
                     message: "The request under '$where' or '$filter' is invalid.",
                     source: "The method '$trimEnd' cannot be used for the requested member or method.");
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
