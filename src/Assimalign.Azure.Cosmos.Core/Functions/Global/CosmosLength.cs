using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Utilities;
    

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosLength : CosmosFunctions, ICosmosFunction
    { 

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression<T>(Expression parameter, out Expression expression)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            expression = null;

            if (!Array.Exists(new[] { "array", "string" }, x => x == Type.ToLower()))
                throw new ArgumentNullException("Must Supply a valid Type: 'string' or 'array' for $Length function.");

            if (parameter is MemberExpression member)
            {
                Type enumerableType = null;
                if (member.Type.IsTypedEnumerable(out var enumType))
                {
                    enumerableType = enumType;
                }
                else if (member.Type.IsArrayType(out var arrayType))
                {
                    enumerableType = arrayType;
                }
                if (enumerableType is not null && (enumerableType == typeof(char) || enumerableType.IsClass))
                {
                    expression = Expression.Call(
                        typeof(Enumerable),
                        "Count",
                        new Type[] { enumerableType },
                        member);
                }
                else
                {
                    return false;
                }
            }

            if (base.IsFunctionAvailable && base.CurrentFunction.TryGetExpression(expression, out var child))
                expression = child;

            if (null == expression)
                return false;

            return true;
        }
    }
}
