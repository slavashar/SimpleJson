using System;
using System.Globalization;
using System.IO;

namespace SimpleJson
{
    public class JsonTextWriter
    {
        private readonly TextWriter writer;

        public JsonTextWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void WriteValueDelimiter()
        {
            this.writer.Write(',');
        }

        public void WriteStartObject()
        {
            this.writer.Write("{");
        }

        public void WriteEndObject()
        {
            this.writer.Write("}");
        }

        public void WriteStartArray()
        {
            this.writer.Write("[");
        }

        public void WriteEndArray()
        {
            this.writer.Write("]");
        }

        public void WritePropertyName(string name)
        {
            WriteEscapedJavaScriptString(this.writer, name, '"');
            this.writer.Write(":");
        }

        public void WriteNull()
        {
            this.writer.Write("null");
        }

        public void WriteValue(long value)
        {
            this.writer.Write(value.ToString(CultureInfo.InvariantCulture));
        }

        public void WriteValue(double value)
        {
            var text = value.ToString(CultureInfo.InvariantCulture);

            if (double.IsNaN(value) || double.IsInfinity(value) || (text.IndexOf('.') != -1 || text.IndexOf('E') != -1) || text.IndexOf('e') != -1)
            {
                this.writer.Write(text);
            }
            else
            {
                this.writer.Write(text + ".0");
            }
        }

        public void WriteValue(string value)
        {
            WriteEscapedJavaScriptString(this.writer, value, '"');
        }

        public void WriteValue(bool value)
        {
            this.writer.Write(value ? "true" : "false");
        }

        public void WriteValue(DateTimeOffset value)
        {
            string format;

            if (value.Offset.Ticks > 0)
            {
                format = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFzzz";
            }
            else if (value.Millisecond > 0)
            {
                format = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFF";
            }
            else if (value.Hour > 0 || value.Minute > 0 || value.Second > 0)
            {
                format = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
            }
            else
            {
                format = "yyyy'-'MM'-'dd";
            }

            var str = value.ToString(format, CultureInfo.InvariantCulture);

            this.writer.Write('"');
            this.writer.Write(str);
            this.writer.Write('"');
        }

        private static void WriteEscapedJavaScriptString(TextWriter writer, string s, char delimiter)
        {
            writer.Write(delimiter);

            if (s != null)
            {
                char[] buffer = null;
                var index1 = 0;
                for (int index2 = 0; index2 < s.Length; ++index2)
                {
                    char c = s[index2];
                    if (c < 32 || c >= 128 || (c == 92 || c == delimiter))
                    {
                        string str;
                        switch (c)
                        {
                            case '\\':
                                str = "\\\\";
                                break;
                            case '\x0085':
                                str = "\\u0085";
                                break;
                            case '\x2028':
                                str = "\\u2028";
                                break;
                            case '\x2029':
                                str = "\\u2029";
                                break;
                            case '\b':
                                str = "\\b";
                                break;
                            case '\t':
                                str = "\\t";
                                break;
                            case '\n':
                                str = "\\n";
                                break;
                            case '\f':
                                str = "\\f";
                                break;
                            case '\r':
                                str = "\\r";
                                break;
                            case '"':
                                str = "\\\"";
                                break;
                            case '\'':
                                str = "\\'";
                                break;
                            default:
                                str = (int)c <= 31 ? new string(new[] { '\\', 'u', IntToHex(c >> 12 & 15), IntToHex(c >> 8 & 15), IntToHex(c >> 4 & 15), IntToHex(c & 15) }) : null;
                                break;
                        }

                        if (str != null)
                        {
                            if (index2 > index1)
                            {
                                if (buffer == null)
                                {
                                    buffer = s.ToCharArray();
                                }

                                writer.Write(buffer, index1, index2 - index1);
                            }

                            index1 = index2 + 1;
                            writer.Write(str);
                        }
                    }
                }

                if (index1 == 0)
                {
                    writer.Write(s);
                }
                else
                {
                    if (buffer == null)
                    {
                        buffer = s.ToCharArray();
                    }

                    writer.Write(buffer, index1, s.Length - index1);
                }
            }

            writer.Write(delimiter);
        }

        private static char IntToHex(int n)
        {
            return n <= 9 ? (char)(n + 48) : (char)(n - 10 + 97);
        }
    }
}