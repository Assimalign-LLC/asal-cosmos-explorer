using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosPaging : ICosmosPaging
    {

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$page")]
        public int Page { get; set; } = 1;


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$count")]
        public int Count { get; set; } = 50;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> CreateQuery<T>(IQueryable<T> source)
        {
            var offset = Expression.Call(
                 typeof(Queryable),
                 "Skip",
                 new Type[] { source.ElementType },
                 source.Expression,
                 Expression.Constant(this.Count * (this.Page <= 1 ? 0 : this.Page - 1)));

            var limit = Expression.Call(
                   typeof(Queryable),
                   "Take",
                   new Type[] { source.ElementType },
                   offset,
                   Expression.Constant(this.Count));

            return source.Provider.CreateQuery<T>(limit) as IOrderedQueryable<T>;
        }
    }
}
