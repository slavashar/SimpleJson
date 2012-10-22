using System;

using Xunit;

namespace SimpleJson.Facts
{
    public class JsonValueFacts
    {
        [Fact]
        public void create_null_json_value()
        {
            var value = new JsonValue();

            value.ToString().ShouldEqual("null");
            value.Kind.ShouldEqual(JsonElement.Null);
            value.Value.ShouldBeNull();
        }

        [Fact]
        public void compare_null_json_values()
        {
            JsonValue value1 = new JsonValue(), value2 = new JsonValue();
            value1.ShouldEqual(value2);
        }

        [Fact]
        public void create_int_json_value()
        {
            var value = new JsonValue(100);

            value.ToString().ShouldEqual("100");
            value.Kind.ShouldEqual(JsonElement.Number);
            value.Value.ShouldEqual(100L);
        }
        
        [Fact]
        public void convert_large_number_to_json()
        {
            var obj = new JsonObject();
            obj["value"] = new JsonValue(long.MaxValue);

            var json = obj.ToString();

            obj = JsonObject.Parse(json);
            ((JsonValue)obj["value"]).Value.ShouldEqual(long.MaxValue);
        }

        [Fact]
        public void compare_int_json_values()
        {
            JsonValue value1 = new JsonValue(100), value2 = new JsonValue(100);
            value1.ShouldEqual(value2);
        }

        [Fact]
        public void compare_diff_int_json_values()
        {
            JsonValue value1 = new JsonValue(100), value2 = new JsonValue(110);
            value1.ShouldNotEqual(value2);
        }

        [Fact]
        public void create_float_json_value()
        {
            var value = new JsonValue(1.1);

            value.ToString().ShouldEqual("1.1");
            value.Kind.ShouldEqual(JsonElement.Number);
            value.Value.ShouldEqual(1.1D);
        }

        [Fact]
        public void persist_decimal_point_for_float_json_values()
        {
            var value = new JsonValue(10.000);
            value.ToString().ShouldEqual("10.0");
        }

        [Fact]
        public void persist_exp_for_float_json_values()
        {
            var value = new JsonValue(1e100);
            value.ToString().ShouldEqual("1E+100");
        }

        [Fact]
        public void compare_float_json_values()
        {
            JsonValue value1 = new JsonValue(1.1), value2 = new JsonValue(1.1);
            value1.ShouldEqual(value2);
        }

        [Fact]
        public void compare_diff_float_json_values()
        {
            JsonValue value1 = new JsonValue(1.1), value2 = new JsonValue(1.2);
            value1.ShouldNotEqual(value2);
        }

        [Fact]
        public void compare_float_and_int_json_values()
        {
            JsonValue value1 = new JsonValue(10.0), value2 = new JsonValue(10);
            value1.ShouldEqual(value2);
        }

        [Fact]
        public void compare_diff_float_and_int_json_values()
        {
            JsonValue value1 = new JsonValue(10.1), value2 = new JsonValue(10);
            value1.ShouldNotEqual(value2);
        }

        [Fact]
        public void create_string_json_value()
        {
            var value = new JsonValue("test");

            value.ToString().ShouldEqual("\"test\"");
            value.Kind.ShouldEqual(JsonElement.String);
            value.Value.ShouldEqual("test");
        }

        [Fact]
        public void create_date_json_value()
        {
            var value = new JsonValue(new DateTime(2000, 1, 1));

            value.ToString().ShouldEqual("\"2000-01-01\"");
            value.Kind.ShouldEqual(JsonElement.String);
            value.Value.ShouldEqual(new DateTime(2000, 1, 1));
        }

        [Fact]
        public void create_datetime_json_value()
        {
            var value = new JsonValue(new DateTime(2000, 1, 1, 12, 59, 59));

            value.ToString().ShouldEqual("\"2000-01-01T12:59:59\"");
            value.Kind.ShouldEqual(JsonElement.String);
            value.Value.ShouldEqual(new DateTime(2000, 1, 1, 12, 59, 59));
        }

        [Fact]
        public void create_datetime_offset_json_value()
        {
            var value = new JsonValue(new DateTimeOffset(2000, 1, 1, 12, 59, 59, new TimeSpan(4, 0, 0)));

            value.ToString().ShouldEqual("\"2000-01-01T12:59:59+04:00\"");
            value.Kind.ShouldEqual(JsonElement.String);
            value.Value.ShouldEqual(new DateTimeOffset(2000, 1, 1, 12, 59, 59, new TimeSpan(4, 0, 0)));
        }

        [Fact]
        public void compare_string_json_values()
        {
            JsonValue value1 = new JsonValue("test"), value2 = new JsonValue("test");
            value1.ShouldEqual(value2);
        }

        [Fact]
        public void compare_diff_string_json_values()
        {
            JsonValue value1 = new JsonValue("test"), value2 = new JsonValue("tset");
            value1.ShouldNotEqual(value2);
        }

        [Fact]
        public void compare_string_and_data_json_values()
        {
            JsonValue value1 = new JsonValue("2000-01-01"), value2 = new JsonValue(new DateTime(2000, 1, 1));
            value1.ShouldEqual(value2);
        }

        [Fact]
        public void create_boolean_json_value()
        {
            var value = new JsonValue(true);

            value.ToString().ShouldEqual("true");
            value.Kind.ShouldEqual(JsonElement.Boolean);
            value.Value.ShouldEqual(true);
        }
    }
}