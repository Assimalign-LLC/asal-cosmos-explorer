
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Assimalign.Azure.Cosmos.Functions
{

    using Assimalign.Azure.Cosmos.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosFloor : CosmosFunctions, ICosmosFunction
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

            // Check if the 'parameter' expression is a member and a numeric type
            if (parameter is MemberExpression member && member.Type.IsFloatingPointNumericType(out var numericMemberType))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (member.Type.IsNullable(numericMemberType))
                {
                    parameter = Expression.Convert(parameter, numericMemberType);
                }

                var method = CosmosUtility.GetFloorMethod(numericMemberType);
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            else if (parameter is MethodCallExpression methodCall && methodCall.Method.ReturnType.IsFloatingPointNumericType(out var numericReturnType))
            {
                // If the type is nullable lets convert it to non-nullable or
                // the MethodCallExpression will fail due to incompatible types
                if (methodCall.Type.IsNullable(numericReturnType))
                {
                    parameter = Expression.Convert(parameter, numericReturnType);
                }

                var method = CosmosUtility.GetFloorMethod(numericReturnType);
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            else
            {
                // TODO: Add parameter value does not match required type for expression to warnings collection
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
