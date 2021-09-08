using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Utilities;


    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosRound : CosmosFunctions, ICosmosFunction
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            expression = null;

            if (parameter is BinaryExpression binary && binary.Type.IsFloatingPointNumericType(out var numericBinaryType))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (binary.Type.IsNullable(numericBinaryType))
                {
                    parameter = Expression.Convert(parameter, numericBinaryType);
                }

                var method = CosmosUtility.GetRoundMethod(numericBinaryType);
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            // Check if the 'parameter' expression is a member and a numeric type
            if (parameter is MemberExpression member && member.Type.IsFloatingPointNumericType(out var numericMemberType))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (member.Type.IsNullable(numericMemberType))
                {
                    parameter = Expression.Convert(parameter, numericMemberType);
                }
                
                var method = CosmosUtility.GetRoundMethod(numericMemberType);
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            // Check if the 'parameter' expression is a method and a numeric type
            else if (parameter is MethodCallExpression methodCall && methodCall.Method.ReturnType.IsFloatingPointNumericType(out var numericReturnType))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (methodCall.Type.IsNullable(numericReturnType))
                {
                    parameter = Expression.Convert(parameter, numericReturnType);
                }

                var method = CosmosUtility.GetRoundMethod(numericReturnType);
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            // If member or method check where not valid assume the parameter type does not match the required type for this function
            else
            {
                throw new CosmosInvalidMethodRequestException(
                    message: "The request under '$where' or '$filter' is invalid.",
                    source: "The method '$round' cannot be used for the requested member or method.");
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
