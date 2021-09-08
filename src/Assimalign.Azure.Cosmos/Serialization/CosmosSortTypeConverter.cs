using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Serialization
{
    using Assimalign.Azure.Cosmos.Clauses;
    
    internal sealed class CosmosSortTypeConverter : JsonConverter<SortType>
    {
        public override SortType Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString().ToLower();
                if (Array.Exists(new[] { "asc", "ascending" }, x => x == stringValue))
                {
                    return SortType.Ascending;
                }
                if (Array.Exists(new[] { "desc", "descending" }, x => x == stringValue))
                {
                    return SortType.Descending;
                }
            }
            if (reader.TokenType == JsonTokenType.Number)
            {
                return (SortType)reader.GetInt32();
            }
            else
            {
                return SortType.Ascending;
            }
        }

        public override void Write(Utf8JsonWriter writer, SortType value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString().ToLower());
    }
}
