using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{

    public class Day13 : Exercise
    {

        private abstract class Value
        {
            public abstract string Raw { get; set; }

            public override string ToString() => Raw;
        }

        private class IntValue : Value
        {
            public int Value { get; set; }

            public override string Raw { get { return Value.ToString(); } set { throw new NotSupportedException(); } }
        }

        private class ListValue : Value
        {
            public List<Value> Subvalues { get; private set; } = new();

            public override string Raw { get; set; }
        }

        private const bool LOG = false;

        public override string Name => "Distress Signal";

        public override string Description => "Sort packets so they are in order.";

        public override byte Day => 13;

        string[] lines;

        public override void SetUp()
        {
            lines = System.IO.File.ReadAllLines("Inputs/13.txt");
        }


        private List<ListValue> Parse()
        {
            List<ListValue> values = new();

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Trim().Length == 0)
                    continue;
                var first = ParseLine(lines[i]);
                values.Add(first);
            }

            return values;
        }

        private ListValue ParseLine(string str)
        {
            ListValue value = new();
            ParseList(ref value, str, 0);
            return value;
        }

        private int CountBracketIndex(string str, int index)
        {
            int brackets = 0;

            for(int i = index; i < str.Length; ++i)
            {
                var c = str[i];
                if (c == '[')
                    ++brackets;
                else if(c == ']')
                {
                    --brackets;
                    if (brackets == 0)
                        return i;
                }
            }

            return str.Length - 1;
        }

        private int ParseList(ref ListValue value, string str, int index)
        {
            var closed = CountBracketIndex(str, index);
            value.Raw = str.Substring(index, closed - index+1);
            ++index; //Skip opening bracket
            while (index < str.Length)
            {
                var c = str[index];
                if (c == '[')
                {
                    ListValue next = new();
                    index = ParseList(ref next, str, index);
                    value.Subvalues.Add(next);
                }
                else if (c == ']')
                {
                    ++index;
                    return index;
                }
                else if (c == ',')
                {
                    ++index;
                }
                else
                {
                    var nextComma = str.IndexOf(',', index);
                    var nextClosed = str.IndexOf(']', index);

                    var until = nextComma;
                    if (nextComma == -1 || nextClosed < nextComma)
                        until = nextClosed;

                    var sub = str.Substring(index, until - index);
                    var num = int.Parse(sub);
                    index = until;
                    value.Subvalues.Add(new IntValue { Value = num });
                }
            }

            throw new NotSupportedException();
        }

        private int CompareInt(IntValue left, IntValue right, int indent)
        {
            if (left.Value < right.Value)
            {
                if (LOG)
                    Utils.WriteIndent("- Left side is smaller, so inputs are in the right order", indent);
                return -1;
            }

            if (left.Value > right.Value)
            {
                if (LOG)
                    Utils.WriteIndent("- Right side is smaller, so inputs are not in the right order", indent);
                return 1;
            }

            return 0;
        }

        private int Compare(ListValue left, ListValue right, int indent = 0)
        {
            if (LOG) Utils.WriteIndent($"- Comparing {left} and {right}", indent);
            indent += 2;
            int index = 0;
            while (true)
            {
                var leftOut = index == left.Subvalues.Count;
                var rightOut = index == right.Subvalues.Count;
                if (leftOut)
                {
                    if (!rightOut)
                    {
                        if(LOG) Utils.WriteIndent("- Left side ran out of items, so inputs are in the right order", indent);
                        return -1;
                    }
                    return 0;
                }
                else if (rightOut)
                {
                    if (LOG) Utils.WriteIndent("- Right side ran out of items, so inputs are not in the right order", indent);
                    return 1;
                }

                Value leftValue = left.Subvalues[index];
                Value rightValue = right.Subvalues[index];

                if (LOG) Utils.WriteIndent($"- Compare {leftValue} vs {rightValue}", indent);

                if (leftValue.GetType() != rightValue.GetType())
                {
                    if (leftValue is IntValue)
                    {
                        if (LOG) Utils.WriteIndent($"- Mixed types; convert left to [{leftValue}] and retry comparison", indent);
                        ListValue val = new();
                        val.Subvalues.Add(new IntValue { Value = (leftValue as IntValue).Value });
                        left.Subvalues[index] = val;
                        leftValue = val;
                    }
                    else
                    {
                        if (LOG) Utils.WriteIndent($"- Mixed types; convert right to [{rightValue}] and retry comparison", indent);
                        ListValue val = new();
                        val.Subvalues.Add(new IntValue { Value = (rightValue as IntValue).Value });
                        right.Subvalues[index] = val;
                        rightValue = val;
                    }
                }

                if (leftValue is IntValue && rightValue is IntValue)
                {
                    var compare = CompareInt(leftValue as IntValue, rightValue as IntValue, indent + 2);
                    if (compare != 0)
                        return compare;
                }
                else if (leftValue is ListValue && rightValue is ListValue)
                {
                    var compare = Compare(leftValue as ListValue, rightValue as ListValue, indent + 2);
                    if (compare != 0)
                        return compare;
                }

                ++index;
            }
        }

        public override object SolvePartOne()
        {
            var values = Parse();

            var index = 1;
            var result = 0;

            for (int i = 0; i < values.Count; i+=2)
            {
                var first = values[i];
                var second = values[i + 1];
                //Console.WriteLine($"\n== Pair {index} ==");
                var compare = Compare(first, second);
                if (compare < 1)
                {
                    result += index;
                }

                ++index;
            }

            return result;
        }

        public override object SolvePartTwo()
        {
            var values = Parse();

            var a = ParseLine("[[2]]");
            var b = ParseLine("[[6]]");

            values.Add(a);
            values.Add(b);

            values.Sort((a, b) => Compare(a, b));

            if(LOG)
            {
                foreach (var val in values)
                    Console.WriteLine(val);
            }

            var first = values.IndexOf(a)+1;
            var second = values.IndexOf(b)+1;
            return first*second;
        }
    }
}
