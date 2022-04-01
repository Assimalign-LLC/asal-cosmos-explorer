using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Assimalign.Azure.Cosmos.Utilities
{
    internal static class CosmosTypeExtensions
    {
        /// <summary>
        /// Identifies whether type implements the IEnumerable interface
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypedEnumerable(this Type type, bool search = false)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return true;

                var interfaces = type.GetInterfaces();
                foreach (var argument in type.GetGenericArguments())
                {
                    var enumerableType = typeof(IEnumerable<>).MakeGenericType(argument);

                    if (interfaces.Contains(enumerableType))
                        return true;
                }
            }
            else if (search)
            {
                return type.FindInterfaces((filter, criteria) => IsTypedEnumerable(filter), null)[0] != null;
            }

            return false;
        }

        /// <summary>
        /// Identifies whether type implements the IEnumerable interface and returns interface implementation type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="implementation">The IEnumerable Implementation</param>
        /// <returns></returns>
        public static bool IsTypedEnumerable(this Type type, out Type implementation)
        {
            implementation = null;
            if (type.IsGenericType)
            {
                var arguments = type.GetGenericArguments();
                if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    implementation = arguments[0];
                    return true;
                }
                var interfaces = type.GetInterfaces();
                foreach (var argument in arguments)
                {
                    var enumerableType = typeof(IEnumerable<>).MakeGenericType(argument);

                    if (interfaces.Contains(enumerableType))
                    {
                        implementation = argument;
                        return true;
                    }
                }
            }
            else
            {
                var hasIntefaces = type.GetInterfaces().Length > 0;

                if (hasIntefaces)
                {
                    var other = type.FindInterfaces((filter, criteria) => IsTypedEnumerable(filter), null).First();

                    if (null != other)
                    {
                        implementation = other.GetGenericArguments().First();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="implementation"></param>
        /// <returns></returns>
        public static bool IsArrayType(this Type type, out Type implementation)
        {
            var isArray = false;
            if (type.IsArray)
            {
                implementation = type.GetElementType();
                isArray = true;
            }
            else
                implementation = null;
            return isArray;
        }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public static bool IsObjectType(this Type type)
		{
			if (type.IsClass &&
				!type.IsArray &&
				!type.IsTypedEnumerable() &&
				type != typeof(string))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if type consist of all numeric types as well as floating point types.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		/// <returns></returns>
		public static bool IsNumericType(this Type type, bool checkNullable = true)
        {
			if (type.IsSignedNumericType(checkNullable))
			{
				return true;
			}
			if (type.IsUnsignedNumericType(checkNullable))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if type consist of all numeric types as well as floating point types and return the implementation.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="implementation"></param>
		/// <param name="checkNullable"></param>
		/// <returns></returns>
		public static bool IsNumericType(this Type type, out Type implementation, bool checkNullable = true)
        {
			implementation = null;

			if (type.IsSignedNumericType(out var signedNumericType, checkNullable))
            {
				implementation = signedNumericType;
				return true;
            }
			if (type.IsUnsignedNumericType(out var unsignedNumericType, checkNullable))
            {
				implementation = unsignedNumericType;
				return true;
            }

			return false;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsDoubleType(this Type type)
        {
			if (type == typeof(double) || type == typeof(Nullable<>).MakeGenericType(typeof(double))) 
			{
				return true;
            }
			else
            {
				return false;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="implementation"></param>
		/// <param name="checkNullable"></param>
		/// <returns></returns>
		public static bool IsFloatingPointNumericType(this Type type, out Type implementation, bool checkNullable = true)
        {
			implementation = null;
			var numericTypes = new Type[]
			{
				typeof(double),
				typeof(decimal),
				typeof(Single)
			};

			foreach (var numericType in numericTypes)
			{
				if (checkNullable && type == typeof(Nullable<>).MakeGenericType(numericType))
				{
					implementation = numericType;
					return true;
				}
				else if (type == numericType)
				{
					implementation = numericType;
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		/// <returns></returns>
		public static bool IsFloatingPointNumericType(this Type type, bool checkNullable = true)
		{
			var numericTypes = new Type[]
			{
				typeof(double),
				typeof(decimal),
				typeof(Single)
			};

			foreach (var numericType in numericTypes)
			{
				if (checkNullable && type == typeof(Nullable<>).MakeGenericType(numericType))
				{
					return true;
				}
				else if (type == numericType)
				{
					return true;
				}
			}

			return false;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		/// <returns></returns>
		public static bool IsSignedNumericType(this Type type, bool checkNullable = true)
        {
			var numericTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(decimal),
				typeof(Single)
			};

			foreach (var numericType in numericTypes)
			{
				if (checkNullable && type == typeof(Nullable<>).MakeGenericType(numericType))
				{
					return true;
				}
				else if (type == numericType)
				{
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Checks whether the type is Signed Numeric (meaning it can be negative or positive)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="implementation"></param>
		/// <param name="checkNullable"></param>
		public static bool IsSignedNumericType(this Type type, out Type implementation, bool checkNullable = true)
		{
			implementation = null;
			var numericTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(decimal),
				typeof(Single)
			};

			foreach (var numericType in numericTypes)
			{
				if (checkNullable && type == typeof(Nullable<>).MakeGenericType(numericType))
				{
					implementation = numericType;
					return true;
				}
				else if (type == numericType)
				{
					implementation = numericType;
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		/// <returns></returns>
		public static bool IsUnsignedNumericType(this Type type, bool checkNullable = true)
		{
			var numericTypes = new Type[]
			{
				typeof(ushort),
				typeof(uint),
				typeof(ulong)
			};

			foreach (var numericType in numericTypes)
			{
				if (checkNullable && type == typeof(Nullable<>).MakeGenericType(numericType))
				{
					return true;
				}
				else if (type == numericType)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks whether the type is Unsigned Numeric Type
		/// </summary>
		/// <param name="type"></param>
		/// <param name="implementation"></param>
		/// <param name="checkNullable"></param>
		public static bool IsUnsignedNumericType(this Type type, out Type implementation, bool checkNullable = true)
		{
			implementation = null;
			var numericTypes = new Type[]
			{
				typeof(ushort),
				typeof(uint),
				typeof(ulong)
			};

			foreach (var numericType in numericTypes)
			{
				if (checkNullable && type == typeof(Nullable<>).MakeGenericType(numericType))
				{
					implementation = numericType;
					return true;
				}
				else if (type == numericType)
				{
					implementation = numericType;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		public static bool IsBooleanType(this Type type, bool checkNullable = true)
		{
			if (checkNullable && type == typeof(Nullable<>).MakeGenericType(typeof(bool)))
			{
				return true;
			}
			if (type == typeof(bool))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		public static bool IsGuidType(this Type type, bool checkNullable = true)
		{
			if (checkNullable && type == typeof(Nullable<>).MakeGenericType(typeof(Guid)))
			{
				return true;
			}
			if (type == typeof(Guid))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="baseType"></param>
		/// <returns></returns>
		public static bool IsNullable(this Type type, Type baseType)
        {
			if (type == typeof(Nullable<>).MakeGenericType(baseType))
            {
				return true;
            }
			else
            {
				return false;
            }
        }



		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		public static bool IsDateTimeType(this Type type, bool checkNullable = true)
		{
			if (checkNullable && type == typeof(Nullable<>).MakeGenericType(typeof(DateTime)))
			{
				return true;
			}
			if (type == typeof(DateTime))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		public static bool IsTimeSpanType(this Type type, bool checkNullable = true)
		{
			if (checkNullable && type == typeof(Nullable<>).MakeGenericType(typeof(TimeSpan)))
			{
				return true;
			}
			if (type == typeof(TimeSpan))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Certain types in the .NET such as strings aren't considered value types 
		/// since they are mutable. Need to considered types such as these to be value types
		/// as if will cut down on the type checking for the expression building.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="checkNullable"></param>
		public static bool IsSystemValueType(this Type type, bool checkNullable = true)
		{
			// Will use this array of types to check for nullable value and enum types
			var valueTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(decimal),
				typeof(Single),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(char),
				typeof(byte),
				typeof(sbyte),
				typeof(bool),
				typeof(Guid),
				typeof(DateTime),
				typeof(TimeSpan)
			};

			// Since Value types are immutable by default one needs to explicitly 
			// set the value type as nullable by adding '?', adding [NullableAttribute], 
			// or wrapping it Nullable<?>
			if (type.IsValueType || type.IsEnum)
			{
				return true;
			}

			// Since a string is mutable it is not nessarlity considered a
			// value type. Let's include it as a value type since it is a supported system type.
			if (type == typeof(string))
			{
				return true;
			}

			// Let's ensure that the type is not wrapped in the Nullable<> type class
			if (checkNullable)
			{
				foreach (var valueType in valueTypes)
				{
					if (type == typeof(Nullable<>).MakeGenericType(valueType))
					{
						return true;
					}
				}
			}

			return false;
		}



		/// <summary>
		/// Certain types in the .NET such as strings aren't considered value types 
		/// since they are mutable. Need to considered types such as these to be value types
		/// as if will cut down on the type checking for the expression building.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="implementation"></param>
		public static bool IsSystemValueType(this Type type, out Type implementation)
		{
			implementation = null;

			// Will use this array of types to check for nullable value and enum types
			var valueTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(decimal),
				typeof(Single),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(char),
				typeof(byte),
				typeof(sbyte),
				typeof(bool),
				typeof(Guid),
				typeof(DateTime),
				typeof(TimeSpan)
			};

			foreach (var valueType in valueTypes)
			{
				if (type == typeof(Nullable<>).MakeGenericType(valueType))
				{
					implementation = valueType;
					return true;
				}
			}

			// Since Value types are immutable by default one needs to explicitly 
			// set the value type as nullable by adding '?', adding [NullableAttribute], 
			// or wrapping it Nullable<?>
			if (type.IsValueType || type.IsEnum)
			{
				implementation = type;
				return true;
			}

			// Since a string is mutable it is not nessarlity considered a
			// value type. Let's include it as a value type since it is a supported system type.
			if (type == typeof(string))
			{
				implementation = type;
				return true;
			}

			return false;
		}



		/// <summary>
		/// Certain types in the .NET such as strings aren't considered value types 
		/// since they are mutable. Need to considered types such as these to be value types
		/// as if will cut down on the type checking for the expression building.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="implementation"></param>
		/// <param name="isNullabe"></param>
		public static bool IsSystemValueType(this Type type, out Type implementation, out bool isNullabe)
		{
			isNullabe = false;
			implementation = null;

			// Will use this array of types to check for nullable value and enum types
			var valueTypes = new Type[]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(double),
				typeof(decimal),
				typeof(Single),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(char),
				typeof(byte),
				typeof(sbyte),
				typeof(bool),
				typeof(Guid),
				typeof(DateTime),
				typeof(TimeSpan)
			};

			foreach (var valueType in valueTypes)
			{
				if (type == typeof(Nullable<>).MakeGenericType(valueType))
				{
					isNullabe = true;
					implementation = valueType;
					return true;
				}
			}

			// Since Value types are immutable by default one needs to explicitly 
			// set the value type as nullable by adding '?', adding [NullableAttribute], 
			// or wrapping it Nullable<?>
			if (type.IsValueType || type.IsEnum)
			{
				implementation = type;
				return true;
			}

			// Since a string is mutable it is not nessarlity considered a
			// value type. Let's include it as a value type since it is a supported system type.
			if (type == typeof(string))
			{
				implementation = type;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks if Type is Dictionary Type
		/// </summary>
		/// <param name="type"></param>
		public static bool IsDictionaryType(this Type type)
		{
			var results = type.FindInterfaces((type, criteria) => type == typeof(IDictionary), null);

			if (results.Length > 0)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a basic Deep Clone of an object. 
		/// </summary>
		/// <param name="source"></param>
		public static object Clone(this object source)
		{
			// Get the type of source object and create a new instance of that type
			var sourceType = source.GetType();
			var instance = Activator.CreateInstance(sourceType);

			// Get all the properties of source object type that are readable and writable
			var properties = sourceType
				.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(prop => prop.CanRead && prop.CanWrite);

			// Assign all source property to taget object 's properties
			foreach (var property in properties)
			{
				// Check whether property type is value type, enum or string type
				if (property.PropertyType.IsSystemValueType())
				{
					property.SetValue(instance, property.GetValue(source, null), null);
				}
				// Assumption: property type is object/complex types, so need to recursively call this method until the end of the tree is reached
				else
				{
					var value = property.GetValue(source, null);
					if (value == null)
					{
						property.SetValue(instance, null, null);
					}
					else
					{
						property.SetValue(instance, value.Clone(), null);
					}
				}
			}

			return instance;
		}
    }
}
