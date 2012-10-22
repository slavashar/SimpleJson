using System;
using System.Globalization;

using Xunit;

namespace SimpleJson.Facts
{
    public class JsonMemberFact
    {
        [Fact]
        public void convert_null_to_jmembe()
        {
            var jmember = ((object)null).GetJsonMember();
            jmember.ToString().ShouldEqual("null");
        }

        [Fact]
        public void convert_string_to_jmember()
        {
            var jmember = "test".GetJsonMember();
            jmember.ToString().ShouldEqual("\"test\"");
        }

        [Fact]
        public void convert_date_to_jmember()
        {
            var jmember = new DateTime(2010, 01, 01).GetJsonMember();
            jmember.ToString().ShouldEqual("\"2010-01-01\"");
        }

        [Fact]
        public void convert_date_offset_to_jmember()
        {
            var jmember = new DateTimeOffset(2010, 01, 01, 12, 0, 0, new TimeSpan(4, 0, 0)).GetJsonMember();
            jmember.ToString().ShouldEqual("\"2010-01-01T12:00:00+04:00\"");
        }

        [Fact]
        public void convert_int_to_jmembe()
        {
            var jmember = 100.GetJsonMember();
            jmember.ToString().ShouldEqual("100");
        }

        [Fact]
        public void convert_timespan_to_jmember()
        {
            var jmember = new TimeSpan(1, 2, 3).GetJsonMember();
            jmember.ToString().ShouldEqual((((((1 * 60) + 2) * 60) + 3) * 10000000L).ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void convert_double_to_jmember()
        {
            var jmember = 1.5.GetJsonMember();
            jmember.ToString().ShouldEqual("1.5");
        }

        [Fact]
        public void convert_boolean_to_jmember()
        {
            var jmember = true.GetJsonMember();
            jmember.ToString().ShouldEqual("true");
        }

        [Fact]
        public void convert_collection_to_jmember()
        {
            var jmember = new object[] { 1, "test" }.GetJsonMember();
            jmember.ToString().ShouldEqual("[1,\"test\"]");
        }

        [Fact]
        public void convert_object_to_jmember()
        {
            var jmember = new { value = 1, name = "test" }.GetJsonMember();
            jmember.ToString().ShouldEqual("{\"value\":1,\"name\":\"test\"}");
        }
    }
}