using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Serialization
{
	using Assimalign.Azure.Cosmos.Clauses;

	internal class CosmosOperatorConverter : JsonConverter<OperatorType>
	{
		private static readonly string[] operatorEqualTo = new[] { "EQ", "Equal", "EqualTo" };
		private static readonly string[] operatorNotEqualTo = new[] { "NE", "NotEqual", "NotEqualTo" };
		private static readonly string[] operatorGreaterThan = new[] { "GT", "GreaterThan" };
		private static readonly string[] operatorGreaterThanEqualTo = new[] { "GTE", "GreaterThanEqualTo", "GreaterThanOrEqual" };
		private static readonly string[] operatorLessThan = new[] { "LT", "LessThan" };
		private static readonly string[] operatorLessThanEqualTo = new[] { "LTE", "LessThanEqualTo", "LessThanOrEqual" };


		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="type"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public override OperatorType Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var value = reader.GetString();

				if (Array.Exists(operatorEqualTo, x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
					return OperatorType.Equal;
				
				if (Array.Exists(operatorNotEqualTo, x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
					return OperatorType.NotEqual;
				
				if (Array.Exists(operatorGreaterThan, x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
					return OperatorType.GreaterThan;
                
				if (Array.Exists(operatorGreaterThanEqualTo, x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
					return OperatorType.GreaterThanOrEqual;
                
				if (Array.Exists(operatorLessThan, x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
					return OperatorType.LessThan;
                
				if (Array.Exists(operatorLessThanEqualTo, x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
					return OperatorType.LessThanOrEqual;
                
                else
					throw new JsonException("Invalid Operator Value");
			}
			else if (reader.TokenType == JsonTokenType.Number)
			{
				return (OperatorType)reader.GetInt32();
			}
			else
			{
				reader.Read();
				return OperatorType.None;
			}
		}

		public override void Write(Utf8JsonWriter writer, OperatorType value, JsonSerializerOptions options) =>
			writer.WriteStringValue(value.ToString());
	}
}
