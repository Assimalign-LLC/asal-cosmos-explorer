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
    public sealed class CosmosArrayExists : CosmosFunctions, ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosArrayExists() { }


        /// <summary>
        /// The Member of the object in the array to evaluate.
        /// </summary>
        [JsonPropertyName("member")]
        public string Member { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("value")]
        [JsonConverter(typeof(CosmosObjectConverter))]
        public object Value { get; set; }


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
                if (member.Type.IsSystemValueType())
                {
                    throw new Exception("The $exists Function is not supported for Value Types. Exp: 'string', 'number', 'dateTime' etc");
                }

                Type enumerableType;
                if (member.Type.IsTypedEnumerable(out var enumType))
                    enumerableType = enumType;

                else if (member.Type.IsArrayType(out var arrayType))
                    enumerableType = arrayType;

                else
                    throw new CosmosInvalidComparisonException("$Exists", member.Member.Name);

                ParameterExpression enumerableParameter = Expression.Parameter(enumerableType);
                Expression enumerableMember = Expression.Property(enumerableParameter, this.Member);

                if (base.IsFunctionAvailable && base.CurrentFunction.TryGetExpression(enumerableMember, out var child))
                {
                    enumerableMember = child;
                }


                var constant = Expression.Constant(Convert.ChangeType(this.Value, enumerableMember.Type));
                var evaluation = Expression.Equal(enumerableMember, constant);
                var lambda = Expression.Lambda(evaluation, enumerableParameter);

                expression = Expression.Call(
                    typeof(Enumerable),
                    "Any",
                    new Type[] { enumerableType },
                    member,
                    lambda);
            }

            if (expression == null)
                return false;

            return true;
        }
    }
}
