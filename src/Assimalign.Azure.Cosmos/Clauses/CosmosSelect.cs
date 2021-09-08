using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Assimalign.Azure.Cosmos.Clauses
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Exceptions;
    using Assimalign.Azure.Cosmos.Functions;
    using Assimalign.Azure.Cosmos.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosSelect : ICosmosSelect<CosmosSelect>
    {

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$properties")]
        public IEnumerable<CosmosSelect> Properties { get; set; }

      
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$property")]
        public string Property { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$filterBy")]
        public CosmosSelectFilter FilterBy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instances"></param>
        /// <returns></returns>
        private IDictionary<string, CosmosSelect[]> BreakoutSelect<T>(CosmosSelect[] instances)
        {
            var breakout = new Dictionary<string, CosmosSelect[]>();
            var distinct = instances.Select(x => x.Property.Split('.').First()).Distinct();
            var last = distinct.Last();

            foreach (var value in distinct)
            {
                var results = instances
                    .Where(x => x.Property.Split('.').First() == value)
                    .Select(x =>
                    {
                        var field = string.Join('.', x.Property.Split('.').Skip(1));
                        if (field == string.Empty)
                        {
                            return new CosmosSelect()
                            {
                                FilterBy = x.FilterBy
                            };
                        }
                        else
                        {
                            return new CosmosSelect()
                            {
                                Property = field,
                                FilterBy = x.FilterBy
                            };
                        }

                    }).ToArray();

                breakout.Add(value, results);
            }

            return breakout;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> CreateQuery<T>(IQueryable<T> source)
        {

            var projections = BreakoutSelect<T>(this.Properties.ToArray());
            var parameter = Expression.Parameter(typeof(T));
            var type = GetAnonymousSelectType<T>(typeof(T), projections);
            var initialize = BuildAnonymousSelectExpression<T>(type, projections, parameter);
            var lambda = Expression.Lambda<Func<T, object>>(initialize, parameter);
            var select = Expression.Call(
                typeof(Queryable),
                "Select",
                new Type[] { source.ElementType, typeof(object) },
                source.Expression,
                lambda);

            return source.Provider.CreateQuery<T>(select) as IOrderedQueryable<T>;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">The type in which to build anonymous type from.</param>
        /// <param name="projections"> The collection of parent and child properties</param>
        /// <returns></returns>
        private Type GetAnonymousSelectType<T>(Type type, IDictionary<string, CosmosSelect[]> projections)
        {
            // Get Builder Type
            var builder = CosmosAnonymousTypes.GetTypeBuilder(type.Name);

            // Iterate through select projections
            foreach (var projection in projections)
            {
                var member = type.GetProperty(
                    projection.Key,
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance);

                // Let's check if the member is null, and if, the property
                // passed does not exist on the type being built
                if (null == member)
                {
                    throw new CosmosInvalidPropertySelectException(type, projection.Key);
                }

                // Check if all Child Properties, if any, are null
                // this will tell us that all the properties have been created for the type
                if (projection.Value.All(x => x?.Property == null))
                {
                    builder.CreateProperty(member.Name, member.PropertyType);
                }


                else if (projection.Value.Any(x => x.Property != null))
                {
                    var children = BreakoutSelect<T>(projection.Value);
                    if (member.PropertyType.IsTypedEnumerable(out var enumType))
                    {
                        var enumerable = GetAnonymousSelectType<T>(enumType, children);
                        var enumerableType = typeof(IEnumerable<>).MakeGenericType(enumerable);
                        builder.CreateProperty(member.Name, enumerableType);
                    }
                    else
                    {
                        var child = GetAnonymousSelectType<T>(member.PropertyType, children);
                        builder.CreateProperty(member.Name, child);
                    }
                }
            }

            return builder.CreateType();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projections"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private Expression BuildAnonymousSelectExpression<T>(Type type, IDictionary<string, CosmosSelect[]> projections, Expression member)
        {
            var instance = Expression.New(type);
            var anonymousParameter = Expression.Parameter(type);
            var anonymousBindings = new List<MemberBinding>();

            foreach (var projection in projections)
            {
                var sourceMember = Expression.Property(member, projection.Key);
                var anonymousMember = Expression.Property(anonymousParameter, projection.Key);

                if (projection.Value.All(x => x.Property == null))
                {
                    var filter = projection.Value.First().FilterBy;
                    var isFilter = filter != null;

                    // Let's check if we have an enumerable Type and a Filter Object
                    // if so, we can filter out select
                    if (isFilter && sourceMember.Type.IsTypedEnumerable(out var enumType))
                    {
                        var enumerableParameter = Expression.Parameter(enumType);
                        var enumerableLambdaBody = filter.GetLambdaExpressionBody(enumerableParameter);
                        var enumerableLambda = Expression.Lambda(enumerableLambdaBody, enumerableParameter);
                        var enumerableWhere = Expression.Call(
                            typeof(Enumerable),
                            "Where",
                            new Type[] { enumType },
                            sourceMember,
                            enumerableLambda);

                        var listConversion = Expression.Call(
                           typeof(Enumerable),
                           "ToList",
                           new Type[] { enumType },
                           enumerableWhere);

                        var binding = Expression.Bind(anonymousMember.Member, listConversion);
                        anonymousBindings.Add(binding);
                    }
                    else if (isFilter && sourceMember.Type.IsArrayType(out var arrayType))
                    {
                        var enumerableParameter = Expression.Parameter(arrayType);
                        var enumerableLambdaBody = filter.GetLambdaExpressionBody(enumerableParameter);
                        var enumerableLambda = Expression.Lambda(enumerableLambdaBody, enumerableParameter);
                        var enumerableWhere = Expression.Call(
                            typeof(Enumerable),
                            "Where",
                            new Type[] { arrayType },
                            sourceMember,
                            enumerableLambda);

                        var arrayConversion = Expression.Call(
                           typeof(Enumerable),
                           "ToArray",
                           new Type[] { arrayType },
                           enumerableWhere);

                        var binding = Expression.Bind(anonymousMember.Member, arrayConversion);
                        anonymousBindings.Add(binding);
                    }
                    else
                    {
                        var binding = Expression.Bind(anonymousMember.Member, sourceMember);
                        anonymousBindings.Add(binding);
                    }
                }

                else if (projection.Value.Any(x => x != null))
                {
                    if (sourceMember.Type.IsTypedEnumerable(out var enumType))
                    {
                        var enumerableType = enumType;
                        var enumerableParameter = Expression.Parameter(enumerableType);
                        var enumerable = BuildAnonymousSelectExpression<T>(enumerableType, BreakoutSelect<T>(projection.Value), enumerableParameter);
                        var lambda = Expression.Lambda(enumerable, enumerableParameter);
                        var select = Expression.Call(
                            typeof(Enumerable),
                            "Select",
                            new Type[] { enumerableType, enumerable.Type },
                            sourceMember,
                            lambda);

                        var cast = Expression.Convert(select, anonymousMember.Type);
                        var binding = Expression.Bind(anonymousMember.Member, cast);
                        anonymousBindings.Add(binding);
                    }
                    else
                    {
                        var select = BuildAnonymousSelectExpression<T>(anonymousMember.Type, BreakoutSelect<T>(projection.Value), sourceMember);
                        var binding = Expression.Bind(anonymousMember.Member, select);
                        anonymousBindings.Add(binding);
                    }
                }
            }

            return Expression.MemberInit(instance, anonymousBindings);
        }
    }
}
