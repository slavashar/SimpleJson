using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SimpleJson
{
    public class JsonTextReader
    {
        private readonly TextReader reader;

        private readonly char[] buffer = new char[5];

        private readonly Stack<Level> stack = new Stack<Level>();

        private char? newxtChar;
        private Token currentToken;
        private JsonValue? currentValue;
        
        public JsonTextReader(TextReader reader)
        {
            this.reader = reader;
            this.stack.Push(Level.Root);

            this.Read();
        }
        
        private enum Token
        {
            StartObject = 1,
            EndObject,

            StartArray,
            EndArray,

            PairName,

            Value
        }

        private enum Level
        {
            Root,
            Object,
            Array
        }

        public bool IsStartObject
        {
            get { return this.currentToken == Token.StartObject; }
        }

        public bool IsPairName
        {
            get { return this.currentToken == Token.PairName; }
        }

        public bool IsEndObject
        {
            get { return this.currentToken == Token.EndObject; }
        }

        public bool IsStartArray
        {
            get { return this.currentToken == Token.StartArray; }
        }

        public bool IsEndArray
        {
            get { return this.currentToken == Token.EndArray; }
        }

        public bool Read()
        {
            return this.InternalRead();
        }

        public IJsonMember GetJsonMember()
        {
            switch (this.currentToken)
            {
                case Token.Value:
                case Token.PairName:
                    return this.currentValue;

                case Token.StartObject:
                    return JsonObject.ReadFrom(this);

                case Token.StartArray:
                    return JsonArray.ReadFrom(this);

                default:
                    throw new InvalidOperationException(string.Format("The JsonReader should not be on a element of type {0}.", this.currentToken));
            }
        }
        
        private static string ReadEscapedJavaScriptString(TextReader reader, char delimiter)
        {
            var str = new List<char>();

            while (true)
            {
                var r = reader.Read();

                if (r == -1)
                {
                    throw new FormatException("Unexpected end of the JSON");
                }

                var c = (char)r;

                if (c == delimiter)
                {
                    return new string(str.ToArray());
                }

                str.Add(c == '\\' ? GetUnescapedCharacter(reader) : c);
            }
        }

        private static char GetUnescapedCharacter(TextReader reader)
        {
            var r = reader.Read();

            if (r == -1)
            {
                throw new FormatException("Unexpected end of the JSON");
            }

            var c2 = (char)r;

            switch (c2)
            {
                case '\\':
                    return '\\';

                case '\"':
                    return '\"';

                case '\'':
                    return '\'';

                case 'b':
                    return '\b';

                case 't':
                    return '\t';

                case 'n':
                    return '\n';

                case 'f':
                    return '\f';

                case 'r':
                    return '\r';
            }

            if (c2 == 'u')
            {
                var buffer = new char[4];
                if (reader.Read(buffer, 0, 4) == 4)
                {
                    return Convert.ToChar(int.Parse(new string(buffer, 0, 4), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
                }

                throw new FormatException("Unexpected end of the JSON");
            }

            throw new FormatException("Invalid JSON unescaped string character");
        }
        
        private static JsonValue ReadNumber(TextReader reader, ref char ch)
        {
            long result;
            int expResult = 0, fracResult = 0, decPlaces = 0;
            bool negative = false;
            bool? expNegative = null;
            bool frac = false, exp = false, number = true;

            if (ch == '-')
            {
                negative = true;
                result = 0;
            }
            else
            {
                result = ParceNumber(ch);
            }

            int r;

            while ((r = reader.Read()) >= 0)
            {
                ch = (char)r;
                
                switch (ch)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        number = true;
                        if (exp)
                        {
                            if (!expNegative.HasValue)
                            {
                                expNegative = expNegative.GetValueOrDefault(true);
                            }
                            
                            expResult = (expResult * 10) + ParceNumber(ch);
                        }
                        else if (frac)
                        {
                            fracResult = (fracResult * 10) + ParceNumber(ch);
                            decPlaces++;
                        }
                        else
                        {
                            result = (result * 10) + ParceNumber(ch);
                        }

                        break;

                    case '.':
                        if (frac || exp)
                        {
                            throw new FormatException("Invalid JSON number format.");
                        }

                        number = false;
                        frac = true;
                        break;

                    case 'E':
                    case 'e':
                        if (exp)
                        {
                            throw new FormatException("Invalid JSON number format.");
                        }

                        number = false;
                        exp = true;

                        break;
                    case '+':
                    case '-':
                        if (!exp || expNegative.HasValue)
                        {
                            throw new FormatException("Invalid JSON number format.");
                        }

                        expNegative = ch == '+';
                        break;

                    default:
                        if (!number)
                        {
                            throw new FormatException("Invalid JSON number format.");
                        }

                        if (frac || exp)
                        {
                            var v = ((double)result + ((double)fracResult / (decPlaces * 10))) * Math.Pow(10, expNegative.GetValueOrDefault() ? -1 * expResult : expResult);
                            return new JsonValue(negative ? -1 * v : v);
                        }

                        return new JsonValue(negative ? -1 * result : result);
                }
            }
            
            throw new FormatException("Unexpected end of the JSON.");
        }

        private static int ParceNumber(char digit)
        {
            switch (digit)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
            }

            throw new InvalidOperationException();
        }
        
        private bool InternalRead()
        {
            char ch;

            if (this.newxtChar.HasValue)
            {
                ch = this.newxtChar.Value;
                this.newxtChar = null;
            }
            else
            {
                var r = this.reader.Read();
                if (r == -1)
                {
                    return false;
                }

                ch = (char)r;
            }

            var level = this.stack.Peek();

            if (level == Level.Object && this.currentToken != Token.PairName)
            {
                switch (ch)
                {
                    case '}':
                        return this.Push(Token.EndObject);
                        
                    case '"':
                    case '\'':
                        var res = this.Push(Token.PairName, new JsonValue(ReadEscapedJavaScriptString(this.reader, ch)));
                        if (res)
                        {
                            var r = this.reader.Read();
                            if (r == -1)
                            {
                                throw new FormatException("Unexpected end of the JSON.");
                            }

                            if ((char)r != ':')
                            {
                                throw new FormatException("Invalid character after JSON object pair name.");
                            }
                        }

                        return res;

                    case ',':
                        {
                            var r = this.reader.Read();
                            if (r == -1)
                            {
                                throw new FormatException("Unexpected end of the JSON.");
                            }

                            ch = (char)r;

                            if (ch == '"')
                            {
                                goto case '"';
                            }

                            if (ch == '\'')
                            {
                                goto case '\'';
                            }
                            
                            goto default;
                        }

                    default:
                        throw new FormatException("Invalid JSON object character.");
                }
            }

            if (level == Level.Array)
            {
                switch (ch)
                {
                    case ']':
                        return this.Push(Token.EndArray);

                    case ',':
                        if (this.currentToken == Token.StartArray)
                        {
                            throw new FormatException("Invalid JSON array character.");
                        }

                        {
                            var r = this.reader.Read();
                            if (r == -1)
                            {
                                throw new FormatException("Unexpected end of the JSON.");
                            }

                            ch = (char)r;
                        }

                        break;

                    default:
                        if (this.currentToken != Token.StartArray)
                        {
                            throw new FormatException("Invalid JSON array character.");
                        }

                        break;
                }
            }

            switch (ch)
            {
                case '{':
                    return this.Push(Token.StartObject);

                case '[':
                    return this.Push(Token.StartArray);

                case '"':
                case '\'':
                    return this.Push(Token.Value, JsonValue.ParseString(ReadEscapedJavaScriptString(this.reader, ch)));

                case 't':
                case 'T':
                    this.buffer[0] = ch;
                    if (this.reader.Read(this.buffer, 1, 3) != 3)
                    {
                        throw new FormatException("Unexpected end of the JSON.");
                    }

                    if (new string(this.buffer, 0, 4).ToUpperInvariant() != "TRUE")
                    {
                        throw new FormatException("Invalid Boolean value.");
                    }

                    return this.Push(Token.Value, new JsonValue(true));

                case 'f':
                case 'F':
                    this.buffer[0] = ch;
                    if (this.reader.Read(this.buffer, 1, 4) != 4)
                    {
                        throw new FormatException("Unexpected end of the JSON.");
                    }

                    if (new string(this.buffer, 0, 5).ToUpperInvariant() != "FALSE")
                    {
                        throw new FormatException("Invalid Boolean value.");
                    }

                    return this.Push(Token.Value, new JsonValue(false));

                case 'n':
                case 'N':
                    this.buffer[0] = ch;
                    if (this.reader.Read(this.buffer, 1, 3) != 3)
                    {
                        throw new FormatException("Unexpected end of the JSON.");
                    }

                    if (new string(this.buffer, 0, 4).ToUpperInvariant() != "NULL")
                    {
                        throw new FormatException("Invalid null value.");
                    }

                    return this.Push(Token.Value, new JsonValue());

                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    var v = ReadNumber(this.reader, ref ch);
                    this.newxtChar = ch;
                    return this.Push(Token.Value, v);
            }

            throw new NotImplementedException();
        }

        private bool Push(Token element, JsonValue? value = null)
        {
            switch (element)
            {
                case Token.StartObject:
                    this.stack.Push(Level.Object);
                    break;

                case Token.EndObject:
                    if (this.stack.Pop() != Level.Object)
                    {
                        throw new FormatException("Invalid format");
                    }

                    break;

                case Token.StartArray:
                    this.stack.Push(Level.Array);
                    break;

                case Token.EndArray:
                    if (this.stack.Pop() != Level.Array)
                    {
                        throw new FormatException("Invalid format");
                    }

                    break;
            }

            this.currentToken = element;
            this.currentValue = value;

            return true;
        }
    }
}