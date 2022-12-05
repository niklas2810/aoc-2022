using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day03 : Exercise
    {
        public override string Name => "Rucksack Reorganization";

        public override string Description => "Find out which item is in both compartment of a rucksack";

        public override byte Day => 3;

        private List<string> backpacks = new();

        public override void SetUp()
        {
            backpacks.Clear();
            string[] lines = System.IO.File.ReadAllLines("Inputs/03.txt");

            foreach (var line in lines)
            {
                backpacks.Add(line);
            }
        }

        public override object SolvePartOne()
        {
            long result = 0;

            foreach (var line in backpacks)
            {
                string first = line.Substring(0, line.Length / 2);
                string second = line.Substring(line.Length / 2);
                var items = first.ToCharArray().Where(c => second.Contains(c)).Distinct().ToArray();
                if (items.Length != 1)
                    throw new NotSupportedException();

                result += DetermineValueForChar(items[0]);
            }

            return result;
        }

        private long DetermineValueForChar(char v)
        {
            if (v >= 'a' && v <= 'z')
                return v - 'a' + 1;

            if (v >= 'A' && v <= 'Z')
                return v - 'A' + 27;

            throw new NotSupportedException();
        }

        public override object SolvePartTwo()
        {
            long result = 0;

            for (int i = 0; i < backpacks.Count; i+=3)
            {
                var first = backpacks[i];
                var second = string.Concat(backpacks[i + 1].ToCharArray().Where(c => first.Contains(c)));
                var third = string.Concat(backpacks[i + 2].ToCharArray().Where(c => second.Contains(c)).Distinct());

                if (third.Length != 1)
                    throw new NotSupportedException();

                result += DetermineValueForChar(third[0]);
            }

            return result;
        }
    }
}
