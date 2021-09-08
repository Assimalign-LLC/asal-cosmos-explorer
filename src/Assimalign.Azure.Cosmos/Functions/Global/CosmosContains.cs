using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosContains : ICosmosFunction
    {
        /// <summary>
        /// The value to search for in string.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonConverter(typeof(CosmosObjectConverter))]
        public object Value { get; set; }

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

            if (parameter is MemberExpression member)
            {
                Type enumerableType;
                Expression enumerableConstant;

                if (member.Type.IsTypedEnumerable(out var enumType))
                {
                    enumerableType = enumType;
                    enumerableConstant = Expression.Constant(Convert.ChangeType(this.Value, enumerableType));
                }
                else if (member.Type.IsArrayType(out var arrayType))
                {
                    enumerableType = arrayType;
                    enumerableConstant = Expression.Constant(Convert.ChangeType(this.Value, enumerableType));
                }
                else
                {
                    throw new CosmosInvalidComparisonException("$Contains", member.Member.Name);
                }

                // String Contains Method
                if (enumerableType == typeof(char))
                {
                    var comparison = CaseSensitive ? 
                        StringComparison.InvariantCulture : 
                        StringComparison.InvariantCultureIgnoreCase;
                    var arguments = CosmosUtility.GetArgumentExpressions(this.Value, comparison);
                    var method = CosmosUtility.GetContainsMethod();
                    expression = CosmosUtility.GetMethodExpression(parameter, method, arguments);
                }
                // Array Contains Method
                else
                {
                    expression = Expression.Call(
                        typeof(Enumerable),
                        "Contains",
                        new Type[] { enumerableType },
                        member,
                        enumerableConstant);
                }
            }

            if (null == expression)
                return false;

            return true;
        }
    }
}
