using System;
using System.Globalization;
using System.IO;

namespace SimpleJson
{
    public struct JsonValue : IJsonMember
    {
        private readonly dynamic value;
        private readonly JsonElement kind;
        
        public JsonValue(string value)
        {
            if (value == null)
            {
                this.kind = JsonElement.Null;
                this.value = null;
            }
            else
            {
                this.value = value;
                this.kind = JsonElement.String;
            }
        }

        public JsonValue(DateTime value)
        {
            this.kind = JsonElement.String;
            this.value = value;
        }

        public JsonValue(DateTimeOffset value)
        {
            this.kind = JsonElement.String;
            this.value = value;
        }

        public JsonValue(long value)
        {
            this.kind = JsonElement.Number;
            this.value = value;
        }

        public JsonValue(double value)
        {
            this.kind = JsonElement.Number;
            this.value = value;
        }

        public JsonValue(bool value)
        {
            this.kind = JsonElement.Boolean;
            this.value = value;
        }

        public object Value
        {
            get { return this.value; }
        }

        public JsonElement Kind
        {
            get { return this.kind; }
        }

        public static bool operator ==(JsonValue value1, JsonValue value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(JsonValue value1, JsonValue value2)
        {
            return !value1.Equals(value2);
        }  

        public static JsonValue ParseString(string json)
        {
            if (json == null)
            {
                return new JsonValue();
            }

            DateTimeOffset dt;
            if (DateTimeOffset.TryParseExact(json, new[] { "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", "yyyy'-'MM'-'dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return new JsonValue(dt);
            }

            return new JsonValue(json);
        }

        public static JsonValue ParseNumber(string json)
        {
            if (json.IndexOf('.') != -1 || json.IndexOf('e') != -1)
            {
                return new JsonValue(double.Parse(json, CultureInfo.InvariantCulture));
            }

            return new JsonValue(long.Parse(json, CultureInfo.InvariantCulture));
        }

        public static JsonValue ParseBoolean(string json)
        {
            return new JsonValue(bool.Parse(json));
        }

        void IJsonMember.WriteTo(JsonTextWriter textWriter)
        {
            if (this.kind == JsonElement.Null)
            {
                textWriter.WriteNull();
            }
            else if (this.value is DateTime)
            {
                textWriter.WriteValue(Convert.ToDateTime(this.value, CultureInfo.InvariantCulture));
            }
            else
            {
                textWriter.WriteValue(this.value);
            }
        }
        
        public override string ToString()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                ((IJsonMember)this).WriteTo(new JsonTextWriter(stringWriter));
                return stringWriter.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JsonValue))
            {
                return false;
            }

            var jvalue = (JsonValue)obj;
            
            if (this.kind != jvalue.kind)
            {
                return false;
            }

            switch (this.kind)
            {
                case JsonElement.Null:
                    return true;

                case JsonElement.String:
                    return this.ToString().Equals(jvalue.ToString());

                case JsonElement.Number:
                    return Convert.ToDouble(this.value).Equals(Convert.ToDouble(jvalue.value));

                case JsonElement.Boolean:
                    return this.value.Equals(jvalue.value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.value == null ? 0 : this.value.GetHashCode();
        }
    }
}