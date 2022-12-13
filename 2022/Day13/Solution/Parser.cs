using Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Solution
{
    [DebuggerDisplay("{Left} <=> {Right}")]
    public class ValuePair
    {
        public Value Left { get; set; }
        public Value Right { get; set; }

        public ValuePair(Value left, Value right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"{Left}\n{Right}";
        }
    }

    public abstract class Value
    { }

    [DebuggerDisplay("{Value}")]
    public class NumericValue : Value
    {
        public int Value { get; set; }
        public NumericValue(int value) => Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [DebuggerDisplay("{Values}")]
    public class ListValue : Value
    {
        public List<Value> Values { get; set; } = new();

        public ListValue() { }

        public ListValue(params Value[] values)
        {
            Values = values.ToList();
        }

        public override string ToString()
        {
            return "[" + string.Join(",", Values.Select(v => v.ToString())) + "]";
        }
    }

    public static class Parser
    {
        public static List<ValuePair> ParseInput(string input)
        {
            return input.ToSections().Select(ParsePairs).ToList();
        }

        public static ValuePair ParsePairs(string input)
        {
            var lines = input.ToLines();
            return new(ParseValue(lines[0]), ParseValue(lines[1]));
        }

        public static Value ParseValue(string input)
        {
            ListValue root = new();
            var values = new Stack<ListValue>();
            values.Push(root);

            input = input.Substring(1, input.Length - 2); // Strip the outer most brackets
            var reader = new CharacterReader(input);

            while (reader.CanRead)
            {
                var nextChar = reader.Peek();
                if (nextChar == '[')
                {
                    var newValue = new ListValue();
                    values.Peek().Values.Add(newValue);
                    values.Push(newValue);
                    reader.Read();
                }
                else if (nextChar == ']')
                {
                    values.Pop();
                    reader.Read();
                }
                else if (char.IsNumber(nextChar))
                {
                    var value = reader.ReadInt();
                    values.Peek().Values.Add(new NumericValue(value));
                }
                else if (nextChar == ',')
                {
                    reader.Read();
                }
            }

            return root;
        }
    }

    public class CharacterReader
    {
        private string _source;
        private int _index = 0;


        public CharacterReader(string input)
        {
            _source = input;
        }

        public bool CanRead => _index < _source.Length;
        public char Read()
        {
            var chr = _source[_index];
            _index++;
            return chr;
        }

        public char Peek()
        {
            return _source[_index];
        }

        public int ReadInt()
        {
            var str = new StringBuilder();
            while (CanRead)
            {
                if (char.IsNumber(Peek()))
                {
                    str.Append(Read());
                }
                else
                {
                    break;
                }
            }

            return int.Parse(str.ToString());
        }
    }
}
