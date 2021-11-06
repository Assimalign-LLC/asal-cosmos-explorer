using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Assimalign.Azure.Cosmos.Serialization
{


    internal sealed class CosmosQueryObjectConverter : JsonConverter<object>
    {
        public const string GuidFormat = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";
        public const string DateFormat = "";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override object Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True || 
                reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }


            if (reader.TokenType == JsonTokenType.Number)
            {
                var numberValue = Encoding.UTF8.GetString(reader.ValueSpan);

                // Let's make an assumption that all numbers with a '.' are floating point decimal
                // the expression pipeline should convert the type to the correct one base on the repository model
                if (numberValue.Contains('.') && reader.TryGetDecimal(out var deci))
                    return deci;

                else if (reader.TryGetInt32(out var int32))
                    return int32;

                else if (reader.TryGetInt64(out var int64))
                    return int64;
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                if (reader.TryGetGuid(out var guid))
                    return guid;

                if (DateTime.TryParse(stringValue, out var date))
                    return date;

                return stringValue;
            }
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                throw new JsonException("Object Types are not supported value parameters for queries");
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            switch(value)
            {
                case string str:
                    writer.WriteStringValue(str);
                    break;
                case Guid guid:
                    writer.WriteStringValue(guid);
                    break;
                case DateTime date:
                    writer.WriteStringValue(date);
                    break;
                case short int16:
                    writer.WriteNumberValue(int16);
                    break;
                case int int32:
                    writer.WriteNumberValue(int32);
                    break;
                case long int64:
                    writer.WriteNumberValue(int64);
                    break;
                case decimal deci:
                    writer.WriteNumberValue(deci);
                    break;
                case double dble:
                    writer.WriteNumberValue(dble);
                    break;
                case Single single:
                    writer.WriteNumberValue(single);
                    break;
                case bool boolean:
                    writer.WriteBooleanValue(boolean);
                    break;
            }
        }
    }
}
