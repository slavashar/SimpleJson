using System;
using System.Linq;

using Xunit;

namespace SimpleJson.Facts
{
    public class JsonObjectTests
    {
        [Fact]
        public void create_json_object()
        {
            var jobject = new JsonObject();
            jobject.ToString().ShouldEqual("{}");
        }

        [Fact]
        public void parse_empty_json()
        {
            var obj = JsonObject.Parse("{}");

            obj.Count().ShouldEqual(0);
        }

        [Fact]
        public void create_json_object_with_a_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonObject();
            jobject.ToString().ShouldEqual("{\"test\":{}}");
        }

        [Fact]
        public void parse_json_with_nested_object()
        {
            var obj = JsonObject.Parse("{\"test\":{}}");

            obj.Count().ShouldEqual(1);
            ((JsonObject)obj["test"]).Count().ShouldEqual(0);
        }

        [Fact]
        public void parse_json_without_openning_bracket()
        {
            Assert.Throws<FormatException>(() => JsonObject.Parse("\"test\":0}")).Message.ShouldEqual("Error reading JSON object from JsonReader.");
        }

        [Fact]
        public void parse_json_without_closing_bracket()
        {
            Assert.Throws<FormatException>(() => JsonObject.Parse("{\"test\":0")).Message.ShouldEqual("Unexpected end of the JSON.");
        }

        [Fact]
        public void create_json_object_with_null_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue();
            jobject.ToString().ShouldEqual("{\"test\":null}");
        }

        [Fact]
        public void parse_json_with_null_property()
        {
            var obj = JsonObject.Parse("{\"test\":null}");

            obj.Count().ShouldEqual(1);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.Null);
        }

        [Fact]
        public void create_json_object_with_string_type_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue("test");
            jobject.ToString().ShouldEqual("{\"test\":\"test\"}");
        }
        
        [Fact]
        public void parse_json_with_string_property()
        {
            var obj = JsonObject.Parse("{\"test\":\"test\"}");

            obj.Count().ShouldEqual(1);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual("test");
        }

        [Fact]
        public void create_json_object_with_integer_type_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue(10);
            jobject.ToString().ShouldEqual("{\"test\":10}");
        }

        [Fact]
        public void parse_json_with_int_property()
        {
            var obj = JsonObject.Parse("{\"test\":1}");

            obj.Count().ShouldEqual(1);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.Number);
            ((JsonValue)obj["test"]).Value.ShouldEqual(1L);
        }

        [Fact]
        public void create_json_object_with_float_type_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue(10F);
            jobject.ToString().ShouldEqual("{\"test\":10.0}");
        }

        [Fact]
        public void parse_json_with_decimal_property()
        {
            var obj = JsonObject.Parse("{\"test\":10.0}");

            obj.Count().ShouldEqual(1);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.Number);
            ((JsonValue)obj["test"]).Value.ShouldEqual(10D);
        }

        [Fact]
        public void create_json_object_with_true_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue(true);
            jobject.ToString().ShouldEqual("{\"test\":true}");
        }

        [Fact]
        public void parse_json_with_boolean_property()
        {
            var obj = JsonObject.Parse("{\"test\":true}");

            obj.Count().ShouldEqual(1);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.Boolean);
            ((JsonValue)obj["test"]).Value.ShouldEqual(true);
        }

        [Fact]
        public void create_json_object_with_false_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue(false);
            jobject.ToString().ShouldEqual("{\"test\":false}");
        }

        [Fact]
        public void create_json_object_with_date_type_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue(new DateTime(2012, 3, 1));
            jobject.ToString().ShouldEqual("{\"test\":\"2012-03-01\"}");
        }

        [Fact]
        public void parse_json_with_with_date_type_property()
        {
            var obj = JsonObject.Parse("{\"test\":\"2012-03-01\"}");

            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual(new DateTimeOffset(new DateTime(2012, 3, 1)));
        }

        [Fact]
        public void parse_json_with_with_date_time_type_property()
        {
            var obj = JsonObject.Parse("{\"test\":\"2012-03-01T05:10:45\"}");

            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual(new DateTimeOffset(new DateTime(2012, 3, 1, 5, 10, 45)));
        }

        [Fact]
        public void parse_json_with_with_date_time_2_type_property()
        {
            var obj = JsonObject.Parse("{\"test\":\"2012-03-01T05:10:45.678\"}");

            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual(new DateTimeOffset(new DateTime(2012, 3, 1, 5, 10, 45, 678)));
        }

        [Fact]
        public void parse_json_with_with_date_time_offset_type_property()
        {
            var obj = JsonObject.Parse("{\"test\":\"2012-03-01T05:10:45.678+05:00\"}");

            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual(new DateTimeOffset(2012, 3, 1, 5, 10, 45, 678, new TimeSpan(5, 0, 0)));
        }

        [Fact]
        public void create_json_object_with_array_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonArray(new[] { 1, 2, 3 });
            jobject.ToString().ShouldEqual("{\"test\":[1,2,3]}");
        }

        [Fact]
        public void create_json_object_with_array_of_mix_types_property()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonArray(new object[] { 1, "my", true });
            jobject.ToString().ShouldEqual("{\"test\":[1,\"my\",true]}");
        }

        [Fact]
        public void create_json_object_with_empty_array()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonArray(new object[0]);
            jobject.ToString().ShouldEqual("{\"test\":[]}");
        }

        [Fact]
        public void create_json_object_with_array_with_only_one_element()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonArray(new object[] { 5 });
            jobject.ToString().ShouldEqual("{\"test\":[5]}");
        }

        [Fact]
        public void crete_json_with_properties()
        {
            var jobject = new JsonObject();
            jobject["test"] = new JsonValue("test");
            jobject["my_val"] = new JsonValue(10);
            jobject.ToString().ShouldEqual("{\"test\":\"test\",\"my_val\":10}");
        }

        [Fact]
        public void parse_object_with_many_properties()
        {
            var obj = JsonObject.Parse("{\"test\":\"test\",\"my_val\":10}");

            obj.Count().ShouldEqual(2);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual("test");
            ((JsonValue)obj["my_val"]).Kind.ShouldEqual(JsonElement.Number);
            ((JsonValue)obj["my_val"]).Value.ShouldEqual(10L);
        }

        [Fact]
        public void parse_object_with_many_properties_2()
        {
            var obj = JsonObject.Parse("{\"test\":\"test\",\"my_val\":[1,2,3,4,5]}");

            obj.Count().ShouldEqual(2);
            ((JsonValue)obj["test"]).Kind.ShouldEqual(JsonElement.String);
            ((JsonValue)obj["test"]).Value.ShouldEqual("test");
            ((JsonArray)obj["my_val"]).Count().ShouldEqual(5);
            ((JsonArray)obj["my_val"]).Select(x => (long)((JsonValue)x).Value).Sum().ShouldEqual(15);
        }
    }
}