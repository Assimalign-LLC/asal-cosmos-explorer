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
    public sealed class CosmosStringEquals : ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosStringEquals() { }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }


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

            var comparison = CaseSensitive ? 
                StringComparison.InvariantCulture : 
                StringComparison.InvariantCultureIgnoreCase;
            var arguments = CosmosUtility.GetArgumentExpressions(this.Value, comparison);
            var methodinfo = CosmosUtility.GetStringEqualsMethod();

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
                     source: "The method '$stringEquals' cannot be used for the requested member or method.");
            }


            if (null == expression)
                return false;

            return true;
        }
    }
}
