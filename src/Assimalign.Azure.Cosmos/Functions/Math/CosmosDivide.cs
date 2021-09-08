
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Serialization;
    using Assimalign.Azure.Cosmos.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosDivide : CosmosFunctions, ICosmosFunction
    {

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$property")]
        public string Property { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$properties")]
        public IEnumerable<CosmosDivide> Properties { get; set; }

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
            Expression division;

            if (parameter is BinaryExpression binary && binary.Type.IsNumericType(out var numericType0))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (binary.Type.IsNullable(numericType0))
                {
                    division = Expression.Convert(binary, numericType0);
                }
                else
                {
                    division = binary;
                }

                value = Convert.ChangeType(value, division.Type);
                expression = Expression.Divide(division, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is MemberExpression member && member.Type.IsNumericType(out var numericType1))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (member.Type.IsNullable(numericType1))
                {
                    division = Expression.Convert(member, numericType1);
                }
                else
                {
                    division = member;
                }

                value = Convert.ChangeType(value, division.Type);
                expression = Expression.Divide(division, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is MethodCallExpression method)
            {
                value = Convert.ChangeType(value, method.Type);
                expression = Expression.Multiply(method, CosmosUtility.GetArgumentExpression(value));
            }
            else if (parameter is ParameterExpression param)
            {
                foreach (var property in Properties)
                {
                    division = CosmosUtility.GetMemberExpression(property.Property, param);

                    if (division.Type.IsNumericType(out var numericType2, false) &&
                        division.Type.IsNullable(numericType2))
                    {
                        division = Expression.Convert(division, numericType2);
                    }
                    else
                    {
                        // If one property is not numeric than adding is invalid
                        throw new CosmosInvalidMethodRequestException(
                            message: "The request under '$where' or '$filter' is invalid.",
                            source: "The method '$divide' cannot be used for the requested member or method.");
                    }
                    if (expression == null)
                    {
                        expression = division;
                    }
                    else
                    {
                        expression = Expression.Divide(expression, division);
                    }
                }
            }
            else
            {
                throw new CosmosInvalidMethodRequestException(
                    message: "The request under '$where' or '$filter' is invalid.",
                    source: "The method '$divide' cannot be used for the requested member or method.");
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
