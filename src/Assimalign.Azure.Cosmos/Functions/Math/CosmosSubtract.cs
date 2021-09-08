using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Serialization;
    using Assimalign.Azure.Cosmos.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosSubtract : CosmosFunctions, ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosSubtract() {  }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$property")]
        public string Property { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$properties")]
        public IEnumerable<CosmosSubtract> Properties { get; set; }

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
            Object value = this.Value;
            Expression subtraction;

            if (parameter is BinaryExpression binary && binary.Type.IsNumericType(out var numericType0))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (binary.Type.IsNullable(numericType0))
                {
                    subtraction = Expression.Convert(binary, numericType0);
                }
                else
                {
                    subtraction = binary;
                }

                value = Convert.ChangeType(value, subtraction.Type);
                expression = Expression.Subtract(subtraction, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is MemberExpression member && member.Type.IsNumericType(out var numericType1))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (member.Type.IsNullable(numericType1))
                {
                    subtraction = Expression.Convert(member, numericType1);
                }
                else
                {
                    subtraction = member;
                }

                value = Convert.ChangeType(value, subtraction.Type);
                expression = Expression.Subtract(subtraction, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is MethodCallExpression method && method.Method.ReturnType.IsNumericType())
            {
                value = Convert.ChangeType(value, method.Type);
                expression = Expression.Subtract(method, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is ParameterExpression param)
            {
                foreach (var property in Properties)
                {
                    subtraction = CosmosUtility.GetMemberExpression(property.Property, param);

                    if (subtraction.Type.IsNumericType(out var numericType2, false) &&
                        subtraction.Type.IsNullable(numericType2))
                    {
                        subtraction = Expression.Convert(subtraction, numericType2);
                    }
                    else
                    {
                        // If one property is not numeric than subtracting is invalid
                        throw new CosmosInvalidMethodRequestException(
                           message: "The request under '$where' or '$filter' is invalid.",
                           source: "The method '$subtract' cannot be used for the requested member or method.");
                    }
                    if (expression == null)
                    {
                        expression = subtraction;
                    }
                    else
                    {
                        expression = Expression.Subtract(expression, subtraction);
                    }
                }
            }
            else
            {
                throw new CosmosInvalidMethodRequestException(
                   message: "The request under '$where' or '$filter' is invalid.",
                   source: "The method '$subtract' cannot be used for the requested member or method.");
            }

            if (base.IsFunctionAvailable && 
                base.CurrentFunction.TryGetExpression(expression, out var nested))
            {
                expression = nested;
            }

            if (expression != null)
                return true;

            return false;
        }
    }
}
