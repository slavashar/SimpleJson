using System;

using Xunit;
using Xunit.Extensions;

namespace SimpleJson.Facts
{
    public class JsonTextReaderFacts
    {
        [Theory]
        [InlineData("test", @"test")]
        [InlineData("te\"st", "te\\\"st")]
        [InlineData("te\nst", "te\\nst")]
        [InlineData("te\\st", "te\\\\st")]
        [InlineData("012\x0085345", "012\\u0085345")]
        [InlineData("тест", "тест")]
        public void check_json_string(string input, string json)
        {
            var jsonResult = new JsonObject(new { s = input }).ToString();
            jsonResult.ShouldEqual(string.Concat("{\"s\":\"", json, "\"}"));
            ((JsonValue)JsonObject.Parse(jsonResult)["s"]).Value.ShouldEqual(input);
        }
    }
}