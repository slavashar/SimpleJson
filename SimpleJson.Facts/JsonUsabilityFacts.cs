using Xunit;

namespace SimpleJson.Facts
{
    public class JsonUsabilityFacts
    {
        [Fact]
        public void general_case()
        {
            var jobject = new JsonObject();
            jobject["name"] = new JsonValue("test");
            jobject["value"] = new JsonValue(1);
            jobject["list"] = new JsonArray { new JsonValue(5), new JsonValue("test") };

            var json = jobject.ToString();

            json.ShouldEqual("{\"name\":\"test\",\"value\":1,\"list\":[5,\"test\"]}");
        }

        [Fact]
        public void dynamic_case()
        {
            dynamic jobject = new JsonObject();
            jobject.name = "test";
            jobject.value = 1;
            jobject.list = new object[] { 5, "test" };

            string json = jobject.ToString();

            json.ShouldEqual("{\"name\":\"test\",\"value\":1,\"list\":[5,\"test\"]}");
        }

        [Fact]
        public void anonymous_case()
        {
            var obj = new
                {
                    name = "test", 
                    value = 1, 
                    list = new object[] { 5, "test" }
                };

            var json = new JsonObject(obj).ToString();

            json.ShouldEqual("{\"name\":\"test\",\"value\":1,\"list\":[5,\"test\"]}");
        }

        [Fact]
        public void temp()
        {
            dynamic jobject = JsonObject.Parse("{\"var1\":3,\"var2\":5}");

            jobject.sum = jobject.var1 + jobject.var2;

            string json = jobject.ToString();

            json.ShouldEqual("{\"var1\":3,\"var2\":5,\"sum\":8}");
        }
    }
}
