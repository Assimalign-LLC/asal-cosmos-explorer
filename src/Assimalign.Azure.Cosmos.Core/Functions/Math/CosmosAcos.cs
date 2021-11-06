using System;
using System.Linq.Expressions;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosAcos : CosmosFunctions, ICosmosFunction
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

            if (parameter is BinaryExpression binary && binary.Type.IsFloatingPointNumericType())
            {
                // If the type is floating point but not double lets convert it
                if (!binary.Type.IsDoubleType())
                {
                    parameter = Expression.Convert(parameter, typeof(double));
                }

                var method = CosmosUtility.GetAcosMethod();
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            // Check if the 'parameter' expression is a member and a numeric type
            if (parameter is MemberExpression member && member.Type.IsFloatingPointNumericType())
            {
                // If the type is floating point but not double lets convert it
                if (!member.Type.IsDoubleType())
                {
                    parameter = Expression.Convert(parameter, typeof(double));
                }

                var method = CosmosUtility.GetAcosMethod();
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            // Check if the 'parameter' expression is a method and a numeric type
            else if (parameter is MethodCallExpression methodCall && methodCall.Type.IsFloatingPointNumericType())
            {
                // If the type is floating point but not double lets convert it
                if (methodCall.Type.IsDoubleType())
                {
                    parameter = Expression.Convert(parameter, typeof(double));
                }

                var method = CosmosUtility.GetAcosMethod();
                expression = CosmosUtility.GetMethodExpression(method, parameter);
            }
            // If member or method check where not valid assume the parameter type does not match the required type for this function
            else
            {
                throw new CosmosInvalidMethodRequestException(
                    message: "The request under '$where' or '$filter' is invalid.",
                    source: "The method '$abs' cannot be used for the requested member or method.");
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
