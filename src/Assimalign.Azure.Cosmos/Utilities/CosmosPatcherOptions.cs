using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Utilities
{
	/// <summary>
	/// 
	/// </summary>
    public class CosmosPatcherOptions
    {
		internal IDictionary<Type, PropertyInfo[]> uniqueKeys = new Dictionary<Type, PropertyInfo[]>();

		/*
			NOTES: 


		*/


		/// <summary>
		/// Indicates whether properties should be set to null.
		/// </summary>
		public bool IsNullableSetAllowed { get; set; } = false;

		/// <summary>
		/// Indicates whether Int16, Int32, Int64, Decimals, and Double should be set to 0 if not nullable.
		/// </summary>
		public bool IsNumericDefaultSetAllowed { get; set; } = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsBooleanDefaultSetAllowed { get; set; } = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsDateTimeDefaultSetAllowed { get; set; } = false;


		/// <summary>
		/// Indicates that only nullable boolean values should be set. Default is 'true'.
		/// </summary>
		/// <remarks>
		/// Since there are only two possibilities for a boolean it's impossible to tell if the value was 
		/// explicitly changed to 'false' from 'true' since that is the default value. By setting this property to 
		/// </remarks>
		//public bool OnlySetNullableBooleanValues { get; set; } = true;


		/// <summary>
		/// 
		/// </summary>
		public CosmosPatcherOptions AddTypeKey<T>(Expression<Func<T, PropertyInfo[]>> expression)
		{
			var func = expression.Compile();

			//func.Invoke(

			return this;
		}
	}
}
