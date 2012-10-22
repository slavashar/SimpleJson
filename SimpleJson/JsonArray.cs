using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SimpleJson
{
    public class JsonArray : IJsonMember, IEnumerable<IJsonMember>
    {
        private readonly IList<IJsonMember> values = new List<IJsonMember>();

        public JsonArray()
        {
        }

        public JsonArray(IEnumerable values)
        {
            foreach (var v in values)
            {
                this.values.Add(v.GetJsonMember());
            }
        }

        public JsonElement Kind
        {
            get { return JsonElement.Array; }
        }

        public static JsonArray Parse(string json)
        {
            using (var r = new StringReader(json))
            {
                return ReadFrom(new JsonTextReader(r));
            }
        }

        public static JsonArray ReadFrom(JsonTextReader reader)
        {
            if (!reader.IsStartArray)
            {
                throw new FormatException("Error reading JSON array.");
            }

            var array = new JsonArray();

            while (reader.Read())
            {
                if (reader.IsEndArray)
                {
                    return array;
                }

                array.Add(reader.GetJsonMember());
            }

            throw new FormatException("Unexpected end of the JSON");
        }

        public void Add(IJsonMember member)
        {
            this.values.Add(member);
        }

        public void WriteTo(JsonTextWriter textWriter)
        {
            textWriter.WriteStartArray();

            var first = true;

            foreach (var member in this.values)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    textWriter.WriteValueDelimiter();
                }
                
                member.WriteTo(textWriter);
            }
            
            textWriter.WriteEndArray();
        }
        
        public IEnumerator<IJsonMember> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                this.WriteTo(new JsonTextWriter(writer));
                return writer.GetStringBuilder().ToString();
            }
        }

        public override bool Equals(object obj)
        {
            var ar = obj as JsonArray;

            if (ar == null || this.values.Count != ar.values.Count)
            {
                return false;
            }

            for (int i = 0; i < this.values.Count; i++)
            {
                if (!this.values[i].Equals(ar.values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var result = 0;

            foreach (var item in this.values)
            {
                unchecked
                {
                    result += item.GetHashCode();
                }
            }

            return result;
        }
    }
}