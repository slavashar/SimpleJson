using System;
using System.Collections;

namespace SimpleJson
{
    public interface IJsonMember
    {
        JsonElement Kind { get; }

        void WriteTo(JsonTextWriter textWriter);
    }

    public static class JsonMemberExtensions
    {
        public static IJsonMember GetJsonMember(this object value)
        {
            if (value == null)
            {
                return new JsonValue();
            }
            
            if (value is string || value is char || value is Guid || value is Uri)
            {
                return new JsonValue(value.ToString());
            }

            if (value is DateTime)
            {
                return new JsonValue((DateTime)value);
            }

            if (value is DateTimeOffset)
            {
                return new JsonValue((DateTimeOffset)value);
            }

            if (value is long || value is int || (value is short || value is sbyte) || (value is ulong || value is uint || (value is ushort || value is byte)) || value is Enum)
            {
                return new JsonValue(Convert.ToInt64(value));
            }
            
            if (value is TimeSpan)
            {
                return new JsonValue(((TimeSpan)value).Ticks);
            }

            if (value is double || value is float || value is decimal)
            {
                return new JsonValue(Convert.ToDouble(value));
            }
            
            if (value is bool)
            {
                return new JsonValue((bool)value);
            }

            if (value is IEnumerable) 
            {
                return new JsonArray((IEnumerable)value);
            }

            return new JsonObject(value);
        }
    }
}