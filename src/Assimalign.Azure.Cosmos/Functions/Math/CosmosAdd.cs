using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Serialization;
    using Assimalign.Azure.Cosmos.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosAdd : CosmosFunctions, ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosAdd() { }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$property")]
        public string Property { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$properties")]
        public IEnumerable<CosmosAdd> Properties { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("value")]
        [JsonConverter(typeof(CosmosQueryObjectConverter))]
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
            Expression addition;

            if (parameter is BinaryExpression binary && binary.Type.IsNumericType(out var numericType0))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (binary.Type.IsNullable(numericType0))
                {
                    addition = Expression.Convert(binary, numericType0);
                }
                else
                {
                    addition = binary;
                }

                value = Convert.ChangeType(value, addition.Type);
                expression = Expression.Add(addition, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is MemberExpression member && member.Type.IsNumericType(out var numericType1))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (member.Type.IsNullable(numericType1))
                {
                    addition = Expression.Convert(member, numericType1);
                }
                else
                {
                    addition = member;
                }

                value = Convert.ChangeType(value, addition.Type);
                expression = Expression.Add(addition, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is MethodCallExpression method)
            {
                value = Convert.ChangeType(value, method.Type);
                expression = Expression.Add(method, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is ParameterExpression param)
            {
                foreach (var property in Properties)
                {
                    addition = CosmosUtility.GetMemberExpression(property.Property, param);

                    if (addition.Type.IsNumericType(out var numericType2, false) && 
                        addition.Type.IsNullable(numericType2))
                    {
                        addition = Expression.Convert(addition, numericType2);
                    }
                    else
                    {
                        throw new CosmosInvalidMethodRequestException(
                             message: "The request under '$where' or '$filter' is invalid.",
                             source: "The method '$add' cannot be used for the requested member or method.");
                    }
                    if (expression == null)
                    {
                        expression = addition;
                    }
                    else
                    {
                        expression = Expression.Add(expression, addition);
                    }
                }
            }
            else
            {
                throw new CosmosInvalidMethodRequestException(
                   message: "The request under '$where' or '$filter' is invalid.",
                   source: "The method '$add' cannot be used for the requested member or method.");
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
