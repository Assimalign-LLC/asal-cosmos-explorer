using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos
{
    using Assimalign.Azure.Cosmos.Clauses;

    /// <summary>
    /// Represents a query object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CosmosQuery<T> : ICosmosQuery<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosQuery()
        {
            QueryId = Guid.NewGuid();
        }


        /// <summary>
        /// A unique identifier for the query request.
        /// </summary>
        public Guid QueryId { get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$select")]
        public CosmosSelect Select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$where")]
        public CosmosWhere Where { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("$orderBy")]
        public CosmosOrderBy OrderBy { get; set; }

        /// <summary>
        /// A object representing the paging request for the 
        /// </summary>
        [JsonPropertyName("$paging")]
        public CosmosPaging Paging { get; set; } = new CosmosPaging();


        /// <summary>
        /// Builds the query collection
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IOrderedQueryable<T> BuildQuery(IOrderedQueryable<T> source)
        {
            if (this.Where is not null)
            {
                source = this.Where.CreateQuery<T>(source);
            }
            if (this.OrderBy is not null)
            {
                source = this.OrderBy.CreateQuery<T>(source);
            }
            if (this.Paging is not null)
            {
                source = this.Paging.CreateQuery<T>(source);
            }
            if (this.Select is not null)
            {
                source = this.Select.CreateQuery<T>(source);
            }

            return source;
        }
    }
}


/*
        NOTICE: Below is Old Code and may be useful in the future. DON'T FU*K**G REMOVE IT.
     
     

    //var array = this.OrderBy.ToArray();
    //for (int i = 0; i < array.Length; i++)
    //{
    //    string method;
    //    if (i == 0)
    //    {
    //        method = array[i].Sort == SortType.Descending ? 
    //            "OrderByDescending" : "OrderBy";
    //    }
    //    else
    //    {
    //        method = array[i].Sort == SortType.Descending ? 
    //            "ThenByDescending" : "ThenBy";
    //    }

    //    var parameter = _itemParameter.Invoke();
    //    var member = CosmosQueryHelper.GetPropertyMemberExpression(array[i].Property, parameter);
    //    var lambda = Expression.Lambda<Func<T, object>>(
    //        member.Type.IsValueType == true ? Expression.Convert(member, typeof(object)) : member, parameter);
    //    var orderBys = Expression.Call(
    //        typeof(Queryable),
    //        method,
    //        new Type[] { source.ElementType, typeof(object) },
    //        source.Expression,
    //        lambda);
    //    source = source.Provider.CreateQuery<T>(orderBys);
    //}


    //var parameter = _itemParameter.Invoke();
    //var expressions = new Dictionary<string, Expression>();
    //var bindings = new List<MemberBinding>();
    //var builder = AnonymousTypes.GetTypeBuilder(typeof(T).Name);

    //foreach (var statement in this.Select)
    //{
    //    var propertyName = statement.Alias ?? statement.Property.Split('.').LastOrDefault();
    //    var property = QueryHelper.GetPropertyMemberExpression(statement.Property, parameter);
    //    var properties = statement.Property.Split('.');

    //    Type propertyType = typeof(T);
    //    PropertyInfo propertyInfo = null;

    //    for(int i = 0; i < properties.Length; i++)
    //    {
    //        if (propertyInfo != null)
    //            propertyType = propertyInfo.PropertyType;

    //        propertyInfo = propertyType.GetProperty(properties[i]);

    //        if (propertyInfo.PropertyType.IsValueType)
    //        {

    //        }
    //        else if (propertyInfo.PropertyType == typeof(string))
    //        {

    //        }
    //        else
    //        {

    //        }
    //    }


    //    if (null != statement.Child && statement.Child.TryGetExpression<T>(property, out var expression))
    //    {
    //        expressions.Add(propertyName, expression);
    //        if (expression is MethodCallExpression method)
    //            builder.CreateProperty(propertyName, method.Type);
    //        else
    //            throw new Exception("Internal Exception: returned expression invalid");
    //    }
    //    else
    //    {
    //        expressions.Add(propertyName, property);
    //        builder.CreateProperty(propertyName, property.Type);
    //    }       
    //}

    //void CreateBinding(PropertyInfo property)
    //{
    //    var parameter = Expression.Parameter(property.PropertyType);
    //    var instance = Expression.New(property.PropertyType);

    //}

    //var types = builder.CreateType();
    //var instance = Expression.New(type);

    //foreach (var expression in expressions)
    //{
    //    var property = type.GetProperty(expression.Key);
    //    if (expression.Value is MethodCallExpression method)
    //        bindings.Add(Expression.Bind(property, method));
    //    else if (expression.Value is MemberExpression member)
    //        bindings.Add(Expression.Bind(property, member));
    //}



    //var initialize = Expression.MemberInit(instance, bindings);
    //var lambda = Expression.Lambda<Func<T, object>>(initialize, parameter);
    //var linq = Expression.Call(
    //    typeof(Queryable),
    //    "Select",
    //    new Type[] { source.ElementType, typeof(object) },
    //    source.Expression,
    //    lambda);

    //source = source.Provider.CreateQuery<T>(linq);

    // Original 
    //   var parameter = _itemParameter.Invoke();
    //   var expressions = new Dictionary<string, Expression>();
    //   var bindings = new List<MemberBinding>();
    //   var builder = AnonymousTypes.GetTypeBuilder(typeof(T).Name);

    //   foreach (var statement in this.Select)
    //   {
    //       var propertyName = statement.Alias ?? statement.Property.Split('.').LastOrDefault();
    //       var property = QueryHelper.GetPropertyMemberExpression(statement.Property, parameter);
    //       if (null != statement.Child && statement.Child.TryGetExpression<T>(property, out var expression))
    //       {
    //           expressions.Add(propertyName, expression);
    //           if (expression is MethodCallExpression method)
    //               builder.CreateProperty(propertyName, method.Type);
    //           else
    //               throw new Exception("Internal Exception: returned expression invalid");
    //       }
    //       else
    //       {
    //           expressions.Add(propertyName, property);
    //           builder.CreateProperty(propertyName, property.Type);
    //       }       
    //   }

    //   var type = builder.CreateType();
    //   var instance = Expression.New(type);

    //   foreach (var expression in expressions)
    //   {
    //       var property = type.GetProperty(expression.Key);
    //       if (expression.Value is MethodCallExpression method)
    //           bindings.Add(Expression.Bind(property, method));
    //       else if (expression.Value is MemberExpression member)
    //           bindings.Add(Expression.Bind(property, member));
    //   }

    //   var initialize = Expression.MemberInit(instance, bindings);
    //   var lambda = Expression.Lambda<Func<T, object>>(initialize, parameter);
    //   var linq = Expression.Call(
    //       typeof(Queryable),
    //       "Select",
    //       new Type[] { source.ElementType, typeof(object) },
    //       source.Expression,
    //       lambda);

    //   source = source.Provider.CreateQuery<T>(linq);
*/

