using System;

using Xunit;

namespace SimpleJson.Facts
{
    public class JsonArrayFacts
    {
        [Fact]
        public void create_empty_array()
        {
            var array = new JsonArray();
            array.ToString().ShouldEqual("[]");
        }

        [Fact]
        public void parse_empty_array()
        {
            var array = JsonArray.Parse("[]");
            array.ShouldBeEmpty();
        }
        
        [Fact]
        public void create_array()
        {
            var array = new JsonArray(new[] { 1, 2, 3 });
            array.ToString().ShouldEqual("[1,2,3]");
        }

        [Fact]
        public void add_new_item_to_array()
        {
            var array = new JsonArray { new JsonValue(1), new JsonValue(2), new JsonValue(3) };
            array.ToString().ShouldEqual("[1,2,3]");
        }

        [Fact]
        public void parse_array()
        {
            var array = JsonArray.Parse("[1,2,3]");
            array.ShouldEqual(new IJsonMember[] { new JsonValue(1), new JsonValue(2), new JsonValue(3) });
        }

        [Fact]
        public void parse_json_array_without_openning_bracket()
        {
            Assert.Throws<FormatException>(() => JsonArray.Parse("1,2,3]")).Message.ShouldEqual("Error reading JSON array.");
        }

        [Fact]
        public void parse_json_array_without_closing_bracket()
        {
            Assert.Throws<FormatException>(() => JsonArray.Parse("[1,2,3")).Message.ShouldEqual("Unexpected end of the JSON.");
        }

        [Fact]
        public void parse_jsonwithout_closing_array_bracket()
        {
            // TODO: check the message
            Assert.Throws<FormatException>(() => JsonObject.Parse("{\"test\":[1,2,3}")).Message.ShouldEqual("Invalid JSON array character.");
        }

        [Fact]
        public void create_jagged_array()
        {
            var array = new JsonArray(new[] { new object[0], new object[] { 1 }, new object[] { 1, 2 } });
            array.ToString().ShouldEqual("[[],[1],[1,2]]");
        }

        [Fact]
        public void parse_jagged_array()
        {
            var array = JsonArray.Parse("[[],[1],[1,2]]");
            array.ShouldEqual(new IJsonMember[] { new JsonArray(), new JsonArray(new[] { 1 }), new JsonArray(new[] { 1, 2 }) });
        }

        [Fact]
        public void compare_empty_arrays()
        {
            JsonArray array1 = new JsonArray(), array2 = new JsonArray();
            array1.ShouldEqual(array2);
        }

        [Fact]
        public void compare_arrays()
        {
            JsonArray array1 = new JsonArray(new[] { 1, 2, 3 }), array2 = new JsonArray(new[] { 1, 2, 3 });
            array1.ShouldEqual(array2);
        }

        [Fact]
        public void compare_jagged_arrays()
        {
            JsonArray array1 = new JsonArray(new[] { new object[0], new object[] { 1 }, new object[] { 1, 2 } }), 
                      array2 = new JsonArray(new[] { new object[0], new object[] { 1 }, new object[] { 1, 2 } });
            array1.ShouldEqual(array2);
        }

        [Fact]
        public void compare_arrays_with_different_lenth()
        {
            JsonArray array1 = new JsonArray(new[] { 1, 2, 3 }), array2 = new JsonArray(new[] { 1, 2 });
            array1.ShouldNotEqual(array2);
        }

        [Fact]
        public void compare_arrays_with_different_items()
        {
            JsonArray array1 = new JsonArray(new[] { 1, 2, 3 }), array2 = new JsonArray(new[] { 3, 2, 1 });
            array1.ShouldNotEqual(array2);
        }
    }
}