using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Clauses
{
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Functions;
    using Assimalign.Azure.Cosmos.Serialization;
    using Assimalign.Azure.Cosmos.Utilities;

    /// <summary>
    /// The filter clause is used to filter out data specifically on the document level.
    /// This usually consists of a collection such as an array of objects or value types.
    /// </summary>
    public sealed class CosmosSelectFilter : CosmosFunctions, ICosmosSelectFilter<CosmosSelectFilter>
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$or")]
        public IEnumerable<CosmosSelectFilter> Or { get; set; } = new List<CosmosSelectFilter>();

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$and")]
        public IEnumerable<CosmosSelectFilter> And { get; set; } = new List<CosmosSelectFilter>();

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$operator")]
        public OperatorType Operator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$function")]
        public ValueTypeFunctions Function { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$value")]
        [JsonConverter(typeof(CosmosObjectConverter))]
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$property")]
        public string Property { get; set; }


         

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal Expression GetLambdaExpressionBody(Expression parameter, CosmosSelectFilter instance = null)
        {
            // Represent a Parent expression to reference for any child expression
            Expression parent = null;

            if (this.And.Any())
            {
                foreach (var where in this.And)
                {
                    var isParent = parent != null;
                    // Check if only one filter was passed
                    if (!isParent && !where.And.Any() && !where.Or.Any())
                    {
                        parent = BuildLambdaExpressionBody(where, parameter);
                    }
                    else if (isParent)
                    {
                        var child = GetLambdaExpressionBody(parameter, where);
                        if (where.Or.Any())
                        {
                            parent = Expression.AndAlso(parent, child);
                        }
                        else // We've reached the end of the And 'Concatenation'
                        {
                            parent = Expression.AndAlso(parent, child);
                        }
                    }
                }

                return parent;
            }

            if (this.Or.Any())
            {
                foreach (var where in this.Or)
                {
                    var isParent = parent != null;
                    // Check if only one filter was passed
                    if (!isParent && !where.And.Any() && !where.Or.Any())
                    {
                        parent = BuildLambdaExpressionBody(where, parameter);
                    }
                    else if (isParent)
                    {
                        var child = GetLambdaExpressionBody(parameter, where);
                        if (where.And.Any())
                        {
                            parent = Expression.OrElse(parent, child);
                        }
                        else // We've reached the end of the And 'Concatenation'
                        {
                            parent = Expression.OrElse(parent, child);
                        }
                    }
                }

                return parent;
            }

            return BuildLambdaExpressionBody(this, parameter);
        }



        /// <summary>
        /// Builds the passed through instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        private Expression BuildLambdaExpressionBody(CosmosSelectFilter where, Expression expression)
        {

            // Check if Expression is ParameterExpression (Only Parameter Expressions should be passed at this point)
            if (expression is ParameterExpression parameter)
            {
                // Get the member expression to apply the evaluation to.
                if (where.Property is not null)
                {
                    expression = CosmosUtility.GetMemberExpression(where.Property, parameter);
                }
                // Check if any reserved functions were set
                if (where.Function is not ValueTypeFunctions.None)
                {
                    if (where.Function == ValueTypeFunctions.ToLower)
                        expression = CosmosUtility.GetMethodExpression(expression, CosmosUtility.GetToLowerMethod());

                    else if (where.Function == ValueTypeFunctions.ToUpper)
                        expression = CosmosUtility.GetMethodExpression(expression, CosmosUtility.GetToUpperMethod());
                }
                // Check if there are any child functions to chain to the clause
                if (base.IsFunctionAvailable && base.CurrentFunction.TryGetExpression(expression, out var method))
                {
                    expression = method;
                }
                // Check if there is an evaluation to a property of root/parent of the type
                if (where.Value is string stringValue)
                {
                    var properties = stringValue.Split('.');
                    if (properties.First() == "$root")
                    {
                        if (properties.Count() == 1)
                        {
                            throw new CosmosInvalidPropertyException(
                                "A property chain must be provided hen using '$root' as the value comparison");
                        }
                        where.Value = CosmosUtility.GetMemberExpression(string.Join('.', properties.Skip(1)), parameter);
                    }
                }
            }
            else
            {
                // TODO: Decide whether to throw an error or log warning of un-applied filter
            }

            // Check if Value is of a $root property to compare to 
            if (where.Value is MemberExpression member)
            {
                Expression constant = member;

                // Check if query is requesting to compare complex type
                if (!constant.Type.IsSystemValueType())
                {
                    throw new CosmosInvalidComparisonException(member.Member.Name);
                }
                if (constant.Type.IsNullable(expression.Type))
                {
                    constant = Expression.Convert(constant, expression.Type);
                }
                // The types need to be the same
                if (member.Type != expression.Type)
                {
                    throw new CosmosInvalidComparisonException(expression.Type.Name, member.Member.Name);
                }

                // Return an operator expression
                return CosmosUtility.GetOperatorExpression(
                    where.Operator,
                    expression,
                    constant);
            }
            // Check if a OrElse or AndAlso Binary Expression was returned
            else if (expression is BinaryExpression binary &&
                where.Value is bool)
            {
                return binary;
            }
            // Check if expression is a called method and the method returns boolean type
            // This will cut down on the extra predicate if continuing
            else if (expression is MethodCallExpression methodExpression &&
                methodExpression.Method.ReturnType.IsBooleanType())
            {
                return methodExpression;
            }
            else
            {
                // Convert Type if possible to ensure evaluation is coursing the correct type
                ConvertValueType(ref where, expression.Type);

                var constant = CosmosUtility.GetArgumentExpression(where.Value);

                // Return an operator expression
                return CosmosUtility.GetOperatorExpression(
                    where.Operator,
                    expression,
                    constant);
            }
        }


        /// <summary>
        /// Depending on the Expression Method or Member return type you may need 
        /// to coarse the evaluation value to the correct type. Example: int32 -> decimal
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="type"></param>
        private void ConvertValueType(ref CosmosSelectFilter instance, Type type)
        {
            try
            {
                if (type.IsValueType)
                {
                    instance.Value = Convert.ChangeType(instance.Value, type);
                }
            }
            catch (InvalidCastException exception)
            {
                throw new CosmosInvalidComparisonException(exception);
            }
        }
    }
}
