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
	public static class CosmosPatcher
	{

		/// <summary>
		/// Will two objects of the same type
		/// </summary>
		public static T Merge<T>(T target, T source, CosmosPatcherOptions options = null)
			where T : class, new()
		{
			options ??= new CosmosPatcherOptions();

			// Create a Clone instance of the target to recevie merges
			var instance = target.Clone();

			// Get properties to iterate through. Let's memoize the request for the type so we don't have to b
			var getProperties = Cacher<T, PropertyInfo[]>.Memoize(instance =>
				instance.GetType()
				.GetProperties()
				.Where(prop => prop.CanRead && prop.CanWrite)
				.ToArray());

			// Iterate through properties and evaluate target to source
			foreach (var property in getProperties.Invoke(target))
			{
				var targetValue = property.GetValue(target);
				var sourceValue = property.GetValue(source);

				// Check if property on both target and source are null as well as does not wrapped in nullable class, if so, exit
				if (sourceValue == null && targetValue == null && property.PropertyType != typeof(Nullable<>))
					continue;

				// Check if Nullable target set allowed
				if (sourceValue == null && !options.IsNullableSetAllowed)
					continue;

				// Check if property type if Array Type
				if (property.PropertyType.IsArrayType(out var arrayType))
				{
					// var keys = options.uniqueKeys[type];
					// Check if element type of array is System Value Type
					if (arrayType.IsSystemValueType())
					{
						foreach (var item in (object[])sourceValue)
						{

							if (((object[])sourceValue).Contains(item))
							{

							}
						}
					}
					// Check if element type of array is System Value Type
					else if (arrayType.IsObjectType())
					{

					}
					continue;
				}

				// Check if property type if Enumerable Type (IEnumerable<>, IEnumerable, ICollection, IList, IDictionary, etc.,)
				if (property.PropertyType.IsTypedEnumerable(out var enumType))
				{
					targetValue ??= Activator.CreateInstance(typeof(List<>).MakeGenericType(enumType));
					if (enumType.IsSystemValueType())
					{

					}
					else
					{
						//var enumerable = Merge(
					}
					//typeof(List<>).MakeGenericType(property.PropertyType)
				}

				// 
				if (property.PropertyType.IsDictionaryType())
				{
					Console.WriteLine($"Found Dictionary Type for: {property.Name}");
				}

				// Check if property type is Object Type (Class or Record )
				if (property.PropertyType.IsObjectType())
				{
					// Check if target value is null. Need to create an instance if not to be able to get the type for child loop
					targetValue ??= Activator.CreateInstance(property.PropertyType);

					// If source is null and the code has made it this far 
					// this means that setting properties to null is allowed
					if (sourceValue == null)
					{
						property.SetValue(instance, null);
					}
					else
					{
						// Capture Child Merge Changes then set them to the new instance of the target
						var changes = Merge(targetValue, sourceValue, options);
						property.SetValue(instance, changes);
					}
					continue;
				}

				// Check if Non-Nullable Source Value is different than target value
				if (targetValue != sourceValue && sourceValue != null)
				{
					// Lets check if property type if numeric 
					if (property.PropertyType.IsNumericType())
					{
						SetNumericValueType(ref instance, sourceValue, property, options);
						continue;
					}
					if (property.PropertyType.IsBooleanType())
					{
						SetBooleanValueType(ref instance, sourceValue, property, options);
						continue;
					}
					if (property.PropertyType.IsDateTimeType())
					{
						SetDateTimeValueType(ref instance, sourceValue, property, options);
						continue;
					}
					if (property.PropertyType.IsTimeSpanType())
					{
						SetTimeSpanValueType(ref instance, sourceValue, property, options);
						continue;
					}
					if (property.PropertyType.IsGuidType())
					{
						SetGuidValueType(ref instance, sourceValue, property, options);
						continue;
					}
					if (sourceValue is string stringValue)
					{
						property.SetValue(instance, stringValue);
						continue;
					}
					// Just incase we missed any value types lets try to set the value to the target
					// if IsSystemValueType evaluates true
					if (property.PropertyType.IsSystemValueType())
					{
						property.SetValue(instance, sourceValue);
						continue;
					}
				}
				// If we have made this farin the code then setting property to null value is allowed
				else
				{
					property.SetValue(instance, sourceValue);
				}
			}
			return (T)instance;
		}


		/// <summary>
		/// 
		/// </summary>
		public static IEnumerable<T> Merge<T>(IEnumerable<T> target, IEnumerable<T> source, CosmosPatcherOptions options = null)
			where T : class, new()
		{
			options ??= new CosmosPatcherOptions();




			return target;
		}



		/// <summary>
		/// 
		/// </summary>
		private static void SetNumericValueType<T>(ref T target, object value, PropertyInfo property, CosmosPatcherOptions options)
		{
			var isAllowed = options.IsNumericDefaultSetAllowed;

			if (value is ushort uint16 && ((uint16 == 0 && isAllowed) || uint16 > 0))
			{
				property.SetValue(target, value);
			}
			else if (value is short int16 && ((int16 == 0 && isAllowed) || int16 > 0 || int16 < 0))
			{
				property.SetValue(target, value);
			}
			else if (value is uint uint32 && ((uint32 == 0 && isAllowed) || uint32 > 0))
			{
				property.SetValue(target, value);
			}
			else if (value is int int32 && ((int32 == 0 && isAllowed) || int32 > 0 || int32 < 0))
			{
				property.SetValue(target, value);
			}
			else if (value is ulong uint64 && ((uint64 == 0 && isAllowed) || uint64 > 0))
			{
				property.SetValue(target, value);
			}
			else if (value is long int64 && ((int64 == 0 && isAllowed) || int64 > 0 || int64 < 0))
			{
				property.SetValue(target, value);
			}
			else if (value is decimal deci && ((deci == 0 && isAllowed) || deci > 0 || deci < 0))
			{
				property.SetValue(target, value);
			}
			else if (value is double dbl && ((dbl == 0 && isAllowed) || dbl > 0 || dbl < 0))
			{
				property.SetValue(target, value);
			}
			else if (value is null && options.IsNullableSetAllowed)
			{
				property.SetValue(target, null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static void SetBooleanValueType<T>(ref T target, object value, PropertyInfo property, CosmosPatcherOptions options)
		{
			// Check if the boolean value is 'true' or if default value set is allowed
			if (value is bool boolean && (boolean || options.IsBooleanDefaultSetAllowed))
			{
				property.SetValue(target, boolean);
			}
			if (value is null && options.IsNullableSetAllowed)
			{
				property.SetValue(target, null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static void SetDateTimeValueType<T>(ref T target, object value, PropertyInfo property, CosmosPatcherOptions options)
		{
			if (value is DateTime dateTime && options.IsDateTimeDefaultSetAllowed)
			{
				property.SetValue(target, dateTime);
			}
			if (value is null && options.IsNullableSetAllowed)
			{
				property.SetValue(target, null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static void SetTimeSpanValueType<T>(ref T target, object value, PropertyInfo property, CosmosPatcherOptions options)
		{
			if (value is TimeSpan timeSpan && options.IsBooleanDefaultSetAllowed)
			{
				property.SetValue(target, timeSpan);
			}
			if (value is null && options.IsNullableSetAllowed)
			{
				property.SetValue(target, null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static void SetGuidValueType<T>(ref T target, object value, PropertyInfo property, CosmosPatcherOptions options)
		{
			if (value is Guid guid && options.IsBooleanDefaultSetAllowed)
			{
				property.SetValue(target, guid);
			}
			if (value is null && options.IsNullableSetAllowed)
			{
				property.SetValue(target, null);
			}
		}



		internal static class Cacher<TIn, TOut>
		{
			// Will hold reflection methods for specified types
			private static IDictionary<TIn, TOut> cache = new Dictionary<TIn, TOut>();

			/// <summary>
			/// Will use this method to cache requests for specified types
			/// </summary>
			public static Func<TIn, TOut> Memoize(Func<TIn, TOut> method)
			{
				return input => cache.TryGetValue(input, out var results) ?
					results :
					cache[input] = method(input);
			}
		}
	}
}
