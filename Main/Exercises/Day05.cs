using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day05 : Exercise
    {
        public override string Name => "Supply Stacks";

        public override string Description => "Move crates with a cargo crane";

        public override byte Day => 5;

        private string[] lines;

        private readonly List<Stack<char>> supplies = new();
        private readonly List<(int, int, int)> instructions = new();

        public override void SetUp()
        {
            lines = System.IO.File.ReadAllLines("Inputs/05.txt");
        }

        private void ParseLines()
        {
            supplies.Clear();
            instructions.Clear();


            var numbersLine = -1;
            var instructionRegex = new Regex(@"move (\d+) from (\d+) to (\d+)");

            for (int i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];
                if (numbersLine < 0)
                {
                    if (line.Contains('1'))
                    {
                        numbersLine = i++;
                        for (int j = 0; j < (line.Length + 1) / 4; ++j)
                            supplies.Add(new());
                    }

                    continue;
                }

                if (!line.StartsWith("move"))
                    throw new NotSupportedException();

                var parts = instructionRegex.Split(line);

                if (parts.Length != 5)
                    throw new NotSupportedException();

                instructions.Add((int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
            }

            for (int i = numbersLine - 1; i >= 0; --i)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j += 4)
                {
                    var stackIndex = j / 4;
                    var c = line[j + 1];

                    if (c < 'A' || c > 'Z')
                        continue;

                    supplies[stackIndex].Push(c);
                }
            }

            Console.WriteLine($"Processed {instructions.Count} instructions for {supplies.Count} supply stacks.");

        }

        public override object SolvePartOne()
        {
            ParseLines();

            foreach(var ins in instructions)
            {
                var count = ins.Item1;
                var from = ins.Item2 - 1;
                var to = ins.Item3 - 1;
                for(int i = 0; i < count; ++i)
                {
                    supplies[to].Push(supplies[from].Pop());
                }
            }

            
            return string.Concat(supplies.Select(stack => stack.Peek()));
        }

        public override object SolvePartTwo()
        {
            ParseLines();

            foreach (var ins in instructions)
            {
                var count = ins.Item1;
                var from = ins.Item2 - 1;
                var to = ins.Item3 - 1;

                Stack<char> cache = new();

                for (int i = 0; i < count; ++i)
                {
                    cache.Push(supplies[from].Pop());
                }

                while(cache.Count != 0)
                    supplies[to].Push(cache.Pop());
            }

            return string.Concat(supplies.Select(stack => stack.Peek()));
        }
    }
}
