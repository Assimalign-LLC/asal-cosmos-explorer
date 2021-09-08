using System;
using System.Text;
using System.Text.Json;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Clauses;
    

    public static class CosmosQueryBuilder
    {
        // private static ICosmosQuery cosmosQuery;


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static CosmosQuery<T> From<T>() 
            where T : class, new() => new CosmosQuery<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CosmosQuery<T> Select<T, TResult>(this CosmosQuery<T> query, Func<T, TResult> result)
            where T : class, new()
        {
            var selects = new List<CosmosSelect>();
            var instance = new T();
            var type = result.Invoke(instance).GetType();
            var propertyName = string.Empty;

            foreach(var property in type.GetProperties())
            {
                if (property.PropertyType.IsValueType)
                {
                    propertyName = property.Name;
                }
            }


            return query;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static CosmosQuery<T> Where<T>(this CosmosQuery<T> query, Expression<Func<T, bool>> where)
        {
            




            return query;
        }


        public static string ToJson<T>(this CosmosQuery<T> query)
        {
            return JsonSerializer.Serialize<CosmosQuery<T>>(query, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
