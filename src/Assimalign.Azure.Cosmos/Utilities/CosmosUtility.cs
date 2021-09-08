using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace Assimalign.Azure.Cosmos.Utilities
{
	using Assimalign.Azure.Cosmos.Clauses;
	using Assimalign.Azure.Cosmos.Exceptions;
	using Assimalign.Azure.Cosmos.Functions;


    internal static class CosmosUtility
	{
		// Will cache reflection request for methods already obtained
		private static ConcurrentDictionary<string, MethodInfo> methods = new ConcurrentDictionary<string, MethodInfo>();


		static CosmosUtility()	
        {

        }

		#region System.Reflection Method Info Utilities


		/// <summary>
		/// Returns the Round Method info from the Math Type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static MethodInfo GetRoundMethod(Type type)
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Round, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Round, new Type[] { type });
			methods.TryAdd(CosmosLinqToSqlFunctions.Round, method);
			return method;
		}

		/// <summary>
		/// Returns the Ceiling Method info from the Math Type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static MethodInfo GetCeilingMethod(Type type)
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Ceiling, out var cachedMethod))
				return cachedMethod;
			
			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Ceiling, new Type[] { type });
			methods.TryAdd(CosmosLinqToSqlFunctions.Ceiling, method);
			return method;
		}

		/// <summary>
		/// Returns the Floor Method info from the Math Type 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static MethodInfo GetFloorMethod(Type type)
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Floor, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Floor, new Type[] { type });
			methods.TryAdd(CosmosLinqToSqlFunctions.Floor, method);
			return method;
		}

		/// <summary>
		/// Returns the Abs Method info from the Math Type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static MethodInfo GetAbsMethod(Type type)
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Abs, out var cachedMethod))
				return cachedMethod;
			
			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Abs, new Type[] { type });
			methods.TryAdd(CosmosLinqToSqlFunctions.Abs, method);
			return method;
		}

		/// <summary>
		/// Returns the Acos Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetAcosMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Acos, out var cachedMethod))
				return cachedMethod;
			
			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Acos, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Acos, method);
			return method;
		}

		/// <summary>
		/// Returns the Asin Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetAsinMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Asin, out var cachedMethod))
				return cachedMethod;
			
			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Asin, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Asin, method);
			return method;
		}

		/// <summary>
		/// Returns the Atan Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetAtanMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Atan, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Atan, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Atan, method);
			return method;
		}

		/// <summary>
		/// Returns the Cos Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetCosMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Cos, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Cos, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Cos, method);
			return method;
		}

		/// <summary>
		/// Returns the Atan Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetExpMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Exp, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Exp, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Exp, method);
			return method;
		}

		/// <summary>
		/// Returns the Log Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetLogMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Log, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Log, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Log, method);
			return method;
		}

		/// <summary>
		/// Returns the LogTen Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetLogTenMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Log10, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Log10, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Log10, method);
			return method;
		}

		/// <summary>
		/// Returns the Pow Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetPowMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Pow, out var cachedMethod))
				return cachedMethod;
			
			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Pow, new Type[] { typeof(double), typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Pow, method);
			return method;
		}

		/// <summary>
		/// Returns the Sign Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetSingMethod(Type type)
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Sign, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Sign, new Type[] { type });
			methods.TryAdd(CosmosLinqToSqlFunctions.Sign, method);
			return method;
		}

		/// <summary>
		/// Returns the Sin Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetSinMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Sin, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Sin, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Sin, method);
			return method;
		}

		/// <summary>
		/// Returns the Sqrt Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetSqrtMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Sqrt, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Sqrt, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Sqrt, method);
			return method;
		}

		/// <summary>
		/// Returns the Tan Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetTanMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Tan, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Tan, new Type[] { typeof(double) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Tan, method);
			return method;
		}

		/// <summary>
		/// Returns the Sqrt Method info from the Math Type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetTruncateMethod(Type type)
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Truncate, out var cachedMethod))
				return cachedMethod;

			method = typeof(Math).GetMethod(CosmosLinqToSqlFunctions.Truncate, new Type[] { type });
			methods.TryAdd(CosmosLinqToSqlFunctions.Truncate, method);
			return method;
		}

		/// <summary>
		/// Get the method information for StartWith on type 'String'
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetStartsWithMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.StartsWith, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.StartsWith, new Type[] { typeof(string), typeof(StringComparison) });
			methods.TryAdd(CosmosLinqToSqlFunctions.StartsWith, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetEndsWithMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.EndsWith, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.EndsWith, new Type[] { typeof(string), typeof(StringComparison) });
			methods.TryAdd(CosmosLinqToSqlFunctions.EndsWith, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetToUpperMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.ToUpper, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.ToUpper, new Type[] { });
			methods.TryAdd(CosmosLinqToSqlFunctions.ToUpper, method);
			return method;
		}

		/// <summary>
		/// Gets the ToLower Method from string type
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetToLowerMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.ToLower, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.ToLower, new Type[] { });
			methods.TryAdd(CosmosLinqToSqlFunctions.ToLower, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetContainsMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Contains, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.Contains, new Type[] { typeof(string), typeof(StringComparison) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Contains, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetSubStringMethod()
        {
			MethodInfo method = null;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.SubString, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.SubString, new Type[] { typeof(int), typeof(int) });
			methods.TryAdd(CosmosLinqToSqlFunctions.SubString, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetReplaceMethod()
        {
			MethodInfo method = null;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Replace, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.Replace, new Type[] { typeof(string), typeof(string) });
			methods.TryAdd(CosmosLinqToSqlFunctions.Replace, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetStringEqualsMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.StringEquals, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(StringComparison) });
			methods.TryAdd(CosmosLinqToSqlFunctions.StringEquals, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetTrimMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.Trim, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.Trim, new Type[] {  });
			methods.TryAdd(CosmosLinqToSqlFunctions.Trim, method);
			return method;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetTrimStartMethod()
        {
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.TrimStart, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.TrimStart, new Type[] {  });
			methods.TryAdd(CosmosLinqToSqlFunctions.TrimStart, method);
			return method;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static MethodInfo GetTrimEndMethod()
		{
			MethodInfo method;
			if (methods.TryGetValue(CosmosLinqToSqlFunctions.TrimEnd, out var cachedMethod))
				return cachedMethod;

			method = typeof(string).GetMethod(CosmosLinqToSqlFunctions.TrimEnd, new Type[] {  });
			methods.TryAdd(CosmosLinqToSqlFunctions.TrimEnd, method);
			return method;
		}
        #endregion

        #region System.Linq.Expression Utility Functions
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression(string property, ParameterExpression parameter)
		{
			try
            {
				String[] paths = property.Split('.');
				Expression expression = parameter;
				for (int i = 0; i < paths.Length; i++)
				{
					expression = Expression.Property(expression, paths[i]);
				}
				return expression as MemberExpression;
			}
            catch
            {
				throw new CosmosInvalidPropertyException(
					$"The requested property or property path was invalid or malformed: {property}");
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="method"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static MethodCallExpression GetMethodExpression(Expression expression, MethodInfo method) =>
			Expression.Call(expression, method);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static MethodCallExpression GetMethodExpression(MethodInfo method, Expression? expression) =>
			Expression.Call(method, expression);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="method"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static MethodCallExpression GetMethodExpression(Expression expression, MethodInfo method, IEnumerable<Expression> arguments) =>
			Expression.Call(expression, method, arguments);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TIn"></typeparam>
		/// <typeparam name="TOut"></typeparam>
		/// <param name="parameter"></param>
		/// <param name="predicate"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static MethodCallExpression GetStaticMethodExpression<TIn,TOut>(Expression parameter, Func<TIn, TOut> predicate, MethodInfo method) =>
			Expression.Call(method, parameter, Expression.Constant(predicate));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static ConstantExpression GetArgumentExpression(object? value) =>
			Expression.Constant(value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static IEnumerable<ConstantExpression> GetArgumentExpressions(params object[]? values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				yield return Expression.Constant(values[i]);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="where"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static Expression GetOperatorExpression(OperatorType expressionType, Expression expression, Expression constant)
		{
			switch (expressionType)
			{
				case OperatorType.Equal:
					return Expression.Equal(expression, constant);
				case OperatorType.NotEqual:
					return Expression.NotEqual(expression, constant);
				case OperatorType.GreaterThan:
					return Expression.GreaterThan(expression, constant);
				case OperatorType.GreaterThanOrEqual:
					return Expression.GreaterThanOrEqual(expression, constant);
				case OperatorType.LessThan:
					return Expression.LessThan(expression, constant);
				case OperatorType.LessThanOrEqual:
					return Expression.LessThanOrEqual(expression, constant);
				default:
					return null;
			}
		}
		#endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="argTypes"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static MethodBase GetGenericMethod(Type type, string name, Type[] arguments, Type[] argTypes, BindingFlags flags)
		{
			int typeArity = arguments.Length;
			var methods = type.GetMethods()
				.Where(m => m.Name == name)
				.Where(m => m.GetGenericArguments().Length == typeArity)
				.Select(m => m.MakeGenericMethod(arguments));

			return Type.DefaultBinder.SelectMethod(flags, methods.ToArray(), argTypes, null);
		}
	}
}
