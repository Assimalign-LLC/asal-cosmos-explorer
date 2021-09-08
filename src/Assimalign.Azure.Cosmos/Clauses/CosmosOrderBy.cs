using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Clauses
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Serialization;


    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosOrderBy : ICosmosOrderBy<CosmosOrderBy>
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosOrderBy() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="sort"></param>
        public CosmosOrderBy(string property, SortType sort)
        {
            this.Property = property;
            this.Sort = sort;
        }


        /// <summary>
        /// The property path to sort by. 
        /// </summary>
        /// <remarks>
        /// If sorting child properties, set the path 
        /// with '.' as a separator for each child property.
        /// </remarks>
        [JsonPropertyName("$property")]
        public string Property { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$sort")]
        [JsonConverter(typeof(CosmosSortTypeConverter))]
        public SortType Sort { get; set; }

        /// <summary>
        /// Child property to sort by.
        /// </summary>
        [JsonPropertyName("$thenBy")]
        public CosmosOrderBy ThenBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> CreateQuery<T>(IQueryable<T> source) =>
            BuildOrderByExpression(source, this) as IOrderedQueryable<T>;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="index"></param>
        private IQueryable<T> BuildOrderByExpression<T>(IQueryable<T> source, CosmosOrderBy orderBy, int index = 0)
        {
            var parameter = Expression.Parameter(typeof(T));
            var member = CosmosUtility.GetMemberExpression(orderBy.Property, parameter);

            string method;
            if (index == 0)
            {
                method = orderBy.Sort == SortType.Ascending ? 
                    "OrderBy" : 
                    "OrderByDescending";
            }
            else
            {
                method = orderBy.Sort == SortType.Ascending ? 
                    "ThenBy" : 
                    "ThenByDescending";
            }

            var lambda = Expression.Lambda<Func<T, object>>(
                member.Type.IsValueType == true ? Expression.Convert(member, typeof(object)) : member, parameter);
            var linq = Expression.Call(
                typeof(Queryable),
                method,
                new Type[] { source.ElementType, typeof(object) },
                source.Expression,
                lambda);

            source = source.Provider.CreateQuery<T>(linq);

            if (null != orderBy.ThenBy)
            {
                return BuildOrderByExpression(source, orderBy.ThenBy, index + 1);
            }

            return source;
        }
    }
}
