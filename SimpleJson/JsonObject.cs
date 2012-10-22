using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SimpleJson
{
    public class JsonObject : DynamicObject, IJsonMember, IEnumerable<string>
    {
        private readonly PropertKeyedCollection prop = new PropertKeyedCollection();

        public JsonObject()
        {
        }

        public JsonObject(object value)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(value))
            {
                this[descriptor.Name] = descriptor.GetValue(value).GetJsonMember();
            }
        }

        public JsonElement Kind
        {
            get { return JsonElement.Object; }
        }

        public IJsonMember this[string propertyName]
        {
            get
            {
                var property = this.Property(propertyName);
                return property != null ? property.Value : null;
            }

            set
            {
                var property = this.Property(propertyName);
                if (property != null)
                {
                    property.Value = value;
                }
                else
                {
                    this.prop.Insert(this.prop.Count, new JsonPair(propertyName, value));
                }
            }
        }
        
        public static JsonObject Parse(string json)
        {
            using (var r = new StringReader(json))
            {
                return ReadFrom(new JsonTextReader(r));
            }
        }

        public static JsonObject ReadFrom(JsonTextReader reader)
        {
            var jobject = new JsonObject();

            if (!reader.IsStartObject)
            {
                throw new FormatException("Error reading JSON object from JsonReader.");
            }

            while (reader.Read())
            {
                if (reader.IsEndObject)
                {
                    return jobject;
                }

                if (!reader.IsPairName)
                {
                    throw new FormatException("Not expected type of element.");
                }

                var name = ((JsonValue)reader.GetJsonMember()).Value.ToString();

                if (!reader.Read())
                {
                    throw new FormatException("Unexpected end of the JSON.");
                }

                jobject[name] = reader.GetJsonMember();
            }

            throw new FormatException("Unexpected end of the JSON.");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var jmember = this[binder.Name];

            if (jmember is JsonValue)
            {
                result = ((JsonValue)jmember).Value;
                return true;
            }

            result = jmember;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value is IJsonMember ? (IJsonMember)value : value.GetJsonMember();
            return true;
        }

        public void WriteTo(JsonTextWriter textWriter)
        {
            textWriter.WriteStartObject();

            var first = true;
            foreach (JsonPair pr in this.prop)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    textWriter.WriteValueDelimiter();
                }

                pr.WriteTo(textWriter);
            }

            textWriter.WriteEndObject();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.prop.Properties == null ? Enumerable.Empty<string>().GetEnumerator() : this.prop.Properties.Keys.GetEnumerator();
        }

        public override string ToString()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                this.WriteTo(new JsonTextWriter(stringWriter));
                return stringWriter.ToString();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private JsonPair Property(string name)
        {
            if (this.prop.Properties == null)
            {
                return null;
            }

            if (name == null)
            {
                return null;
            }

            JsonPair pr;
            this.prop.Properties.TryGetValue(name, out pr);
            return pr;
        }

        private class JsonPair
        {
            public JsonPair(string name, IJsonMember value)
            {
                this.Name = name;
                this.Value = value;
            }

            public IJsonMember Value { get; set; }

            public string Name { get; private set; }

            public void WriteTo(JsonTextWriter textWriter)
            {
                textWriter.WritePropertyName(this.Name);
                this.Value.WriteTo(textWriter);
            }

            public override string ToString()
            {
                using (var writer = new StringWriter())
                {
                    this.WriteTo(new JsonTextWriter(writer));
                    return writer.GetStringBuilder().ToString();
                }
            }
        }

        private class PropertKeyedCollection : KeyedCollection<string, JsonPair>
        {
            public PropertKeyedCollection() : base(StringComparer.Ordinal)
            {
            }

            public IDictionary<string, JsonPair> Properties
            {
                get { return this.Dictionary; }
            }

            protected override string GetKeyForItem(JsonPair item)
            {
                return item.Name;
            }

            protected override void InsertItem(int index, JsonPair item)
            {
                if (this.Dictionary == null)
                {
                    base.InsertItem(index, item);
                }
                else
                {
                    this.Dictionary[this.GetKeyForItem(item)] = item;
                    this.Items.Insert(index, item);
                }
            }
        }
    }
}